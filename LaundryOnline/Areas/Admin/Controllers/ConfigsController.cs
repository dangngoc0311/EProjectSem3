using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryOnline.Models;
using X.PagedList;
using System.IO;
using NToastNotify;
using Microsoft.AspNetCore.Authorization;

namespace LaundryOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ConfigsController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public ConfigsController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        // GET: Admin/Configs
        public async Task<IActionResult> Index(int? page)
        {
            page = page ?? 1;
            int pageSize = 6;
            var configs = _context.Config.AsQueryable();
            return View(await configs.ToPagedListAsync(page, pageSize));
        }

        // GET: Admin/Configs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var config = await _context.Config
                .FirstOrDefaultAsync(m => m.ConfigId == id);
            if (config == null)
            {
                return NotFound();
            }

            return View(config);
        }

        // GET: Admin/Configs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Configs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConfigId,Description,EmailAddress,ContactNumber,Address,Image")] Config config)
        {
            config.Status = 1;
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\config", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        config.Image = FileName;
                        _context.Add(config);
                        await _context.SaveChangesAsync();
                    }

                }
                _toastNotification.AddSuccessToastMessage("Create new config successfully");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Create new config failed");
                return View(config);
            }

        }

        // GET: Admin/Configs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var config = await _context.Config.FindAsync(id);
            if (config == null)
            {
                return NotFound();
            }
            return View(config);
        }

        // POST: Admin/Configs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ConfigId,Description,EmailAddress,ContactNumber,Address,Status,Image")] Config config,string img)
        {
            if (id != config.ConfigId)
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

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\config", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            config.Image = FileName;
                        }
                    }
                    else
                    {
                        config.Image = img;
                    }
                    _context.Entry(config).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage("Update successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfigExists(config.EmailAddress))
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
            return View(config);
        }

        // GET: Admin/Configs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var config = await _context.Config.FindAsync(id);
            try
            {
                _context.Config.Remove(config);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Delete config successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Delete config failed");
            }

            return RedirectToAction(nameof(Index));
        }

       
        private bool ConfigExists(string email)
        {
            return _context.Config.Any(e => e.EmailAddress == email);
        }
    }
}
