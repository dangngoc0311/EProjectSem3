using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class OrderItem
    {
        [Key]
        [DisplayName("Id")]
        public int OrderItemId { get; set; }

        [DisplayName("Price")]
        [Required]
        [Range(1, Int32.MaxValue)]
        public double PriceUnit { get; set; }

        [DisplayName("Quantity")]
        [Required]
        [Range(1, Int32.MaxValue)]
        public int Quantity { get; set; }

        [DisplayName("Order")]
        [ForeignKey("Order")]
        public string OrderId { get; set; }

        [DisplayName("Unit")]
        [ForeignKey("Unit")]
        public string UnitId { get; set; }

        [DisplayName("Order")]
        public Order Order { get; set; }

        [DisplayName("Unit")]
        public Unit Unit { get; set; }
    }
}
