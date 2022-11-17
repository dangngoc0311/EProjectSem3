using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryOnline.Models;
using X.PagedList;
using QRCoder;
using System.Drawing;
using System.IO;
using NToastNotify;

namespace LaundryOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;
        public OrdersController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index(int? page,string name,string email,string address)
        {
            page = page ?? 1;
            int pageSize = 6;
            var laundryOnlineContext = _context.Orders.Include(o => o.Coupon).Include(o => o.Payment).Include(o => o.User).AsQueryable();
            ViewBag.LaundryStatus = laundryOnlineContext;
            if (!string.IsNullOrEmpty(name))
            {
                laundryOnlineContext = laundryOnlineContext.Where(x => x.FullName.Contains(name) ||  x.Address.Contains(address) || x.EmailAddress.Contains(email));
            }
            return View(await laundryOnlineContext.ToPagedListAsync(page, pageSize));
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Coupon)
                .Include(o => o.Payment)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            ViewBag.OrderDetail = _context.OrderItems.Include(o => o.Unit).Where(o => o.OrderId == id).ToList();

            if (order == null)
            {
                return NotFound();
            }
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(order.OrderId, QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            ViewBag.QrCode = BitmapToBytes(qrCodeImage);
            return View(order);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId");
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,FullName,EmailAddress,ContactNumber,Address,Price,Note,PaymentStatus,OrderStatus,CreatedAt,UpdatedAt,UserId,PaymentId,CouponId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId", order.CouponId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", order.PaymentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Edit(string id)
        {
            var order = _context.Orders.Find(id);
            if(order != null)
            {
                if (order.OrderStatus == 0)
                {
                    order.OrderStatus = 1;
                    _context.Entry(order).State = EntityState.Modified;
                    _toastNotification.AddSuccessToastMessage("Change order status successfully");
                    await _context.SaveChangesAsync();
                }
                else if(order.OrderStatus == 1)
                {
                    order.OrderStatus = 2;
                    _context.Entry(order).State = EntityState.Modified;
                    _toastNotification.AddSuccessToastMessage("Change order status  successfully");
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Change order status  failed");
                return NotFound();
            }
        }

        public async Task<IActionResult> ChangePayment(string id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                if (order.PaymentStatus == 0)
                {
                    order.PaymentStatus = 1;
                    _context.Entry(order).State = EntityState.Modified;
                    _toastNotification.AddSuccessToastMessage("Change payment status successfully");
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Change payment status  failed");
                return NotFound();
            }
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Coupon)
                .Include(o => o.Payment)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
