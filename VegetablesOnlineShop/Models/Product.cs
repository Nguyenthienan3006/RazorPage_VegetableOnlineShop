using System;
using System.Collections.Generic;

namespace VegetablesOnlineShop.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ShortDesc { get; set; }
        public string? Description { get; set; }
        public int CaId { get; set; }
        public int Price { get; set; }
        public string? Thumb { get; set; }
        public string? Video { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSeller { get; set; }
        public bool Active { get; set; }
        public int? UnitslnStock { get; set; }
        public int? ImportPrice { get; set; } 


        public virtual Category Ca { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
