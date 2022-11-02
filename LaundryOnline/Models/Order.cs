using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Order
    {
        [Key]
        [DisplayName("Order Id")]
        public string OrderId { get; set; }

        [DisplayName("Full Name")]
        [Required]
        [StringLength(80)]
        public string FullName { get; set; }

        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        [Phone]
        public string ContactNumber { get; set; }

        [DisplayName("Address")]
        [Required]
        [StringLength(150)]
        public string Address { get; set; }

        [DisplayName("Total Price")]
        [Required]
        [Range(1, Int32.MaxValue)]
        public double Price { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

        [DisplayName("Payment Status")]
        [DefaultValue(1)]
        public byte PaymentStatus { get; set; }

        [DisplayName("Order Status")]
        [DefaultValue(1)]
        public byte OrderStatus { get; set; }

        [DisplayName("Created At")]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [DisplayName("User")]
        [ForeignKey("User")]
        [Required(AllowEmptyStrings = true)]
        public string UserId { get; set; }

        [DisplayName("Payment")]
        [ForeignKey("Payment")]
        public string PaymentId { get; set; }

        [DisplayName("Coupon")]
        [ForeignKey("Coupon")]
        [Required(AllowEmptyStrings = true)]
        public string CouponId { get; set; }

        [DisplayName("User")]
        public User User { get; set; }

        [DisplayName("Payment")]
        public Payment Payment { get; set; }

        [DisplayName("Coupon")]
        public Coupon Coupon { get; set; }

        public ICollection<OrderItem>  OrderItems { get; set; }
    }
}
