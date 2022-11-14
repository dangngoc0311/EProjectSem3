using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.ViewComponents
{
    public class ConfigViewComponent : ViewComponent
    {
        private readonly LaundryOnlineContext _context;
        public ConfigViewComponent(LaundryOnlineContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var config =  _context.Config.Where(c=>c.Status==1).FirstOrDefault();
            return View(config);
        }
    }
}
