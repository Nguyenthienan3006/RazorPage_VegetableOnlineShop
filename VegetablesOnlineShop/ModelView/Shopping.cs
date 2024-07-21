using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VegetablesOnlineShop.ModelView
{
    public class Shopping
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter Full Name")]
        public string FullName { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter the phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Delivery address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Please select a shipping unit")]
        //public int ShipperId { get; set; }

    }
}
