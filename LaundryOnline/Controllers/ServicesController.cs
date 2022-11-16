using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace LaundryOnline.Controllers
{
    public class ServicesController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public ServicesController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        // GET: Admin/Services
        public async Task<IActionResult> Index(int? page, string name, string serviceId)
        {
            page = page ?? 1;
            int pageSize = 6;
            var units = _context.Units.Include(s => s.Service).AsQueryable();
            var services = _context.Services;
            ViewBag.Service = services;
            ViewBag.ServiceId = serviceId;
            if (!String.IsNullOrEmpty(serviceId) && !String.IsNullOrEmpty(name)) 
            {
                units = units.Where(s => s.ServiceId == serviceId).Where(s => s.UnitName.Contains(name));
            }else if (!String.IsNullOrEmpty(serviceId) && String.IsNullOrEmpty(name))
            {
                units = units.Where(s => s.ServiceId==serviceId);
            }
            else if(String.IsNullOrEmpty(serviceId) && !String.IsNullOrEmpty(name))
            {
                units = units.Where(s => s.UnitName.Contains(name));
            }
            return View(await units.ToPagedListAsync(page, pageSize));
        }
    }
}