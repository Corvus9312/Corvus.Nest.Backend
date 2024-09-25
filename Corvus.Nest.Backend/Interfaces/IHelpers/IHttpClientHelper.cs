using Corvus.Nest.Backend.Helpers;
using Corvus.Nest.Backend.Models.BaseModels;

namespace Corvus.Nest.Backend.Interfaces.IHelpers;

public interface IHttpClientHelper
{
    public HttpContentType ContentType { get; set; }

    /// <summary>
    /// Http Get 非同步
    /// </summary>
    /// <param name="uri"> 網址(含傳遞參數) </param>
    /// <returns> Reponse字串 </returns>
    public Task<string> HttpGetAsync(string uri);

    /// <summary>
    /// Http Get 非同步
    /// </summary>
    /// <param name="uri"> 網址(含傳遞參數) </param>
    /// <returns> 回傳 T 物件 </returns>
    public Task<T?> HttpGetAsync<T>(string uri) where T : new();

    /// <summary>
    /// Http Post 非同步
    /// </summary>
    /// <param name="req">
    /// Uri => Api 位置
    /// Authorization => Header-Authorization 驗證方法，若無可給null
    /// ContentType  => Header-Content-Type 
    /// Msg => Body-訊息內容
    /// </param>
    /// <returns> Reponse字串 </returns>
    public Task<string> HttpPostAsync(HttpRequestModel req);

    /// <summary>
    /// Http Post 非同步
    /// </summary>
    /// <param name="req">
    /// Uri => Api 位置
    /// Authorization => Header-Authorization 驗證方法，若無可給null
    /// ContentType  => Header-Content-Type 
    /// Msg => Body-訊息內容
    /// </param>
    /// <returns> 回傳 T 物件 </returns>
    public Task<T?> HttpPostAsync<T>(HttpRequestModel req) where T : new();

    public Task<HttpResponseModel> HttpPostNoJudgeAsync(HttpRequestModel req);

    public Task<string> HttpPatchAsync(HttpRequestModel req);

    public Task<T?> HttpPatchAsync<T>(HttpRequestModel req) where T : new();

    public Task<string> HttpPutAsync(HttpRequestModel req);

    public Task<T?> HttpPutAsync<T>(HttpRequestModel req) where T : new();
}
