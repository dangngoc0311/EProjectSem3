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
namespace LaundryOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var services =_context.Services.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                services = services.Where(j => j.ServiceName.Contains(name));
            }
            return View(await services.ToPagedListAsync(page, pageSize));
        }

        // GET: Admin/Services/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Admin/Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,ServiceDescription")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Create new service successfully");
                return RedirectToAction(nameof(Index));
            }
            _toastNotification.AddErrorToastMessage("Create new service failed");
            return View(service);
        }

        // GET: Admin/Services/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: Admin/Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ServiceId,ServiceName,ServiceDescription")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Update successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
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
            return View(service);
        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var service = await _context.Services.FindAsync(id);
            try
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Delete service " + service.ServiceName + " successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Delete service " + service.ServiceName + " failed");
            }
          
            return RedirectToAction(nameof(Index));
        }


        private bool ServiceExists(string id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
