using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryOnline.Models;
using NToastNotify;
using X.PagedList;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace LaundryOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
            var blogs = _context.Blogs.Include(b=>b.User).AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                blogs = blogs.Where(j => j.Title.Contains(name));
            }
            return View(await blogs.ToPagedListAsync(page, pageSize));
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(string id)
        {
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

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Image,Content")] Blog blog)
        {
            blog.Status = 1;
            blog.CreatedAt = DateTime.UtcNow.Date;
            blog.UserId = "f44d63ce-2cae-4132-a01b-14b4911475e1";


            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\blogs", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        blog.Image = FileName;
                        _context.Add(blog);
                        await _context.SaveChangesAsync();
                    }

                }
                _toastNotification.AddSuccessToastMessage("Create new blog successfully");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Create new blog failed");
                return View(blog);
            }
        }


        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Blog blog, string img)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var FileName = file.FileName;

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\blogs", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            blog.Image = FileName;
                        }
                    }
                    else
                    {
                        blog.Image = img;
                    }
                    _context.Entry(blog).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage("Update successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Update failed");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            try
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Delete Blogs " + blog.Title + " successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Delete Blogs " + blog.Title + " failed");
            }

            return RedirectToAction(nameof(Index));
        }
        private bool BlogExists(string id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}
