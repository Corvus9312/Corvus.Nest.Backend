using Corvus.Nest.Backend.Interfaces.IHelpers;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Corvus.Nest.Backend.Helpers;

public class DatabaseHelper(
    IConfiguration config, 
    IEncryptionHelper encryption
) : IDatabaseHelper
{
    protected virtual async Task<string?> GetConnStr()
    {
        var base64Conn = config.GetConnectionString("CorvusDatabase");

        if (string.IsNullOrWhiteSpace(base64Conn))
            throw new ArgumentNullException("ConnectionString is null");

        return await encryption.DecryptDES(base64Conn);
    }

    public async Task<IEnumerable<T>> SqlQueryAsync<T>(string queryStr, object? parameters = null, int timeout = 36) where T : new()
    {
        var sqlConn = await GetConnStr();

        using var conn = new SqlConnection(sqlConn);

        return await conn.QueryAsync<T>(queryStr, parameters, commandTimeout: timeout);
    }

    public async Task<IEnumerable<dynamic>> SqlQueryAsync(string queryStr, object? parameters = null, int timeout = 36)
    {
        return await SqlQueryAsync<dynamic>(queryStr, parameters, timeout);
    }

    public async Task<int> SqlNonQueryAsync(string sqlStr, object? parameters = null, int timeout = 36)
    {
        var continueOnCapturedContext = false;
        var sqlConn = await GetConnStr();

        using var conn = new SqlConnection(sqlConn);
        await conn.OpenAsync().ConfigureAwait(continueOnCapturedContext);

        await using var transaction = await conn.BeginTransactionAsync();

        try
        {
            var result = await conn.ExecuteAsync(sqlStr, parameters, transaction, commandTimeout: timeout).ConfigureAwait(continueOnCapturedContext);
            await transaction.CommitAsync().ConfigureAwait(continueOnCapturedContext);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync().ConfigureAwait(continueOnCapturedContext);
            throw;
        }
    }

    public async Task<bool> SqlBulkCopyInsert(DataTable dt, List<string[]> columns, string dbName)
    {
        var sqlConn = await GetConnStr();

        using var conn = new SqlConnection(sqlConn);
        conn.Open();

        using SqlBulkCopy sqlBC = new(conn);
        //設定一個批次量寫入多少筆資料
        sqlBC.BatchSize = 1000;
        //設定逾時的秒數
        sqlBC.BulkCopyTimeout = 7200;

        //設定 NotifyAfter 屬性，以便在每複製 10000 個資料列至資料表後，呼叫事件處理常式。
        sqlBC.NotifyAfter = 10000;

        //設定要寫入的資料庫
        sqlBC.DestinationTableName = dbName;

        //對應資料行
        foreach (string[] column in columns)
        {
            sqlBC.ColumnMappings.Add(column[0], column[1]);
        }

        //開始寫入
        sqlBC.WriteToServer(dt);
        return true;
    }

    public async Task<DataTable> SqlQueryDTAsync(string sqlStr, object? parameters = null, string? tableName = null)
    {
        var sqlConn = await GetConnStr();

        using SqlConnection conn = new(sqlConn);

        var dr = await conn.ExecuteReaderAsync(sqlStr, parameters);

        DataTable dt = new(tableName);

        dt.Load(dr);

        return dt;
    }
}
