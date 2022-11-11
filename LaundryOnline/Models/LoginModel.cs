using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class LoginModel
    {
        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DisplayName("Password")]
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [DisplayName("Remember me")]
        public bool Remember { get; set; }
    }
}
