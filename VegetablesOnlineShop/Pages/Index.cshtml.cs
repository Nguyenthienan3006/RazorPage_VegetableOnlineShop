using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;

        public IndexModel(PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        public IList<Product> ProductsList { get; set; }
        public IList<Category> CategoriesList { get; set; }
        public IList<Product> BestSellerList {  get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGet(int? cateId, int? PageId)
        {
            BestSellerList = await _context.Products.Where(p => p.BestSeller).AsNoTracking().ToListAsync();
            CategoriesList = await _context.Categories.AsNoTracking().ToListAsync();

            //phân trang 
            int pageSize = 10; 
            CurrentPage = PageId ?? 1; 
            CurrentPage = CurrentPage == 0 ? 1 : CurrentPage;
            var count = _context.Products.Count();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            var skipAmount = (CurrentPage - 1) * pageSize;

            if(cateId != null)
            {
                //tính lại tổng số trang
                count = _context.Products.Where(p => p.CaId == cateId).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);

                ProductsList = await _context.Products.Where(p => p.CaId == cateId).AsNoTracking().Include(p => p.Ca)
                    .OrderBy(p => p.ProductId)
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToListAsync();

            }
            else
            {

                ProductsList = await _context.Products.AsNoTracking().Include(p => p.Ca)
                    .OrderBy(p => p.ProductId)
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }
    }
}
