using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Extension;
using VegetablesOnlineShop.Models;
using VegetablesOnlineShop.ModelView;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace VegetablesOnlineShop.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;

        [BindProperty]
        public Shopping model { get; set; }
        [BindProperty]
        public int shipperId { get; set; }
        public SelectList Shippers { get; set; }


        public List<CartItem> cart
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

        public CheckoutModel(PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var shippersList = await _context.Shippers.ToListAsync();
            Shippers = new SelectList(shippersList, "ShipperId", "ShipperName");

            model = new Shopping();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerEmail")))
            {
                TempData["checkout"] = true;
                return RedirectToPage("Login");
            }
            var accEmail = HttpContext.Session.GetString("CustomerEmail");
            if (accEmail != null)
            {
                var customer = _context.Customers.AsNoTracking().SingleOrDefault(p => p.Email == accEmail);
                model.CustomerId = customer.CustomerId;
                model.FullName = customer.FullName;
                model.Email = customer.Email;
                model.Phone = customer.Phone;
                model.Address = customer.Address;
              
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (model.Address != null)
            {
                Order order = new Order
                {
                    CustomerId = model.CustomerId,
                    Address = model.Address,
                    OrderDate = DateTime.Now,
                    ShipDate = DateTime.Now.AddDays(3),
                    TransactStatusId = 1,
                    Total = (int)(cart.Sum(p => p.totalMoney)) + 10,
                    ShipperId = shipperId
                    
                };
                _context.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cart)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = item.product.ProductId,
                        Quantity = item.amount,
                        Total = (int)item.totalMoney,
                        ShipDate = DateTime.Now.AddDays(3)
                    };
                    _context.Add(orderDetail);

                }
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("cart");
                _notyf.Success("You have placed your order successfully");
                return RedirectToPage("My_Order");
            }
            return Page();
        }

    }
}
