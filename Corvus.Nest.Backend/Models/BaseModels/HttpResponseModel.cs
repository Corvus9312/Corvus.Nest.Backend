using System.Net;

namespace Corvus.Nest.Backend.Models.BaseModels;

public class HttpResponseModel
{
    public HttpStatusCode Status { get; set; }

    public object? Message { get; set; }
}