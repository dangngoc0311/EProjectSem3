using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Payment
    {
        [Key]
        [DisplayName("Payment Id")]
        public string PaymentId { get; set; }

        [DisplayName("Payment Name")]
        [Required]
        [StringLength(150)]
        public string PaymentName { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
