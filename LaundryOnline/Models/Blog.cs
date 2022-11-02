using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Blog
    {
        [Key]
        [DisplayName("Blog Id")]
        public string BlogId { get; set; }

        [DisplayName("Blog Title")]
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [DisplayName("Image")]
        public string Image { get; set; }

        [DisplayName("Blog Content")]
        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }

        [DisplayName("Status")]
        [DefaultValue(1)]
        public byte Status { get; set; }

        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("User")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [DisplayName("User")]
        public User User { get; set; }
    }
}
