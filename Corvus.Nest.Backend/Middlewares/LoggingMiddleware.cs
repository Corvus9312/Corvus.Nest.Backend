using Corvus.Nest.Backend.Extensions;
using Corvus.Nest.Backend.Interfaces.IHelpers;
using Corvus.Nest.Backend.Middlewares.Tools;
using Corvus.Nest.Backend.Models.BaseModels;
using Microsoft.IO;

namespace Corvus.Nest.Backend.Middlewares
{
    public class LoggingMiddleware(RequestDelegate next)
    {
        private IFileHelper _fileHelper = null!;
        private readonly RecyclableMemoryStreamManager _recycleStream = new();
        private IWebHostEnvironment _environment = null!;
        private ILogger _logger = null!;

        public async Task Invoke(
            HttpContext context,
            ILogIDs logID,
            IFileHelper fileHelper,
            IWebHostEnvironment environment,
            ILogger<LoggingMiddleware> log)
        {
            _fileHelper = fileHelper;
            _environment = environment;
            _logger = log;

            var sDate = DateTime.UtcNow.AddHours(8);
            context.Request.EnableBuffering();
            var request = context.Request;
            var path = request.Path.ToString();
            var reqHeaders = request.Headers;
            var reqBody = request.Body;
            var reqBodyStr = new StreamReader(reqBody).ReadToEndAsync().Result;
            var scheme = request.Scheme;
            var method = request.Method;
            var queryString = request.QueryString.ToString();
            var ip = $"{context.Connection.RemoteIpAddress}";

            _logger.LogTrace($"Path:{path}");

            // 紀錄Request Body
            await using (var requestStream = _recycleStream.GetStream())
            {
                await reqBody.CopyToAsync(requestStream);
            }
            reqBody.Position = 0;

            var resBody = context.Response.Body;
            await using var responseBody = _recycleStream.GetStream();
            context.Response.Body = responseBody;
            try
            {
                await next(context);

                // CORS 跨網域存取設定
                if (!context.Response.Headers.Where(x => x.Key.Equals("Access-Control-Allow-Origin")).Any())
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");

                // 讀取Stream後需還原Stream Position
                context.Response.Body.Position = 0;
                var response = context.Response;
                var respHeaders = response.Headers;
                var statusCode = response.StatusCode;
                var respBody = response.Body;
                var respBodyStr = new StreamReader(respBody).ReadToEndAsync().Result;

                WriteLog(logID.ID, sDate, ip, statusCode, reqBodyStr, reqHeaders, respHeaders, respBodyStr, scheme, method, path, "");

                context.Response.Body.Position = 0;
                await responseBody.CopyToAsync(resBody);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                _logger.LogError($"Message:{ex}");

                if (string.IsNullOrWhiteSpace($"{ex.InnerException}"))
                    _logger.LogError($"InnerMessage:{ex.InnerException}");

                context.Response.Body.Position = 0;
                var response = context.Response;
                var respHeaders = response.Headers;
                var statusCode = response.StatusCode;
                var respBody = response.Body;
                var respBodyStr = new StreamReader(respBody).ReadToEndAsync().Result;

                var exMsg = string.IsNullOrWhiteSpace(ex.InnerException?.Message) ? $"{ex.Message}" : $"{ex.Message}, innerException:{ex.InnerException}";

                WriteLog(logID.ID, sDate, ip, statusCode, reqBodyStr, reqHeaders, respHeaders, respBodyStr, scheme, method, path, exMsg);

                context.Response.Body.Position = 0;
                await responseBody.CopyToAsync(resBody);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
            }
        }

        private void WriteLog(Guid logID, DateTime sDate, string ip, int statusCode, string requestBody,
            IHeaderDictionary reqHeaders, IHeaderDictionary respHeaders, string responseBody, string scheme,
            string method, string path, string exMsg)
        {
            var reqHeadersString = string.Empty;
            foreach (var item in reqHeaders)
            {
                reqHeadersString += item.Key + "=" + item.Value + ";";
                ip = item.Key.Equals("X-Real-IP") ? item.Value.ToString() : ip;
            }

            var resHeadersString = string.Empty;
            foreach (var item in respHeaders)
            {
                resHeadersString += item.Key + "=" + item.Value + ";";
            }

            var log = new MiddleLogModel
            {
                LogID = logID,
                SDate = sDate,
                EDate = DateTime.UtcNow.AddHours(8),
                IP = ip,
                StatusCode = statusCode,
                ReqHeaders = reqHeadersString,
                ReqBody = requestBody,
                ResHeaders = resHeadersString,
                ResBody = responseBody,
                Scheme = scheme,
                Method = method,
                Path = path,
                ExMsg = exMsg
            };

            WriteLotToTxt(log);
        }

        private void WriteLotToTxt(MiddleLogModel log)
        {
            var path = Path.Combine(_environment.ContentRootPath, "Log");

            _fileHelper.WriteLog(JsonConvert.Serialize(log), path, $"{DateTime.Now:yyyy-MM-dd}.log");
        }
    }
}