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

namespace VegetablesOnlineShop.Pages
{
    [BindProperties]
    public class CheckoutModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        public Shopping model { get; set; }
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
        public List<SelectListItem> Shippers { get; set; }

        public CheckoutModel(PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
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
            if (ModelState.IsValid)
            {
                Order order = new Order
                {
                    CustomerId = model.CustomerId,
                    Address = model.Address,
                    OrderDate = DateTime.Now,
                    ShipDate = DateTime.Now.AddDays(3),
                    TransactStatusId = 1,
                    Total = (int)(cart.Sum(p => p.totalMoney)) + 20000,
                };
                _context.Add(order);
                _context.SaveChanges();

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
                _context.SaveChanges();
                HttpContext.Session.Remove("cart");
                TempData["order_success"] = "You have placed your order successfully";
                return RedirectToPage("My_Order");
            }
            return Page();
        }


        //Shippers = await _context.Shippers
        //    .Select(s => new SelectListItem
        //    {
        //        Value = s.ShipperId.ToString(),
        //        Text = s.ShipperName
        //    }).ToListAsync();


        //// Kiểm tra danh sách Shippers
        //foreach (var shipper in Shippers)
        //{
        //    Console.WriteLine($"ShipperId: {shipper.Value}, ShipperName: {shipper.Text}");
        //}

    }
}
