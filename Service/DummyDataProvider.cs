using FunMediaStore.Model;

namespace FunMediaStore.Service
{
    public static class DummyDataProvider
    {
        public static List<User> Users ((byte[], byte[]) user1Creds, (byte[], byte[]) user2Creds) => new()
            {
                new()
                {
                    Id = 1,
                    Username = "johnDoe",
                    Password = "password123",
                    Email = "user@example.com",
                    Surname = "Doe",
                    Name = "John",
                    Role = "User",
                    PasswordSalt = user1Creds.Item1,
                    PasswordHash = user1Creds.Item2,
                },
                new User
                {
                    Id = 2,
                    Username = "janeSmith",
                    Password = "qwerty123",
                    Email = "janesmith@example.com",
                    Surname = "Smith",
                    Name = "Jane",
                    Role = "User",
                    PasswordSalt = user2Creds.Item1,
                    PasswordHash = user2Creds.Item2,
                },
            };
        public static List<Inventory> Products () => new()
        {
                new()
                {
                    SKU = "MEM001",
                    Type = InventoryType.Subscription,
                    Name = "Book Club Membership",
                    SalePrice = 10.00m
                },
                new()
                {
                    SKU = "MEM002",
                    Type = InventoryType.Subscription,
                    Name = "Video Club Membership",
                    SalePrice = 10.00m
                },
                new()
                {
                    SKU = "MEM003",
                    Type = InventoryType.Subscription,
                    Name = "Premium Membership",
                    SalePrice = 20.00m
                },
                new()
                {
                    SKU = "BOOK001",
                    Type = InventoryType.Book,
                    Name = "Book Title",
                    SalePrice = 15.00m
                },
                new()
                {
                    SKU = "VIDEO001",
                    Type = InventoryType.Video,
                    Name = "Video Title",
                    SalePrice = 12.50m
                }
            };
    }
}
