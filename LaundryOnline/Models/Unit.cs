using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Unit
    {
        [Key]
        [DisplayName("Unit Id")]
        public string UnitId { get; set; }

        [DisplayName("Unit Name")]
        [Required]
        [StringLength(150)]
        public string UnitName { get; set; }

        [DisplayName("Unit Price")]
        [Required]
        [Range(1, Int32.MaxValue)]
        public double UnitPrice { get; set; }

        [ForeignKey("Service")]
        public string ServiceId { get; set; }

        [DisplayName("Service")]
        public Service Service { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
