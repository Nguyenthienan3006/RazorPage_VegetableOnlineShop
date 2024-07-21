using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Access
{
    public class IndexModel : PageModel
    {
        private readonly VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;

        public IndexModel(VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IList<Role> Role { get;set; }

        public async Task OnGetAsync()
        {
            Role = await _context.Roles.ToListAsync();
            if (TempData["success"] != null)
            {
                _notyf.Success($"{TempData["success"]}");
            }
        }
    }
}
