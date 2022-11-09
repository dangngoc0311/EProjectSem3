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
    public class UnitsController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;
        public UnitsController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        // GET: Admin/Units
        public async Task<IActionResult> Index(int? page, string name)
        {
            page = page ?? 1;
            int pageSize = 6;
            var unit = _context.Units.Include(x => x.Service).AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                unit = unit.Where(j => j.UnitName.Contains(name));
            }
            return View(await unit.ToPagedListAsync(page, pageSize));
        }

        // GET: Admin/Units/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Service)
                .FirstOrDefaultAsync(m => m.UnitId == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // GET: Admin/Units/Create
        public IActionResult Create()
        {
            ViewData["ServiceName"] = new SelectList(_context.Services, "ServiceId", "ServiceName");
            return View();
        }

        // POST: Admin/Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitId,UnitName,UnitPrice,ServiceId")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unit);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Create new service successfully");
                return RedirectToAction(nameof(Index));
            }
            _toastNotification.AddErrorToastMessage("Create new service failed");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", unit.ServiceId);
            return View(unit);
        }

        // GET: Admin/Units/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }
            ViewData["Service"] = new SelectList(_context.Services, "ServiceId", "ServiceName", unit.ServiceId);
            return View(unit);
        }

        // POST: Admin/Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UnitId,UnitName,UnitPrice,ServiceId")] Unit unit)
        {
            if (id != unit.UnitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unit);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Update successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.UnitId))
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
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", unit.ServiceId);
            return View(unit);
        }

        // GET: Admin/Units/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Service)
                .FirstOrDefaultAsync(m => m.UnitId == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Admin/Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var unit = await _context.Units.FindAsync(id);
            try
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Delete service " + unit.UnitName + " successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Delete service " + unit.UnitName + " failed");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(string id)
        {
            return _context.Units.Any(e => e.UnitId == id);
        }
    }
}
