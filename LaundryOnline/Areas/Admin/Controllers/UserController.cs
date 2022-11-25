using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BarcodeLib;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Rotativa.AspNetCore;
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
            var users = _context.Users.Where(x => x.Role == 1).AsQueryable();
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
        public IActionResult Details()
        {
            User user = _context.Users.Where(u=>u.UserId== HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult ViewAsPDF()
        {
            List<User> users = _context.Users.ToList(); 
            return new ViewAsPdf(users, ViewData);
        }
        private static Byte[] ConvertImageToByte(Image img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}