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
using System.Net.Mail;
using System.Net;
using NToastNotify;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;
using BarcodeLib;
using System.Drawing;
using QRCoder;
using X.PagedList;
using ZXing.ImageSharp;
using ZXing;
using ZXing.Common;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace LaundryOnline.Controllers
{
    public class OrdersController : Controller, IActionFilter
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
        public OrdersController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? page)
        {
            var usedId = HttpContext.Session.GetString("CustomerId");
            page = page ?? 1;
            int pageSize = 6;
            var laundryOnlineContext = _context.Orders.Include(o => o.Coupon).Include(o => o.Payment).Include(o => o.User).Where(o => o.UserId == usedId);
            return View(await laundryOnlineContext.ToPagedListAsync(page, pageSize));
        }

        public ActionResult CheckBill()
        {
            return View();
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
            ViewBag.OrderDetail = _context.OrderItems.Include(o => o.Unit).Where(o => o.OrderId == id).ToList();

            if (order == null)
            {
                return NotFound();
            }

            // Generate QRcode
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(order.OrderId, QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            ViewBag.QrCode = BitmapToBytes(qrCodeImage);

            // Generate Barcode
            Barcode barcode = new Barcode();
            Image img = barcode.Encode(TYPE.CODE128, order.OrderId, Color.Black, Color.White, 650, 80);
            ViewBag.Barcode = ConvertImageToByte(img);
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Carts = listCarts;
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
            ViewBag.Carts = listCarts;
            if (HttpContext.Session.GetString("Coupon") != null)
            {
                ViewBag.Coupon = _context.Coupons.Find(HttpContext.Session.GetString("Coupon"));
            }
            ViewBag.Payment = _context.Payments;
            order.PaymentStatus = 0;
            order.OrderStatus = 0;
            order.CreatedAt = DateTime.UtcNow.Date;
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                foreach (var item in listCarts)
                {
                    OrderItem o = new OrderItem
                    {
                        PriceUnit = item.UnitPrice,
                        Quantity = item.Quantity,
                        UnitId = item.UnitId,
                        OrderId = order.OrderId
                    };
                    _context.Add(o);
                    await _context.SaveChangesAsync();
                }
                if (HttpContext.Session.GetString("Coupon") != null)
                {
                    UsedCoupon ud = new UsedCoupon()
                    {
                        CouponId = HttpContext.Session.GetString("Coupon"),
                        UserId = HttpContext.Session.GetString("CustomerId")
                    };
                    _context.Add(ud);
                    await _context.SaveChangesAsync();
                }
                EmailModel model = new EmailModel()
                {
                    Subject = "Confirm Order",
                    To = order.EmailAddress
                };

                using (MailMessage mm = new MailMessage(model.From, model.To))
                {
                    mm.Subject = model.Subject;
                    mm.Body = BodyOrderSuccessMail(order);
                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(model.From, model.Password);
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
                HttpContext.Session.Remove("Coupon");
                HttpContext.Session.Remove("ShoppingCart");
                _toastNotification.AddSuccessToastMessage("Order successfully!! Please check your mail");
                return RedirectToAction("Details", new { id = order.OrderId });
            }


            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id, string returnUrl)
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
            if (order.OrderStatus == 0)
            {
                order.OrderStatus = 3;
                order.UpdatedAt = DateTime.UtcNow.Date;
                _context.Entry(order).State = EntityState.Modified;
                EmailModel model = new EmailModel()
                {
                    Subject = "Cancel Order",
                    To = order.EmailAddress
                };

                using (MailMessage mm = new MailMessage(model.From, model.To))
                {
                    mm.Subject = model.Subject;
                    mm.Body = BodyCancelOrderMail(order);
                    mm.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(model.From, model.Password);
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
                _toastNotification.AddSuccessToastMessage("Your order has been cancelled");
                await _context.SaveChangesAsync();
            }
            if (HttpContext.Session.GetString("CustomerId") != null)
            {
                if (Url.IsLocalUrl(returnUrl))

                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });

                }
            }
            else
            {
                return RedirectToAction("Details", new { id = id });
            }
        }



        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
        public string BodyOrderSuccessMail(Order order)
        {
            string body = string.Empty;
            string unit = "";

            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Views\\Shared\\Mail", "OrderSuccessMail.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            foreach (var item in listCarts)
            {
                unit += "<tr>";
                unit += "<td width = '75 % ' align='left' style='font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px; '>";
                unit += item.UnitName + " (" + item.Quantity + ") </td>";
                unit += "<td width = '25 % ' align='left' style='font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px; '>";
                unit += string.Format("{0:C}", item.UnitPrice) + "</td>";
                unit += "</tr>";
            }
            body = body.Replace("{{CreatedAt}}", order.CreatedAt.ToString());
            body = body.Replace("{{FullName}}", order.FullName);
            body = body.Replace("{{Phone}}", order.ContactNumber);
            body = body.Replace("{{Address}}", order.Address);
            body = body.Replace("{{OrderId}}", order.OrderId);
            body = body.Replace("{{Unit}}", unit);
            body = body.Replace("{{ToTalPrice}}", string.Format("{0:C}", order.Price));
            return body;
        }
        public string BodyCancelOrderMail(Order order)
        {
            string body = string.Empty;
            string unit = "";

            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Views\\Shared\\Mail", "CancelOrder.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            List<OrderItem> listOrderItems = _context.OrderItems.Include(i => i.Order).Include(i => i.Unit).Where(o => o.OrderId == order.OrderId).ToList();
            foreach (var item in listOrderItems)
            {
                unit += "<tr>";
                unit += "<td width = '75 % ' align='left' style='font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px; '>";
                unit += item.Unit.UnitName + " (" + item.Quantity + ") </td>";
                unit += "<td width = '25 % ' align='left' style='font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding: 15px 10px 5px 10px; '>";
                unit += string.Format("{0:C}", item.PriceUnit) + "</td>";
                unit += "</tr>";
            }
            body = body.Replace("{{CreatedAt}}", order.CreatedAt?.ToShortDateString());
            body = body.Replace("{{UpdatedAt}}", order.UpdatedAt?.ToShortDateString());
            body = body.Replace("{{FullName}}", order.FullName);
            body = body.Replace("{{Phone}}", order.ContactNumber);
            body = body.Replace("{{Address}}", order.Address);
            body = body.Replace("{{Unit}}", unit);
            body = body.Replace("{{OrderId}}", order.OrderId);
            body = body.Replace("{{ToTalPrice}}", string.Format("{0:C}", order.Price));
            return body;
        }
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        private static Byte[] ConvertImageToByte(Image img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public IActionResult ViewAsPDF(string id)
        {
            var order = _context.Orders
              .Include(o => o.Coupon)
              .Include(o => o.Payment)
              .Include(o => o.User)
              .FirstOrDefault(m => m.OrderId == id);
            ViewData["OrderDetail"] = _context.OrderItems.Include(o => o.Unit).Where(o => o.OrderId == order.OrderId).ToList();
            Barcode barcode = new Barcode();
            Image img = barcode.Encode(TYPE.CODE128, order.OrderId, Color.Black, Color.White, 650, 80);
            ViewData["Barcode"] = ConvertImageToByte(img);
            return new ViewAsPdf(order, ViewData);
        }
    }
}


