using Microsoft.EntityFrameworkCore;
using NET_API_1.Infrastructure.Data;
using NET_API_1.Interfaces.IRepositories;
using NET_API_1.Models.Entity;
using NET_API_1.Utils;

namespace NET_API_1.Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Delete(Role entity)
        {
            throw new NotImplementedException();
        }
        public Task<PaginatedList<Role>> GetListAsync(int PageNumber, int PageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetRoleById(int RoleId)
        {
            var role = await _dbContext.Role.FirstOrDefaultAsync(r => RoleId == r.RoleId);
            if (role == null) throw new NotImplementedException();
            return role;
        }
        public void Insert(Role entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
