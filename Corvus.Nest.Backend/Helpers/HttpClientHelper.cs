using Corvus.Nest.Backend.Extensions;
using Corvus.Nest.Backend.Interfaces.IHelpers;
using Corvus.Nest.Backend.Models.BaseModels;
using System.Text;

namespace Corvus.Nest.Backend.Helpers;

public class HttpClientHelper : IHttpClientHelper
{
    public HttpContentType ContentType { get; set; }

    public HttpClientHelper()
    {
        ContentType = new HttpContentType();
    }

    public virtual async Task<string> HttpGetAsync(string uri)
    {
        var response = await HttpGet(uri);

        return $"{response.Message}";
    }

    public virtual async Task<T?> HttpGetAsync<T>(string uri) where T : new()
    {
        var response = await HttpGet(uri);

        return JsonConvert.Deserialize<T>($"{response.Message}");
    }

    private async Task<HttpResponseModel> HttpGet(string uri)
    {
        // AD驗證
        // HttpClientHandler handler = new() { UseDefaultCredentials = true };

        using HttpClient client = new();
        HttpRequestMessage httpRequest = new(HttpMethod.Get, uri);

        using var reaponse = await client.SendAsync(httpRequest);

        HttpResponseModel result = new()
        {
            Status = reaponse.StatusCode,
            Message = await reaponse.Content.ReadAsStringAsync()
        };

        if (!reaponse.IsSuccessStatusCode)
            throw new Exception(JsonConvert.Serialize(result, true));

        return result;
    }

    public virtual async Task<string> HttpPostAsync(HttpRequestModel req)
    {
        return await HttpFunc((x, IsSuccessStatusCode) =>
            {
                if (!IsSuccessStatusCode)
                    throw new Exception(JsonConvert.Serialize(x, true));

                return $"{x.Message}";
            }, req, HttpMethod.Post);
    }

    public virtual async Task<T?> HttpPostAsync<T>(HttpRequestModel req) where T : new()
    {
        return await HttpFunc((x, IsSuccessStatusCode) =>
            {
                if (!IsSuccessStatusCode)
                    throw new Exception(JsonConvert.Serialize(x, true));

                return JsonConvert.Deserialize<T>($"{x.Message}");
            }, req, HttpMethod.Post);
    }

    public virtual async Task<HttpResponseModel> HttpPostNoJudgeAsync(HttpRequestModel req)
    {
        return await HttpFunc((x, IsSuccessStatusCode) => x, req, HttpMethod.Post);
    }

    private async Task<T> HttpFunc<T>(Func<HttpResponseModel, bool, T> func, HttpRequestModel req, HttpMethod method)
    {
        // 若有AD驗證要加
        // HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };

        // SSL驗證失敗時要加
        var handler = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (sender, cert, cetChain, policyErrors) => true
        };

        using HttpClient client = new(handler);

        //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

        HttpRequestMessage httpRequest = new(method, req.Url);

        if (!string.IsNullOrWhiteSpace(req.Authorization))
            client.DefaultRequestHeaders.Add("Authorization", req.Authorization);

        httpRequest.Content = new StringContent($"{req.Msg}", Encoding.UTF8, req.ContentType);

        var responseTask = await client.SendAsync(httpRequest);

        HttpResponseModel response = new()
        {
            Status = responseTask.StatusCode,
            Message = await responseTask.Content.ReadAsStringAsync()
        };

        return func(response, responseTask.IsSuccessStatusCode);
    }

    public virtual async Task<string> HttpPatchAsync(HttpRequestModel req)
    {
        return await HttpFunc((x, IsSuccessStatusCode) =>
        {
            if (!IsSuccessStatusCode)
                throw new Exception(JsonConvert.Serialize(x, true));

            return $"{x.Message}";
        }, req, HttpMethod.Patch);
    }

    public virtual async Task<T?> HttpPatchAsync<T>(HttpRequestModel req) where T : new()
    {
        return await HttpFunc((x, IsSuccessStatusCode) =>
        {
            if (!IsSuccessStatusCode)
                throw new Exception(JsonConvert.Serialize(x, true));

            return JsonConvert.Deserialize<T>($"{x.Message}");
        }, req, HttpMethod.Patch);
    }

    public virtual async Task<string> HttpPutAsync(HttpRequestModel req)
    {
        return await HttpFunc((x, IsSuccessStatusCode) =>
        {
            if (!IsSuccessStatusCode)
                throw new Exception(JsonConvert.Serialize(x, true));

            return $"{x.Message}";
        }, req, HttpMethod.Put);
    }

    public virtual async Task<T?> HttpPutAsync<T>(HttpRequestModel req) where T : new()
    {
        return await HttpFunc((x, IsSuccessStatusCode) =>
        {
            if (!IsSuccessStatusCode)
                throw new Exception(JsonConvert.Serialize(x, true));

            return JsonConvert.Deserialize<T>($"{x.Message}");
        }, req, HttpMethod.Put);
    }
}
