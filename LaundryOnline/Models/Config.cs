using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Config
    {
        [Key]
        [DisplayName("Config Id")]
        public string ConfigId { get; set; }


        [DisplayName("Phone Number")]
        [Column(TypeName = "ntext")]
        [Required]
        [Phone]
        public string Description { get; set; }

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

        [DisplayName("Status")]
        [DefaultValue(1)]
        public byte Status { get; set; }

        [DisplayName("Image")]
        public string Image { get; set; }
    }
}
