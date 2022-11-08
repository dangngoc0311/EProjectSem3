using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.ViewComponents
{
    public class ServicesViewComponent:ViewComponent
    {
        private readonly LaundryOnlineContext _context;
        public ServicesViewComponent(LaundryOnlineContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var services = _context.Services;
            return View(services);
        }
    }
}
