using System.Data;

namespace Corvus.Nest.Backend.Interfaces.IHelpers;

public interface IDatabaseHelper
{
    public Task<IEnumerable<T>> SqlQueryAsync<T>(string queryStr, object? parameters = null, int timeout = 36) where T : new();

    public Task<IEnumerable<dynamic>> SqlQueryAsync(string queryStr, object? parameters = null, int timeout = 36);

    public Task<int> SqlNonQueryAsync(string sqlStr, object? parameters = null, int timeout = 36);

    public Task<bool> SqlBulkCopyInsert(DataTable dt, List<string[]> columns, string dbName);

    public Task<DataTable> SqlQueryDTAsync(string sqlStr, object? parameters = null, string? tableName = null);
}
