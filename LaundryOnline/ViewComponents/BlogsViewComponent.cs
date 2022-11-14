using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryOnline.ViewComponents
{
    public class BlogsViewComponent : ViewComponent
    {
        private readonly LaundryOnlineContext _context;
        public BlogsViewComponent(LaundryOnlineContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var blogs = _context.Blogs.Where(b=>b.Status==1).OrderByDescending(b=>b.CreatedAt).Take(2);
            return View(blogs);
        }
    }
}
