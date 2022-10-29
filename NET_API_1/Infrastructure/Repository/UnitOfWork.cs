using Microsoft.EntityFrameworkCore;
using NET_API_1.Infrastructure.Data;
using NET_API_1.Interfaces.IRepositories;

namespace NET_API_1.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _userRepository = new UserRepository(dbContext);
            _roleRepository = new RoleRepository(dbContext);
            _refreshTokenRepository = new RefreshTokenRepository(dbContext);
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ?? new UserRepository(_dbContext);
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ?? new RoleRepository(_dbContext);
            }
        }

        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                return _refreshTokenRepository ?? new RefreshTokenRepository(_dbContext);
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
