namespace FunMediaStore.Model.Transfer
{
    public class SaleResponse
    {
        public decimal TotalPrice { get; set; }
        public string? UserEmail { get; set; }
        public ShippingSlip? ShippingSlip { get; set; }

        public PurchaseOrder? Order { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}