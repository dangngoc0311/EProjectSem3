using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class ChangePassword
    {
        [DisplayName("User Id")]
        public string UserId { get; set; }

        [DisplayName("Password")]
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [DisplayName("ConfirmPassword")]
        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [MinLength(6)]
        public string ConfirmPassword { get; set; }

    }
}
