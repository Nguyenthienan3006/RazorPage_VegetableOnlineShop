using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;
using VegetablesOnlineShop.ModelView;
using static NuGet.Packaging.PackagingConstants;

namespace VegetablesOnlineShop.Pages.ADMIN
{
    public class HomePageModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;

        public HomePageModel(PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public int? totalMoney { get; set; }
        public int totalOrder { get; set; }
        public int pendingOrder { get; set; }
        public int totalCustomer { get; set; }


        public IList<AdminReportVM> adminReportVMs { get; set; }
        public IList<Order> orders { get; set; }


        public void OnGet()
        {
            LoadReport();

        }



        public async Task<IActionResult> OnPost(DateTime startDate, DateTime endDate)
        {
            DateTime sqlMinDate = new DateTime(1753, 1, 1);
            DateTime sqlMaxDate = new DateTime(9999, 12, 31);

            if (startDate < sqlMinDate || endDate < sqlMinDate || endDate > sqlMaxDate || startDate > sqlMaxDate || startDate >= endDate)
            {
                _notyf.Warning("Please enter correct date to get report!");
                return RedirectToPage("HomePage");
            }
            GetRevenueAndProfit(startDate, endDate);


            return Page();
        }

        private void LoadReport()
        {
            totalMoney = _context.Orders.Sum(o => o.Total);

            totalOrder = _context.Orders.Select(o => o.OrderId).Count();

            pendingOrder = _context.Orders.Where(o => o.TransactStatusId == 1).Count();

            totalCustomer = _context.Customers.Distinct().Count();
        }


        private async Task<IActionResult> GetRevenueAndProfit(DateTime startDate, DateTime endDate)
        {
            int? totalCost = 0;

            var filteredOrders = _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToList();

            //Tính tổng doanh thu
            var totalEarning = filteredOrders.Sum(od => od.Total);

            if (totalEarning == 0)
            {
                _notyf.Warning("There is no record");
                return Page();
            }

            //Tính tổng giá nhập
            foreach (var item in filteredOrders)
            {
                var orderDatailList = _context.OrderDetails.Where(od => od.OrderId == item.OrderId).ToList();
                totalCost += orderDatailList.Sum(o => o.Product.ImportPrice * o.Quantity);
            }


            //Tính tổng lợi nhuận
            var profit = totalEarning - totalCost;

            double percentage;

            if (profit >= 0)
            {
                // Tính phần trăm lãi
                percentage = ((double)profit / (double)totalEarning) * 100;
                ViewData["ProfitPercentage"] = percentage.ToString("0.##") + "% Profit";
            }
            else
            {
                // Tính phần trăm lỗ
                var loss = -profit;
                percentage = ((double)loss / (double)totalEarning) * 100;
                ViewData["ProfitPercentage"] = percentage.ToString("0.##") + "% Loss";
            }


            ViewData["Earning"] = totalEarning + "$";
            ViewData["Profit"] = profit + "$";
            ViewData["Cost"] = totalCost + "$";
            ViewData["StartDate"] = startDate.ToString("dd MMM yyyy");
            ViewData["EndDate"] = endDate.ToString("dd MMM yyyy");
            LoadReport();
            return Page();
        }



    }
}
