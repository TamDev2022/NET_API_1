using NET_API_1.Models.Entity;

namespace NET_API_1.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserAsync(string str);
    }
}
