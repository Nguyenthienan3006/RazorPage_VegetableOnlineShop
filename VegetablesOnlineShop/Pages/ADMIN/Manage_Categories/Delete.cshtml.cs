using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Categories
{
    public class DeleteModel : PageModel
    {
        private readonly VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext _context;

        public DeleteModel(VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FirstOrDefaultAsync(m => m.CaId == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FindAsync(id);

            if (Category != null)
            {
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
            }
            TempData["success"] = "Deleted successfully !";
            return RedirectToPage("./Index");
        }
    }
}
