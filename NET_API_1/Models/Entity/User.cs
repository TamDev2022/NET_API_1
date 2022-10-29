namespace NET_API_1.Models.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public int RoleId { get; set; }
        public int Status { get; set; }
        public string Code { get; set; } = "";
        public DateTime ExpiryCode { get; set; }
        public string? Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual RefreshToken? RefreshToken { get; set; }
        public virtual Role? Role { get; set; }

    }
}
