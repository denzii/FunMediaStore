using FunMediaStore.Interface;
using FunMediaStore.Model;
using FunMediaStore.Model.Transfer;
using System.Text.Json;

namespace FunMediaStore.Service
{

    public class PointOfSale : IPointOfSale
    {
        private readonly IMockRepo _repo;
        public PointOfSale(IMockRepo repo)
        {
            _repo = repo;
        }

        public async Task<SaleResponse> ProcessAsync(IEnumerable<string> requestedProducts, User user)
        {
            var response = new SaleResponse();

            var stockInfo = await _repo.ProductsExistsAsync(requestedProducts);
            if(stockInfo.Any(p => !p.Availability))
            {
                response.Errors.Add("Some of the requested products were not available in the stock and were ignored in the order");

            }
            var existingProducts = await _repo.GetProductsAsync(stockInfo.Where(p => p.Availability).Select(p => p.InventoryName)!);
            var categories = SortInventoryByType(existingProducts);

            var updatedUser = await ActivateMembershipsAsync(user, categories.Subscriptions.ToList());
            var order = new PurchaseOrder()
            {
                CartTotal = categories.Subscriptions.Sum(item => item.SalePrice),
                Items = categories.Subscriptions,
                DateTime = DateTime.Now,
                Customer = user,
            };

            updatedUser.OrderHistory = new List<PurchaseOrder>()
            {
               order
            };


            if (!await _repo.UpdateUserAsync(updatedUser))
            {
                response.Errors.Add("Failed to update user account with new order/membership due to an internal error. Please try again or contact IT.");
                return response;
            }

            response.TotalPrice = existingProducts.Sum(item => item.SalePrice);
            response.UserEmail = user.Email;
            response.Order = order;

            response.ShippingSlip = categories.PhysicalInventory != null && categories.PhysicalInventory.Count() > 0
                ? GenerateShippingSlip(categories.PhysicalInventory.ToList())
                : null;
            
            return response;
        }

        private CategoricInventories SortInventoryByType(IEnumerable<Inventory> items)
        {
            var physicalInventory = new List<Inventory>();
            var subscriptions = new List<Inventory>();
            foreach (var item in items)
            {
                if (item.Type == InventoryType.Book || item.Type == InventoryType.Video)
                {
                    physicalInventory.Add(item);
                    continue;
                }

                subscriptions.Add(item);  
            }

            return new CategoricInventories
            {
                PhysicalInventory = physicalInventory,
                Subscriptions = subscriptions
            };
        }

        private Task<User> ActivateMembershipsAsync(User user, List<Inventory> subscriptions)
        {
            if (subscriptions.Count > 0)
            {
                user.Subscriptions = new List<string>();
                
                if (subscriptions.Any(s => s.Name == "Book Club Membership") && subscriptions.Any(s => s.Name == "Video Club Membership"))
                {
                    user.Subscriptions.Add("Premium Membership");
                }
                else
                {
                    foreach (var sub in subscriptions.Select(s => s.Name)) user.Subscriptions.Add(sub!);
                }
            }

            return Task.FromResult(user);
        }

        private static ShippingSlip? GenerateShippingSlip(List<Inventory> physicalInventory)
        {
            if (physicalInventory.Count > 0)
            {
                var shippingSlip = new ShippingSlip();

                foreach (var productItem in physicalInventory)
                {
                    shippingSlip.Lines.Add($"{productItem.SKU} - {productItem.Name} - {productItem.SalePrice}");
                }

                return shippingSlip;
            }
            return null;
        }
    }
}
