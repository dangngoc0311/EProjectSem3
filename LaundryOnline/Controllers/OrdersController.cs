using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaundryOnline.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace LaundryOnline.Controllers
{
    public class OrdersController : Controller
    {
        private readonly LaundryOnlineContext _context;

        public OrdersController(LaundryOnlineContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var laundryOnlineContext = _context.Orders.Include(o => o.Coupon).Include(o => o.Payment).Include(o => o.User);
            return View(await laundryOnlineContext.ToListAsync());
        }

        // GET: Orders/Details/5
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
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            List<Cart> listCart = new List<Cart>();
            var cartInSession = HttpContext.Session.GetString("ShoppingCart");
            if (cartInSession != null)
            {
                listCart = JsonConvert.DeserializeObject<List<Cart>>(cartInSession);
            }
            ViewBag.Carts = listCart;
            ViewBag.Payment = _context.Payments;

            if (HttpContext.Session.GetString("Coupon") != null)
            {
                ViewBag.Coupon = _context.Coupons.Find(HttpContext.Session.GetString("Coupon"));
            }
            if (HttpContext.Session.GetString("CustomerId") != null)
            {
                var user = _context.Users.Find(HttpContext.Session.GetString("CustomerId"));
                ViewBag.Users = user;
            }
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            order.PaymentStatus = 0;
            order.OrderStatus = 0;
            order.CreatedAt = DateTime.UtcNow.Date;
            order.UpdatedAt = DateTime.UtcNow.Date;
           
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Payment = _context.Payments;
            List<Cart> listCart = new List<Cart>();
            var cartInSession = HttpContext.Session.GetString("ShoppingCart");
            if (cartInSession != null)
            {
                listCart = JsonConvert.DeserializeObject<List<Cart>>(cartInSession);
            }
            ViewBag.Carts = listCart;
            if (HttpContext.Session.GetString("Coupon") != null)
            {
                ViewBag.Coupon = _context.Coupons.Find(HttpContext.Session.GetString("Coupon"));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId", order.CouponId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", order.PaymentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,FullName,EmailAddress,ContactNumber,Address,Price,Note,PaymentStatus,OrderStatus,CreatedAt,UpdatedAt,UserId,PaymentId,CouponId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId", order.CouponId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "PaymentId", "PaymentId", order.PaymentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
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

        // POST: Orders/Delete/5
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
    }
}
