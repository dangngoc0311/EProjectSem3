using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.Models
{
    public class Cart
    {
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public string Image { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
