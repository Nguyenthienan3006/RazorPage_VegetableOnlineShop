using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages
{
    public class My_OrderModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;

        public List<Order> OrderList { get; set; }
        public List<OrderDetail> OrderDetailList { get; set; }
        public My_OrderModel(PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public async Task<IActionResult> OnGetAsync(bool? detail, int? orderId, bool? remove, bool? completeOrder)
        {
            OrderDetailList = new List<OrderDetail>();

            if(completeOrder == true)
            {
                var completedOrder = _context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
                completedOrder.PaymentDate = DateTime.Now;
                completedOrder.TransactStatusId = 3;


                //Cập nhật số lượng sau khi khách hàng mua hàng thành công
                //lấy ra order detail productId và quantity
                var orderDetail = _context.OrderDetails.Where(o => o.OrderId == orderId).ToList();

                //giảm số lượng trong bảng product
                foreach(var item in  orderDetail)
                {
                    var product = _context.Products.Where(p => p.ProductId == item.ProductId).FirstOrDefault();
                    product.UnitslnStock -= item.Quantity;
                }
                


                _context.Orders.Update(completedOrder);
                _context.SaveChanges();
                _notyf.Custom("Thank you for Supporting our shop", 10, "Navy");
            }

            if (remove == true)
            {
                var removeOrder = _context.Orders.SingleOrDefault(p => p.OrderId == orderId);
                var removeOrderDetail = _context.OrderDetails.Where(p => p.OrderId == orderId).ToList();
                _context.OrderDetails.RemoveRange(removeOrderDetail);
                _context.Orders.Remove(removeOrder);
                _context.SaveChanges();
            }
            var customer = _context.Customers.AsNoTracking().SingleOrDefault(p => p.Email == HttpContext.Session.GetString("CustomerEmail"));
            OrderList = _context.Orders.AsNoTracking().Include(p => p.TransactStatus).Where(p => p.CustomerId == customer.CustomerId).ToList();
            if (detail == true)
            {
                OrderDetailList = _context.OrderDetails.AsNoTracking().Include(p => p.Product).Include(p => p.Order).Where(p => p.OrderId == orderId).ToList();
            }
            return Page();
        }
    }
}
