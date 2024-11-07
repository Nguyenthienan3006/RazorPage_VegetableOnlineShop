using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VegetablesOnlineShop.Helpper;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Products
{
    public class CreateModel : PageModel
    {
        private readonly VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;
        public CreateModel(VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult OnGet()
        {
            if(TempData["fail"] != null)
            {
                _notyf.Error($"{TempData["fail"]}");
            }
         

            ViewData["CaId"] = new SelectList(_context.Categories, "CaId", "CaName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile fThumb)
        {

            Product.ProductName = Utilities.ToTitleCase(Product.ProductName);

            if (fThumb != null)
            {
                string extension = Path.GetExtension(fThumb.FileName); 
                string image = Utilities.SEOUrl(fThumb.FileName) + extension;
                Product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
            }

            if (string.IsNullOrEmpty(Product.Thumb))
            {
                
                TempData["fail"] = "Please select picture only !";
                return RedirectToPage("Create");
            }
            Product.DateCreated = DateTime.Now;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            TempData["success"] = "Created successfully !";
            return RedirectToPage("./Index");
        }
    }
}

