using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
