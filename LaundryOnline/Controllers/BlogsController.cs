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
    public class BlogsController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public BlogsController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index(int? page, string name)
        {
            page = page ?? 1;
            int pageSize = 3;
            var blogs = _context.Blogs.Include(b=>b.User).Where(b => b.Status == 1).AsQueryable();
            ViewBag.blogNews = _context.Blogs.Include(b => b.User).Where(b=>b.Status==1).Take(3);
            
            if (!string.IsNullOrEmpty(name))
            {
                blogs = blogs.Where(j => j.Title.Contains(name));
            }
            return View(await blogs.ToPagedListAsync(page, pageSize));
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ViewBag.blogNews = _context.Blogs.Include(b => b.User).Take(3);
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }
    }
}