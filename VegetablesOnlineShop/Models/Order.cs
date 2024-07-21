using System;
using System.Collections.Generic;

namespace VegetablesOnlineShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? TransactStatusId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Note { get; set; }
        public int? ShipperId { get; set; }
        public string? Address { get; set; }
        public int? Total { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Shipper? Shipper { get; set; }
        public virtual TransactStatus? TransactStatus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
