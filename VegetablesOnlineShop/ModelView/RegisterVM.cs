using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VegetablesOnlineShop.ModelView
{
    public class RegisterVM
    {
        [Key]
        public int CustomerId { get; set; }
        [Display(Name = "First and last name")]
        [Required(ErrorMessage = "Please enter Full Name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(11)]
        [Required(ErrorMessage = "Please enter the phone number")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        [MinLength(5, ErrorMessage = "You need to set a password of at least 5 characters")]
        public string Password { get; set; }
        [MinLength(5, ErrorMessage = "You need to set a password of at least 5 characters")]
        [Display(Name = "Enter the password")]
        [Compare("Password", ErrorMessage = "Please enter the same password")]
        public string ConfirmPassword { get; set; }
    }
}
