using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.Common
{
    public class ShopModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;


        public ShopModel(PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        public IList<Product> ProductsList { get; set; }
        public IList<Category> CategoriesList { get; set; }


        public async Task OnGet(int? cateId, string? searchKey, int? priceOrder)
        {
            CategoriesList = await _context.Categories.AsNoTracking().ToListAsync();

            //filter theo cate
            if(cateId != null)
            {   
                if(priceOrder != null)
                {
                    if(priceOrder == 0)
                    {
                        ProductsList = await _context.Products.Where(p => p.CaId == cateId).Include(p => p.Ca).OrderBy(p => p.Price).AsNoTracking().ToArrayAsync();
                        ViewData["PriceOrder"] = priceOrder;
                        ViewData["cateTemp"] = cateId;
                    }
                    else if(priceOrder == 1) 
                    {
                        ProductsList = await _context.Products.Where(p => p.CaId == cateId).Include(p => p.Ca).OrderByDescending(p => p.Price).AsNoTracking().ToArrayAsync();
                        ViewData["PriceOrder"] = priceOrder;
                        ViewData["cateTemp"] = cateId;
                    }
                }
                else
                {
                    ProductsList = await _context.Products.Where(p => p.CaId == cateId).Include(p => p.Ca).AsNoTracking().ToArrayAsync();
                    ViewData["cateTemp"] = cateId;
                }

               
            }
  
            
            //search by key
            if(!string.IsNullOrEmpty(searchKey))
            {
                if(priceOrder != null)
                {
                    if(priceOrder == 0)
                    {
                        ProductsList = await _context.Products.Where(p => p.ProductName.Contains(searchKey)).Include(p => p.Ca).OrderBy(p => p.Price).AsNoTracking().ToListAsync();
                        ViewData["PriceOrder"] = priceOrder;
                        ViewData["search"] = searchKey;
                    }
                    else if (priceOrder == 1)
                    {
                        ProductsList = await _context.Products.Where(p => p.ProductName.Contains(searchKey)).Include(p => p.Ca).OrderByDescending(p => p.Price).AsNoTracking().ToListAsync();
                        ViewData["PriceOrder"] = priceOrder;
                        ViewData["search"] = searchKey;
                    }
                }
                else
                {
                    ProductsList = await _context.Products.Where(p => p.ProductName.Contains(searchKey)).Include(p => p.Ca).AsNoTracking().ToListAsync();
                    ViewData["search"] = searchKey;
                }
                
            }


            //Sorting
            if(cateId == null && string.IsNullOrEmpty(searchKey))
            {
                if(priceOrder != null)
                {
                    if(priceOrder == 0)
                    {
                        ProductsList = await _context.Products.Include(p => p.Ca).OrderBy(p => p.Price).AsNoTracking().ToArrayAsync();
                        ViewData["PriceOrder"] = priceOrder;
                    }
                    else if(priceOrder == 1)
                    {
                        ProductsList = await _context.Products.Include(p => p.Ca).OrderByDescending(p => p.Price).AsNoTracking().ToArrayAsync();
                        ViewData["PriceOrder"] = priceOrder;
                    }
                }
                else
                {
                    ProductsList = await _context.Products.Include(p => p.Ca).AsNoTracking().ToArrayAsync();
                }
               
            }
        }
    }
}
