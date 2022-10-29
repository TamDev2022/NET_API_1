using NET_API_1.Interfaces.IRepositories;
using NET_API_1.Interfaces.IServices;
using NET_API_1.Models.Entity;

namespace NET_API_1.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Role> GetRoleById(int RoleId)
        {
            return _unitOfWork.RoleRepository.GetRoleById(RoleId);
        }

    }
}
