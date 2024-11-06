using System;
using System.Collections.Generic;

namespace VegetablesOnlineShop.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool Active { get; set; }
        public string? RoleName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateLogin { get; set; }

        public virtual Role? Role { get; set; }
    }
}
