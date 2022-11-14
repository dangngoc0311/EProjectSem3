using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;

namespace LaundryOnline.Controllers
{
    public class CartController : Controller, IActionFilter
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;
        private List<Cart> listCarts = new List<Cart>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cartInSession = HttpContext.Session.GetString("ShoppingCart");
            if (cartInSession != null)
            {
                listCarts = JsonConvert.DeserializeObject<List<Cart>>(cartInSession);
            }
            
            base.OnActionExecuting(context);
        }

        public CartController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("CustomerLogin") != null)
            {
                ViewBag.coupons = _context.Coupons;
            }
            if (HttpContext.Session.GetString("Coupon") != null)
            {
                ViewBag.Coupon = _context.Coupons.Find(HttpContext.Session.GetString("Coupon"));
            }
            return View(listCarts);
        }
        public IActionResult AddCart(string id)
        {
            if (listCarts.Any(c => c.UnitId == id))
            {
                listCarts.Where(c => c.UnitId == id).First().Quantity += 1;
            }
            else
            {
                var unit = _context.Units.Find(id);
                var cart = new Cart()
                {
                    UnitId = unit.UnitId,
                    UnitName = unit.UnitName,
                    UnitPrice = unit.UnitPrice,
                    Quantity = 1
                };
                listCarts.Add(cart);
            }
            HttpContext.Session.SetString("ShoppingCart", JsonConvert.SerializeObject(listCarts));
            _toastNotification.AddSuccessToastMessage("Add cart successfully");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveItemCart(string id)
        {
            if (listCarts.Any(c => c.UnitId == id))
            {
                var remove = listCarts.Where(c => c.UnitId == id).First();
                listCarts.Remove(remove);
                HttpContext.Session.SetString("ShoppingCart", JsonConvert.SerializeObject(listCarts));
                _toastNotification.AddSuccessToastMessage("Remove item successfully");
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult IncreaseQtyCart(string id)
        {
            if (HttpContext.Session.GetString("CustomerLogin") != null)
            {
                ViewBag.coupons = _context.Coupons;
            }
            if (listCarts.Any(c => c.UnitId == id))
            {
                listCarts.Where(c => c.UnitId == id).First().Quantity += 1;
                HttpContext.Session.SetString("ShoppingCart", JsonConvert.SerializeObject(listCarts));
                _toastNotification.AddSuccessToastMessage("Update cart successfully");
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DecreaseQtyCart(string id)
        {
            if (listCarts.Any(c => c.UnitId == id))
            {
                var cart = listCarts.Where(c => c.UnitId == id).First();
                if (cart.Quantity <= 1)
                {
                    cart.Quantity = 1;
                }
                else
                {
                    cart.Quantity -= 1;
                }
                HttpContext.Session.SetString("ShoppingCart", JsonConvert.SerializeObject(listCarts));
                _toastNotification.AddSuccessToastMessage("Update cart successfully");
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("ShoppingCart");
            _toastNotification.AddSuccessToastMessage("Clear cart successfully");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddCoupon(string coupon)
        {
            if (String.IsNullOrEmpty(coupon))
            {
                HttpContext.Session.Remove("Coupon");
            }
            else
            {
                var cou = _context.Coupons.Find(coupon);
                HttpContext.Session.SetString("Coupon", cou.CouponId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}