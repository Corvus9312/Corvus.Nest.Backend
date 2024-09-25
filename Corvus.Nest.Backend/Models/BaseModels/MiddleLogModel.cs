namespace Corvus.Nest.Backend.Models.BaseModels;

public class MiddleLogModel
{
    public Guid LogID { get; set; }

    public DateTime SDate { get; set; }

    public DateTime EDate { get; set; }

    public string? IP { get; set; }

    public int StatusCode { get; set; }

    public string? ReqHeaders { get; set; }

    public string? ReqBody { get; set; }

    public string? ResHeaders { get; set; }

    public string? ResBody { get; set; }

    public string? Scheme { get; set; }

    public string? Method { get; set; }

    public string? Path { get; set; }

    public string? ExMsg { get; set; }
}