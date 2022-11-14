using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class ContactModel
    {
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

        [Required]
        [DisplayName("Message")]
        [Column(TypeName = "ntext")]
        public string Message { get; set; }

        [Required]
        [DisplayName("Subject")]
        public string Subject { get; set; }
    }
}
