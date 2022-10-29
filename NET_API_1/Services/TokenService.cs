using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NET_API_1.Configurations;
using NET_API_1.Interfaces.IRepositories;
using NET_API_1.Interfaces.IServices;
using NET_API_1.Models.DTO;
using NET_API_1.Models.Entity;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static NET_API_1.Configurations.AppSettings;
using static System.Net.WebRequestMethods;

namespace NET_API_1.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TokenService(IOptions<JWTSettings> jwtSettings, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _jwtSettings = jwtSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.Expires),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public RefreshTokenDTO GenerateRefreshToken(int userId)
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var TokenHash = Convert.ToBase64String(randomNumber);

            RefreshTokenDTO reftoken = new RefreshTokenDTO()
            {
                UserId = userId,
                TokenHash = TokenHash,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            return reftoken;
        }

        public async Task<RefreshTokenDTO> GetRefreshByIdAsync(int Id)
        {
            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetRefreshByIdAsync(Id).ConfigureAwait(false);
            if (refreshToken == null) throw new ArgumentException($"refreshToken id {Id} không tồn tại");
            return _mapper.Map<RefreshTokenDTO>(refreshToken);
        }

        public async Task<RefreshTokenDTO> GetRefreshByUserIdAsync(int UserId)
        {
            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetRefreshByUserIdAsync(UserId).ConfigureAwait(false);
            if (refreshToken == null) throw new ArgumentException($"refreshToken id {UserId} không tồn tại");
            return _mapper.Map<RefreshTokenDTO>(refreshToken);
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task<RefreshTokenDTO> RefreshToken(string RefreshToken)
        {
            await _unitOfWork.UserRepository.GetUserAsync("");
            return _mapper.Map<RefreshTokenDTO>(RefreshToken);
        }
        public bool ValidateToken(string token)
        {
            throw new NotImplementedException();
        }

    }
}
