namespace FunMediaStore.Model
{
    public class CategoricInventories
    {
        public ICollection<Inventory> PhysicalInventory { get; set; } = new List<Inventory>();
        public ICollection<Inventory> Subscriptions { get; set;  } = new List<Inventory>();
    }
}
