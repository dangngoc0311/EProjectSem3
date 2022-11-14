using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly LaundryOnlineContext _context;
        public FooterViewComponent(LaundryOnlineContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var config = _context.Config.Where(c => c.Status == 1).First();
            return View(config);
        }
    }
}
