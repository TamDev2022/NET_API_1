using NET_API_1.Models.DTO;
using NET_API_1.Models.Entity;
using NET_API_1.Models.Request;
using NET_API_1.Utils;

namespace NET_API_1.Interfaces.IServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get list user
        /// </summary>
        /// <returns>List UserDTO</returns>
        public Task<PaginatedList<UserDTO>> GetListAsync(int PageNumber, int PageSize);
        public Task<User?> GetUserByIdAsync(int UserId);
        public Task<User?> GetUserAsync(string Email);
        public Task<UserDTO?> GetUserDTOByIdAsync(int UserId);
        public Task<UserDTO?> GetUserDTOAsync(string Email);
        public Task<object> SignInAsync(UserSignInModel model);
        public Task<UserDTO> SignUpAsync(UserSignUpModel model);
        public Task<string> ConfirmAccountAsync(string code, UserDTO userDTO);

    }
}
