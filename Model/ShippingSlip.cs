namespace FunMediaStore.Model
{
    public class ShippingSlip
    {
        public ICollection<string> Lines { get; set; } = new List<string>();
    }
}
