using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Orders
{
    public class OrderDetailModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;


        public OrderDetailModel(PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }    
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
            .Include(o => o.TransactStatus)
            .Include(o => o.Customer)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product) 
            .FirstOrDefaultAsync(o => o.OrderId == id);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var orderToUpdate = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == Order.OrderId);

            if (orderToUpdate != null)
            {
                orderToUpdate.TransactStatusId = 3;

                _context.Orders.Update(orderToUpdate);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}
