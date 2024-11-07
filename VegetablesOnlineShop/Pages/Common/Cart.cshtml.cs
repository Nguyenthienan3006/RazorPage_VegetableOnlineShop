using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VegetablesOnlineShop.Extension;
using VegetablesOnlineShop.Models;
using VegetablesOnlineShop.ModelView;

namespace VegetablesOnlineShop.Pages.Common
{
    public class CartModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyfService;

        public CartModel(INotyfService notyfService, PRN221_OnlineShopDBContext context)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public List<CartItem> CartItems 
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("cart");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        public async Task<IActionResult> OnGet(int? productId, int? amount , int? productIdRemove)
        {
            List<CartItem> cart = CartItems;
            if(productId != null)
            {

                var productCheck = _context.Products.Where(p => p.ProductId == productId).FirstOrDefault();

                //kiểm tra xem đã sold out chưa
                if (productCheck.UnitslnStock <= 0)
                {
                    _notyfService.Warning("Product sold out!");
                    return RedirectToPage("Shop");
                }

                //kiểm tra xem đã hết hạn sản phẩm chưa
                if(productCheck.ExpirationDate < DateTime.Now)
                {
                    _notyfService.Warning("The product has expired and awaiting restock!");
                    return RedirectToPage("Shop");
                }



                var item = cart.FirstOrDefault(c => c.product.ProductId == productId);
                if(item != null)
                {
                    if (amount.HasValue)
                    {
                        item.amount += amount.Value;
                    }
                }
                else
                {
                    Product newProduct = _context.Products.FirstOrDefault(p => p.ProductId == productId);
                    item = new CartItem
                    {
                        product = newProduct,
                        amount = amount.HasValue ? 1 : 1
                    };
                    cart.Add(item);
                }
            }

            if(productIdRemove != null)
            {
                CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productIdRemove);
                if (item != null)
                {
                    cart.Remove(item);
                }
            }

            HttpContext.Session.SetInt32("count", cart.Count);
            HttpContext.Session.Set<List<CartItem>>("cart", cart);
            return Page();
        }
    }
}
