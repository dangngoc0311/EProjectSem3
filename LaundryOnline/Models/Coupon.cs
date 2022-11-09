using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Coupon
    {
        [Key]
        [DisplayName("Coupon Id")]
        public string CouponId { get; set; }

        [DisplayName("Coupon Name")]
        [Required]
        [StringLength(150)]
        public string CouponName { get; set; }

        [DisplayName("Discount")]
        [Range(1, Int32.MaxValue)]
        public double Discount { get; set; }

        [DisplayName("Status")]
        [DefaultValue(1)]
        public byte Status { get; set; }

        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Updated At")]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<UsedCoupon> UsedCoupons { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
