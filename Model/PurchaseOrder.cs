namespace FunMediaStore.Model
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public decimal CartTotal { get; set; }
        public IEnumerable<Inventory> Items { get; set; } = new List<Inventory>();
        public User? Customer { get; set; }
        public DateTime DateTime { get; set; }
    }
}