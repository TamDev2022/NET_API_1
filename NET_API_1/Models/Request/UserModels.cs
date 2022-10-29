using Microsoft.Build.Framework;

namespace NET_API_1.Models.Request
{
    public class UserSignUpModel
    {
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public IFormFile? Avatar { get; set; }
    }

    public record UserSignInModel(string Email, string Password);

}
