using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FunMediaStore.Model
{
    public class Inventory
    {
        [JsonIgnore]
        public string? SKU { get; set; }

        [JsonIgnore]
        public InventoryType Type { get; set; }

        [Required(ErrorMessage = "Which product is being bought? Inventory name is required.")]
        public string? Name { get; set; }

        [JsonIgnore]
        public decimal SalePrice { get; set; }
    }
}
