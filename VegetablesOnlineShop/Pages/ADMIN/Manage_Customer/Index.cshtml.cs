using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VegetablesOnlineShop.Models;
using VegetablesOnlineShop.ModelView;

namespace VegetablesOnlineShop.Pages.ADMIN.Manage_Customer
{
    public class IndexModel : PageModel
    {
        private readonly VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext _context;
        private readonly INotyfService _notyf;
        public IList<Customer> customers { get; set; }
        public IList<CustomerVM> customersVM { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public IndexModel(VegetablesOnlineShop.Models.PRN221_OnlineShopDBContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }


        public async Task OnGetAsync(int? id, string? search, bool? active, int?id2)
        {
            if (id2 != null)
            {
                var account = await _context.Customers.Where(p => p.CustomerId == id2).FirstOrDefaultAsync();
                if (active == true)
                {
                    account.Active = false;
                    TempData["success"] = "Locked Account Successfully !";
                }
                else
                {
                    account.Active = true;
                    TempData["success"] = "UnLocked Account Successfully !";
                }
                _context.Update(account);
                await _context.SaveChangesAsync();
                
            }

            //Paging
            int pageSize = 10; // số lượng mục trên mỗi trang
            CurrentPage = id ?? 1; // trang hiện tại
            CurrentPage = (CurrentPage == 0) ? 1 : CurrentPage;
            var count = _context.Customers.Count();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            var skipAmount = (CurrentPage - 1) * pageSize;


            //search
            if (!string.IsNullOrEmpty(search))
            {
                count = _context.Customers.Where(p => p.Email.Contains(search.Trim())).Count();
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);

                customers = await _context.Customers.Include(p => p.Location).Where(p => p.Email.Contains(search.Trim()))
                .OrderBy(x => x.CustomerId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();                

            }
            else
            {

                //report về customer


                customers = await _context.Customers.Include(p => p.Location)
                .OrderBy(x => x.CustomerId)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();

                customersVM = new List<CustomerVM>();


                foreach (var item in customers)
                {
                    var totalOrder = _context.Orders.Where(o => o.CustomerId == item.CustomerId).Count();
                    var totalSpend = _context.Orders.Where(o => o.CustomerId == item.CustomerId).Select(o => o.Total).Sum();

                    CustomerVM customerVM = new CustomerVM
                    {
                        CustomerId = item.CustomerId,
                        FullName = item.FullName,
                        Active = item.Active,
                        TotalOrders = totalOrder,
                        TotalSpend = totalSpend

                    };

                    customersVM.Add(customerVM);
                }

            }
            if (TempData["success"] != null)
            {
                _notyf.Success($"{TempData["success"]}");
            }
        }
    }
}
