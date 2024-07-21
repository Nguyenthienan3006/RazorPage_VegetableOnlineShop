using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Access
{
    public class CreateModel : PageModel
    {
        private readonly VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext _context;

        public CreateModel(VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Role Role { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(Role);
                await _context.SaveChangesAsync();
                TempData["success"] = "Created Role Successfully !";
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
