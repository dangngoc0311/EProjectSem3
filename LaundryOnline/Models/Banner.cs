using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Banner
    {
        [Key]
        [DisplayName("Banner Id")]
        public int BannerId { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("Title Banner")]
        public string BannerTitle { get; set; }

        [DisplayName("Image")]
        public string Image { get; set; }

        [DisplayName("Create At")]
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Status")]
        [DefaultValue(1)]
        public byte Status { get; set; }
    }
}
