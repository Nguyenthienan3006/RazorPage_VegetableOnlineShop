using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Products
{
    public class DetailsModel : PageModel
    {
        private readonly VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext _context;

        public DetailsModel(VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products
                .Include(p => p.Ca).FirstOrDefaultAsync(m => m.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
