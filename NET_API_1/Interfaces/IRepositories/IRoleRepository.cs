using NET_API_1.Models.Entity;

namespace NET_API_1.Interfaces.IRepositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetRoleById(int RoleId);
    }
}
