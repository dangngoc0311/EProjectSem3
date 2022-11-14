using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace LaundryOnline.Controllers
{
    public class ContactController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public ContactController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }
        public IActionResult Index()
        {
            ViewBag.Config = _context.Config.Where(c => c.Status == 1).FirstOrDefault();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactModel contact)
        {
            ViewBag.Config = _context.Config.Where(c => c.Status == 1).FirstOrDefault();
            if (ModelState.IsValid)
            {
                EmailModel model = new EmailModel()
                {
                    Subject = contact.Subject,
                    To = contact.EmailAddress
                };
                using (MailMessage mm = new MailMessage(model.From, model.To))
                {
                    mm.Subject = model.Subject;
                    mm.Body = BodySendContactMail(contact);
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
                        _toastNotification.AddSuccessToastMessage("Send contact message successfully");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(contact);
        }
        public string BodySendContactMail(ContactModel model)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Views\\Shared\\Mail", "ContactMail.cshtml")))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }
    }
}