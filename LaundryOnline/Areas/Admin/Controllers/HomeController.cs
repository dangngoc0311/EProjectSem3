using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaundryOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly LaundryOnlineContext _context;
        public HomeController(LaundryOnlineContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Users = _context.Users.Where(u => u.Role == 1).Count();
            ViewBag.Orders = _context.Orders.Where(o => o.OrderStatus != 3).Count();
            ViewBag.CancelOrders = _context.Orders.Where(o => o.OrderStatus == 3).Count();
            ViewBag.TotalOrders = _context.Orders.Where(o => o.OrderStatus != 3).Sum(o => o.Price);
            ViewBag.BlogNews = _context.Blogs.Where(b => b.Status == 1).Take(4);
            ViewBag.ListOrders = _context.Orders.OrderByDescending(o => o.CreatedAt).Take(6);
            ViewBag.Charts = from i in _context.Orders
                             group i by i.CreatedAt.Value.Month into grp
                             select new Charts { Month = grp.Key, Count = grp.Sum(i => i.Price) };

            foreach (var item in _context.Orders)
            {
                if (item.OrderStatus == 2)
                {
                    item.PaymentStatus = 1;
                    _context.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    item.PaymentStatus = 0;
                    _context.Entry(item).State = EntityState.Modified;
                }
            }
            return View();
        }
    }
    public class Charts
    {
        public int Month { get; set; }
        public double Count { get; set; }
    }
}

//select new Charts{ Month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(grp.Key), Count = grp.Sum(i => i.Price) };