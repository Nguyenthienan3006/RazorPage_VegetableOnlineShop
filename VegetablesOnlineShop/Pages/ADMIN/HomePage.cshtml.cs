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

        public HomePageModel(PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        public int? totalMoney { get; set; }
        public int totalOrder { get; set; }
        public int pendingOrder { get; set; }
        public int totalCustomer { get; set; }

        public IList<AdminReportVM> adminReportVMs { get; set; }
        public IList<Order> orders { get; set; }


        public void OnGet()
        {

            totalMoney = _context.Orders.Sum(o => o.Total);

            totalOrder = _context.Orders.Select(o => o.OrderId).Count();

            pendingOrder = _context.Orders.Where(o => o.TransactStatusId == 1).Count();

            totalCustomer = _context.Customers.Distinct().Count();
        }

        public void OnPost(DateTime startDate, DateTime endDate)
        {
            GetRevenueAndProfit(startDate, endDate);
        }


        public void GetRevenueAndProfit(DateTime startDate, DateTime endDate)
        {
            var filteredOrders = _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToList();

            //orders = filteredOrders;

            var totalEarning = filteredOrders.Sum(od => od.Total);
            var totalCost = filteredOrders.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity * od.Product.ImportPrice);

            var profit = totalEarning - totalCost;
            var profitPercentage = totalCost > 0 ? (profit / totalCost) * 100 : 0;


            ViewData["Earning"] = totalEarning;
            ViewData["Profit"] = profit;
            ViewData["ProfitPercentage"] = profitPercentage;
            ViewData["StartDate"] = startDate.ToString("dd MMM yyyy");
            ViewData["EndDate"] = endDate.ToString("dd MMM yyyy");
        }


        //public List<Product> GetTopSellingProducts(int topN)
        //{
        //    var topProducts = _context.OrderDetails
        //        .GroupBy(od => od.ProductId)
        //        .Select(g => new ProductSales
        //        {
        //            ProductId = g.Key,
        //            TotalQuantitySold = g.Sum(od => od.Quantity)
        //        })
        //        .OrderByDescending(ps => ps.TotalQuantitySold)
        //        .Take(topN)
        //        .ToList();

        //    return topProducts;
        //}

    }
}
