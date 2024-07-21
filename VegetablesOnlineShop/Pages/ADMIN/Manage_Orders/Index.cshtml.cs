using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Orders
{
    public class IndexModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;

        public IndexModel(PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
            
        }

        public IList<Order> Orders { get; set; }
        public async Task OnGetAsync()
        {

            Orders = await _context.Orders
               .Include(o => o.TransactStatus)
               .Include(o => o.Customer) 
               .Include(o => o.Shipper) 
               .Include(o => o.OrderDetails) 
               .ToListAsync();

        }
    }
}
