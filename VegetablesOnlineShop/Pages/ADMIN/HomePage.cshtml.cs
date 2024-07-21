using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN
{
    public class HomePageModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;

        public HomePageModel(PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        public int? totalMoney { get; set; }
        public int totalOrder { get; set; }
        public int pendingOrder { get; set; }
        public int totalCustomer { get; set; }

        public void OnGet()
        {

            totalMoney = _context.Orders.Sum(o => o.Total);

            totalOrder = _context.Orders.Select(o => o.OrderId).Count();

            pendingOrder = _context.Orders.Where(o => o.TransactStatusId == 1).Count();

            totalCustomer = _context.Accounts.Distinct().Count();
        }
    }
}
