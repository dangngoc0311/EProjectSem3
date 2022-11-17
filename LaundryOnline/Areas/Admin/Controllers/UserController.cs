﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;

namespace LaundryOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public UserController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        public async Task<IActionResult> Index(int? page, string name)
        {
            page = page ?? 1;
            int pageSize = 3;
            var users = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(j => j.UserName.Contains(name) || j.FullName.Contains(name) || j.EmailAddress.Contains(name));
            }
            return View(await users.ToPagedListAsync(page, pageSize));
        }
  
        public async Task<IActionResult> Edit(string id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                if (user.Status == 1)
                {
                    user.Status = 0;
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    user.Status = 1;
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));      
            }
            else
            {
                return NotFound();
            }
 
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        public async Task<IActionResult> Details()
        {

            var user = await _context.Users.FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}