using System.Text;

namespace Corvus.Nest.Backend.Interfaces.IHelpers;

public interface IEncryptionHelper
{
    public Task<string> HMACSHA256(string text, Encoding? encoding = null, string? key = null);

    public string Base64ToString(byte[] bytes);

    public byte[] StringToBase64(string str);

    public Task<string> EncryptDES(string text, string? key = null);

    public Task<string> DecryptDES(string text, string? key = null);
}
