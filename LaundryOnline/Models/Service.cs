using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Service
    {
        [Key]
        [DisplayName("Service Id")]
        public string ServiceId { get; set; }

        [DisplayName("Service Name")]
        [Required]
        [StringLength(150)]
        public string ServiceName { get; set; }

        [DisplayName("Description")]
        [Column(TypeName = "ntext")]
        [Required]
        public string ServiceDescription { get; set; }

        public ICollection<Unit> Units { get; set; }
    }
}
