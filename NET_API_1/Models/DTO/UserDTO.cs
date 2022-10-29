using NET_API_1.Models.Entity;

namespace NET_API_1.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        public UserDTO() { }
        public UserDTO(User user)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            Email = user.Email;
            Avatar = user.Avatar;
        }
    }
}
