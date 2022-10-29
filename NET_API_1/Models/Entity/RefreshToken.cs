namespace NET_API_1.Models.Entity
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? TokenHash { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual User? User { get; set; }
    }
}
