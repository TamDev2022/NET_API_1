using NET_API_1.Models.DTO;
using NET_API_1.Models.Entity;
using System.Security.Claims;

namespace NET_API_1.Interfaces.IServices
{
    public interface ITokenService
    {
        public string GenerateAccessToken(User user);
        public string GenerateRefreshToken();
        public RefreshTokenDTO GenerateRefreshToken(int UserId);
        public Task<RefreshTokenDTO> GetRefreshByIdAsync(int Id);
        public Task<RefreshTokenDTO> GetRefreshByUserIdAsync(int UserId);
        public Task<RefreshTokenDTO> RefreshToken(string RefreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public bool ValidateToken(string token);
    }
}
