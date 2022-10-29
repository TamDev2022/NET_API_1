using Microsoft.CodeAnalysis.Scripting;

namespace NET_API_1.Interfaces.IServices
{
    public interface IPasswordEncoderService
    {
        public string Decode(string value);
        public string Encode(string value);
        public bool Verify(string plaintext, string encodedPassword);
    }
}
