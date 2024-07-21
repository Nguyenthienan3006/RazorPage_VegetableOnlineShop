using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;


namespace VegetablesOnlineShop.Pages
{
    [BindProperties]
    public class ProfileModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        public Customer Customer { get; set; }
        private readonly INotyfService _notyf;
        public ProfileModel(PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public void OnGet()
        {
            var cusEmail = HttpContext.Session.GetString("CustomerEmail");
            if (cusEmail != null)
            {
                Customer = _context.Customers.Include(p => p.Location).Where(p => p.Email == cusEmail).SingleOrDefault();
            }
            if (Customer == null)
            {
                Customer = new Customer(); // Initialize a new Customer object if none is found
            }
            ViewData["LocationIdForEdit"] = new SelectList(_context.Locations, "LocationId", "Name");
        }


        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                Customer.Active = true;
                _context.Customers.Update(Customer);
                _context.SaveChanges();
                //_notyf.Success("Edied successfully !");
                TempData["success"] = "Edited successfully !";
            }
        }
    }
}
