namespace VegetablesOnlineShop.ModelView
{
    public class CustomerVM
    {
        public int CustomerId { get; set; }
        public string? FullName { get; set; }
        public bool Active { get; set; }

        public int TotalOrders { get; set; }
        public int? TotalSpend { get; set; }
    }
}
