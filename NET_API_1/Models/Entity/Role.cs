namespace NET_API_1.Models.Entity
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = "User";
        public virtual User? User { get; set; }
    }
}
