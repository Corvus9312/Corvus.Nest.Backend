namespace Corvus.Nest.Backend.Models.BaseModels;

public class HttpRequestModel
{
    public string Url { get; set; } = null!;

    public string? Authorization { get; set; }

    public string ContentType { get; set; } = null!;

    public string Msg { get; set; } = null!;
}