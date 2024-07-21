using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Products
{
    public class IndexModel : PageModel
    {
        private readonly VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;
        public IList<Product> Product { get; set; }
        public IList<Category> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public IndexModel(VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public async Task OnGetAsync(int? id, int? cateFillter, string? search)
        {
            Categories = await _context.Categories.ToListAsync();
            int pageSize = 10; // số lượng mục trên mỗi trang
            CurrentPage = id ?? 1; // trang hiện tại
            CurrentPage = (CurrentPage == 0) ? 1 : CurrentPage;
            var count = _context.Products.Count();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            var skipAmount = (CurrentPage - 1) * pageSize;
          
            if (cateFillter != null)
            {
                count = _context.Products.Where(p => p.CaId == cateFillter).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                Product = await _context.Products.Where(p => p.CaId == cateFillter)
                .OrderBy(x => x.ProductId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();              
                ViewData["cateTemp"] = cateFillter;
            }

            if (!string.IsNullOrEmpty(search))
            {
                count = _context.Products.Where(p => p.ProductName.Contains(search.Trim())).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                Product = await _context.Products.Where(p => p.ProductName.Contains(search.Trim()))
                .OrderBy(x => x.ProductId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
                ViewData["search"] = search;
            }

            if(cateFillter == null && string.IsNullOrEmpty(search))
            {
                Product = await _context.Products
                .OrderBy(x => x.ProductId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
            }
            if (TempData["success"] != null)
            {
                _notyf.Success($"{TempData["success"]}");
            }
        }
    }
}
