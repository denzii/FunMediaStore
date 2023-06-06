using FunMediaStore.Model;
using FunMediaStore.Model.Transfer;

namespace FunMediaStore.Interface
{
    public interface IPointOfSale
    {
        public Task<SaleResponse> ProcessAsync(IEnumerable<string> requestedProducts, User user);
    }
}
