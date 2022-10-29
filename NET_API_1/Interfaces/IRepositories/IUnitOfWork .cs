using Microsoft.EntityFrameworkCore;

namespace NET_API_1.Interfaces.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        public Task SaveAsync();


    }
}
