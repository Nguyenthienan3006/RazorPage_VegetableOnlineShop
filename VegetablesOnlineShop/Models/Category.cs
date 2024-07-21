using System;
using System.Collections.Generic;

namespace VegetablesOnlineShop.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CaId { get; set; }
        public string? CaName { get; set; }
        public string? Description { get; set; }
        public int? Levels { get; set; }
        public int? Ordering { get; set; }
        public bool Published { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
