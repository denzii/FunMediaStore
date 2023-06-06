using FunMediaStore.Interface;
using FunMediaStore.Model;
using System.Collections;
using System.Collections.Generic;

namespace FunMediaStore.Service
{
    public class MockRepository : IMockRepo
    {
        // Properties for in-memory dummy DB data
        private readonly List<User> _users;
        private readonly List<Inventory> _products;

        public MockRepository(IAuth auth)
        {
            var user1Creds = auth.CreatePasswordHash("password123", null);
            var user2Creds = auth.CreatePasswordHash("qwerty123", null);

            _users = DummyDataProvider.Users(user1Creds, user2Creds);
            _products = DummyDataProvider.Products();
        }

        public Task<bool> AddUserAsync(User user)
        {
            _users.Add(user);
            return Task.FromResult(true); 
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            User? user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user); 
                return Task.FromResult(true);
            }
            return Task.FromResult(false); 
        }

        public Task<User?> GetUserAsync(int id) => Task.FromResult(_users.FirstOrDefault(u => u.Id == id)); 
        public Task<User?> GetUserByEmailAsync(string email) => Task.FromResult(_users.FirstOrDefault(u => u.Email == email));

        public Task<bool> UpdateUserAsync(User user)
        {
            User? existingUser = _users.FirstOrDefault(u => u.Email == user.Email); 
            try
            {
                var index = _users.FindIndex(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    if (user.Subscriptions.Any())
                    {
                        foreach (var sub in user.Subscriptions)
                        {
                            var existingSub = existingUser.Subscriptions.FirstOrDefault(s => s == sub);
                            if (existingSub != null)
                            {
                                // no need to update a subscription object for now, as subscription is just a string for our purposes at the moment
                                continue;
                            }
                            
                            existingUser.Subscriptions.Add(sub);
                        }
                    }
                    if (user.OrderHistory.Any())
                    {
                        foreach (var order in user.OrderHistory) existingUser.OrderHistory.Add(order!);
                    }

                    if (index > -1)
                    {
                        _users[index] = existingUser;
                    }

                    return Task.FromResult(true);
                }

            } catch (Exception)
            {
                return Task.FromResult(false);

            }

            return Task.FromResult(false);
        }

        public Task<bool> UserExistsAsync(string email)
        {
            bool userExists = _users.Any(u => u.Email == email);
            return Task.FromResult(userExists); 
        }

        public Task<IEnumerable<StockInfo>> ProductsExistsAsync(IEnumerable<string> products)
        {
            var existingProducts = products.Where(p => _products.Any(i => p == i.Name));
            var nonExistingProducts = products.Where(p => !_products.Any(i => p == i.Name));

            // concatenate them into a final results object to be processed by the caller
            var result = Task.FromResult(
                existingProducts.Select(x => new StockInfo { InventoryName = x, Availability = true })
                .Concat(nonExistingProducts.Select(x => new StockInfo { InventoryName = x, Availability = false })));

            return result;
        }

        public Task<IEnumerable<Inventory>> GetProductsAsync(IEnumerable<string> products)
        {
            // fetch all products that match the names in the list
            var result = Task.FromResult(_products.Where(i => products.Any(p => i.Name == p)));
            return result;
        }
    }
}
