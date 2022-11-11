using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class EmailModel
    {
        public EmailModel()
        {
            From = "rubyexample001@gmail.com";
            Password = "xbhggplvmbilgycw";
        }
        public string To { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
    }
}
