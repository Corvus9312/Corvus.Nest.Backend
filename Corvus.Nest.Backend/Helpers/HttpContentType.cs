namespace Corvus.Nest.Backend.Helpers;

public class HttpContentType
{
    public string x_www_form_urlencoded { get { return "application/x-www-form-urlencoded"; } }

    public string Json { get { return "application/json"; } }

    public string FormData { get { return "multipart/form-data"; } }
}
