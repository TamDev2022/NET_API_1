using NET_API_1.Models.Entity;

namespace NET_API_1.Interfaces.IServices
{
    public interface IRoleService
    {
        public Task<Role> GetRoleById(int RoleId);

    }
}
