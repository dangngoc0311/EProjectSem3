using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class User
    {
        [Key]
        [DisplayName("User Id")]
        public string UserId { get; set; }

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

        [DisplayName("UserName")]
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [DisplayName("Status")]
        [DefaultValue(1)]
        public byte Status { get; set; }

        [DisplayName("Role")]
        [DefaultValue(1)]
        public byte Role { get; set; }

        public ICollection<Blog> Blogs { get; set; }

        public ICollection<UsedCoupon> UsedCoupons { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
