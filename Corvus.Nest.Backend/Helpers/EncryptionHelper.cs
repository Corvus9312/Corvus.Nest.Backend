using Corvus.Nest.Backend.Interfaces.IHelpers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace Corvus.Nest.Backend.Helpers;

public class EncryptionHelper(
    IHttpClientHelper httpClient,
    IConfiguration config) : IEncryptionHelper
{
    private readonly Encoding _encoding = Encoding.UTF8;

    public async Task<string> HMACSHA256(string text, Encoding? encoding = null, string? key = null)
    {
        var encryptionServiceLink = config.GetValue<string>("Apis:EncryptionService") ?? throw new NullReferenceException("EncryptionService link is null");

        var builder = new UriBuilder(Path.Combine(encryptionServiceLink, "HMACSHA256"));

        var query = HttpUtility.ParseQueryString(builder.Query);
        query.Add("text", $"{text}");
        if (encoding is not null) query.Add("encoding", encoding.EncodingName);
        if (!string.IsNullOrWhiteSpace(key)) query.Add("key", $"{key}");

        builder.Query = HttpUtility.UrlEncode(query.ToString());
        var uri = builder.ToString();

        return await httpClient.HttpGetAsync(uri);
    }

    public string Base64ToString(byte[] bytes) => Convert.ToBase64String(bytes);

    public byte[] StringToBase64(string str) => Convert.FromBase64String(str);

    public async Task<string> EncryptDES(string text, string? key = null)
    {
        var encryptionServiceLink = config.GetValue<string>("Apis:EncryptionService") ?? throw new NullReferenceException("EncryptionService link is null");

        var builder = new UriBuilder(Path.Combine(encryptionServiceLink, "Encrypt"));

        var query = HttpUtility.ParseQueryString(builder.Query);
        query.Add("text", $"{text}");
        if (!string.IsNullOrWhiteSpace(key)) query.Add("key", $"{key}");

        builder.Query = HttpUtility.UrlEncode(query.ToString());
        var uri = builder.ToString();

        return await httpClient.HttpGetAsync(uri);
    }

    public async Task<string> DecryptDES(string text, string? key = null)
    {
        var encryptionServiceLink = config.GetValue<string>("Apis:EncryptionService") ?? throw new NullReferenceException("EncryptionService link is null");

        var builder = new UriBuilder(Path.Combine(encryptionServiceLink, "Decrypt"));

        var query = HttpUtility.ParseQueryString(builder.Query);
        query.Add("text", $"{text}");
        if (!string.IsNullOrWhiteSpace(key)) query.Add("key", $"{key}");

        builder.Query = query.ToString();
        var uri = builder.ToString();

        return await httpClient.HttpGetAsync(uri);
    }
}
