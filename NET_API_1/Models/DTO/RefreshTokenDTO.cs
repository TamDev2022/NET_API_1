using NET_API_1.Models.Entity;

namespace NET_API_1.Models.DTO
{
    public class RefreshTokenDTO
    {
        public int UserId { get; set; }
        public string TokenHash { get; set; } = "defautrefreshtoken";
        public DateTime ExpiryDate { get; set; }

        public RefreshTokenDTO()
        {
        }
        public RefreshTokenDTO(RefreshToken refreshToken)
        {
            UserId = refreshToken.UserId;
            TokenHash = refreshToken.TokenHash;
            ExpiryDate = refreshToken.ExpiryDate;
        }
    }
}
