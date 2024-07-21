using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VegetablesOnlineShop.Models;
using VegetablesOnlineShop.ModelView;

namespace VegetablesOnlineShop.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly PRN221_OnlineShopDBContext _context;
        public LoginViewModel login { get; set; }
        public bool isCheckout { get; set; }
        public LoginModel(PRN221_OnlineShopDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGet(bool logout)
        {
            if (logout)
            {
                HttpContext.Session.Clear();
               
                
            }

            if (TempData["checkout"] != null)
            {
                isCheckout = true;
            }
            else
            {
                isCheckout = false;
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(int role)
        {
            if (role == 0)
            {
                var emailCheck = _context.Customers.SingleOrDefault(p => p.Email.ToLower() == login.Email.ToLower());
                if (emailCheck == null)
                {
                    ModelState.AddModelError("login.Email", "The Email does not exists !");
                }
                else
                {
                    if (emailCheck.Password.ToLower() != login.Password)
                    {
                        ModelState.AddModelError("login.Password", "Incorrect password !");
                    }
                }
                if (ModelState.IsValid)
                {
                    if (emailCheck.Active == true)
                    {
                        emailCheck.LastLogin = DateTime.Now;
                        _context.Customers.Update(emailCheck);
                        _context.SaveChanges();
                        HttpContext.Session.SetString("CustomerEmail", login.Email);
                        if(isCheckout == true)
                        {
                            return RedirectToPage("/Common/Checkout");
                        }
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        TempData["fail"] = "Your account has been banned !";
                    }
                }
            }
            else
            {
                var emailCheck = _context.Accounts.SingleOrDefault(p => p.Email.ToLower() == login.Email.ToLower());
                if (emailCheck == null)
                {
                    ModelState.AddModelError("login.Email", "The Email does not exists !");
                }
                else
                {
                    if (emailCheck.Password.ToLower() != login.Password)
                    {
                        ModelState.AddModelError("login.Password", "Incorrect password !");
                    }
                }
                if (ModelState.IsValid)
                {
                    if (emailCheck.Active == true)
                    {
                        emailCheck.LastLogin = DateTime.Now;
                        _context.Accounts.Update(emailCheck);
                        _context.SaveChanges();
                        HttpContext.Session.SetString("StaffEmail", login.Email);
                        return RedirectToPage("/ADMIN/HomePage");
                    }
                    else
                    {
                        TempData["fail"] = "Your account has been banned !";
                    }
                }
            }
            return Page();
        }
    }
}
