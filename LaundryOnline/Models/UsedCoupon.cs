
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class UsedCoupon
    {
        [Key]
        [DisplayName("Id")]
        public int UsedCouponId { get; set; }

        [DisplayName("User")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [DisplayName("Coupon")]
        [ForeignKey("Coupon")]
        public string CouponId { get; set; }

        [DisplayName("User")]
        public User User { get; set; }

        [DisplayName("Coupon")]
        public Coupon Coupon { get; set; }
    }
}
