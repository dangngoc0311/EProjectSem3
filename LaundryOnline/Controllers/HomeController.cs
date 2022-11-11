using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaundryOnline.Models;
using NToastNotify;
using Microsoft.EntityFrameworkCore;

namespace LaundryOnline.Controllers
{
    public class HomeController : Controller
    {

        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public HomeController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        public IActionResult Index()
        {
            ViewBag.Banner = _context.Banners.Where(b => b.Status == 1);
            ViewBag.Service = _context.Services;
            ViewBag.Blogs = _context.Blogs.Where(b => b.Status == 1).Include(b => b.User); 
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
