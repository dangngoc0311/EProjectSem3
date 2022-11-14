using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(int? page, string name)
        {
            page = page ?? 1;
            int pageSize = 6;
            var units = _context.Units.AsQueryable();
            return View(await units.ToPagedListAsync(page, pageSize));
        }
    }
}