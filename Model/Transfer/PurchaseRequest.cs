using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FunMediaStore.Model.Transfer
{
    public class PurchaseRequest
    {
        [Required(ErrorMessage = "Items must not be empty.")]

        public IEnumerable<string> Products { get; set; } = new List<string>();
    }
}