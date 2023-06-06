using FunMediaStore.Model;

namespace FunMediaStore.Interface
{
    public interface IMockRepo
    {
        // Users
        Task<bool> UserExistsAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserAsync(int id);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);

        // Products
        public Task<IEnumerable<StockInfo>> ProductsExistsAsync(IEnumerable<string> products);
        Task<IEnumerable<Inventory>> GetProductsAsync(IEnumerable<string> products);
    }
}
