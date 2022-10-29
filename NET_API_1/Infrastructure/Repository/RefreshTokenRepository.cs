using Microsoft.EntityFrameworkCore;
using NET_API_1.Infrastructure.Data;
using NET_API_1.Interfaces.IRepositories;
using NET_API_1.Models.Entity;
using NET_API_1.Utils;

namespace NET_API_1.Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(RefreshToken entity)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<RefreshToken>> GetListAsync(int PageNumber, int PageSize)
        {
            throw new NotImplementedException();
        }

        public void Insert(RefreshToken entity)
        {
            _dbContext.RefreshToken.Add(entity);
        }

        public void Update(RefreshToken entity)
        {
            throw new NotImplementedException();
        }
        public async Task<RefreshToken?> GetRefreshByIdAsync(int Id)
        {
            return await _dbContext.RefreshToken.FirstOrDefaultAsync(u => u.Id == Id);
        }
        public async Task<RefreshToken?> GetRefreshByUserIdAsync(int UserId)
        {
            return await _dbContext.RefreshToken.FirstOrDefaultAsync(u => u.UserId == UserId);
        }
    }
}
