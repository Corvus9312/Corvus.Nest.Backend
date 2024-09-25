using System.Text.Encodings.Web;
using System.Text.Json;

namespace Corvus.Nest.Backend.Extensions;

public static class JsonConvert
{
    public static string Serialize<T>(T t, bool encode = false)
    {
        return JsonSerializer.Serialize(t, new JsonSerializerOptions { Encoder = encode ? JavaScriptEncoder.Default : JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
    }

    public static T? Deserialize<T>(string t)
    {
        return JsonSerializer.Deserialize<T>(t);
    }
}
