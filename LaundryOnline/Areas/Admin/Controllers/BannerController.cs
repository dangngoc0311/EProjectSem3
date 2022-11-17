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
    public class BannerController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public BannerController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        // GET: Admin/Banner
        public async Task<IActionResult> Index(int? page, string name)
        {
            page = page ?? 1;
            int pageSize = 6;
            var banner = _context.Banners.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                banner = banner.Where(j => j.BannerTitle.Contains(name));
            }
            return View(await banner.ToPagedListAsync(page, pageSize));
        }

    // GET: Admin/Banner/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.BannerId == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }


        // GET: Admin/Banner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BannerId,BannerTitle,Image")] Banner banner)
        {
            
            banner.Status = 1;
            banner.CreatedAt = DateTime.UtcNow.Date;
            if (ModelState.IsValid)
            {
               

                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\banner", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        banner.Image = FileName;
                        _context.Add(banner);
                        await _context.SaveChangesAsync();
                    }

                }
                _toastNotification.AddSuccessToastMessage("Create new banner successfully");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Create new banner failed");
                return View(banner);
            }
            
        }

        // GET: Admin/Banner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BannerId,BannerTitle,Image,CreatedAt,Status")] Banner banner,string img)
        {
            if (id != banner.BannerId)
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

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\banner", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        banner.Image = FileName;
                    }
                    }
                    else
                    {
                        banner.Image = img;
                    }
                    _context.Entry(banner).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage("Update successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.BannerId))
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
            return View(banner);
        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            try
            {
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Delete banner " + banner.BannerTitle + " successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Delete banner " + banner.BannerTitle + " failed");
            }

            return RedirectToAction(nameof(Index));
        }


        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.BannerId == id);
        }
    }
}
