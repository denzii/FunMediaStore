namespace FunMediaStore.Model
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }

        public string Role { get; set; } = "User";

        public byte[]? PasswordHash { get; internal set; }

        public byte[]? PasswordSalt { get; internal set; }

        public ICollection<PurchaseOrder> OrderHistory { get; set; } = new List<PurchaseOrder>();
        public ICollection<string> Subscriptions { get; set; } = new List<string>();
    }
}
