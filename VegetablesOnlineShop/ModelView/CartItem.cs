using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.ModelView
{
    public class CartItem
    {
        public Product product { get; set; }
        public int amount { get; set; } = 1;
        public double totalMoney => amount * product.Price;
    }
}
