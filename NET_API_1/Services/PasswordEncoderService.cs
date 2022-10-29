using Microsoft.CodeAnalysis.Scripting;
using NET_API_1.Interfaces.IServices;

namespace NET_API_1.Services
{
    public class PasswordEncoderService : IPasswordEncoderService
    {
        public string Decode(string value)
        {
            throw new NotImplementedException();
        }

        public string Encode(string value)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(value, BCrypt.Net.HashType.SHA256);
        }

        public bool Verify(string plaintext, string encodedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(plaintext, encodedPassword, BCrypt.Net.HashType.SHA256);
        }
    }
}
