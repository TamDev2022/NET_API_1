using NET_API_1.Models.Entity;

namespace NET_API_1.Interfaces.IRepositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        public Task<RefreshToken?> GetRefreshByIdAsync(int Id);
        public Task<RefreshToken?> GetRefreshByUserIdAsync(int UserId);
    }
}
