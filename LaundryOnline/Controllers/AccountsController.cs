using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace LaundryOnline.Controllers
{
    public class AccountsController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public AccountsController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            var user = _context.Users.Where(x => x.UserId == HttpContext.Session.GetString("CustomerId")).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                if (AccountEmailExists(forgotPasswordModel.EmailAddress))
                {
                    var acc = _context.Users.FirstOrDefault(m => m.EmailAddress == forgotPasswordModel.EmailAddress);
                    var pass = CreateRandomPassword(8);
                    acc.Password = CreateMD5(pass);
                    _context.Update(acc);
                    await _context.SaveChangesAsync();
                    EmailModel model = new EmailModel()
                    {
                        Subject = "Reset Password",
                        To = forgotPasswordModel.EmailAddress
                    };
                    using (MailMessage mm = new MailMessage(model.From, model.To))
                    {
                        mm.Subject = model.Subject;
                        mm.Body = BodyResetPasswordMail(pass);
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
                    _toastNotification.AddSuccessToastMessage("Check your mail");
                    return RedirectToAction("Login", "Accounts");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Account not exists");
                    return View(forgotPasswordModel);
                }
            }
            return View(forgotPasswordModel);
        }

        public ActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model,string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var checkAccount = _context.Users.FirstOrDefault(u => u.EmailAddress == model.EmailAddress);
                if (checkAccount != null)
                {
                    if (checkAccount.Status == 1)
                    {
                        if (checkAccount.Password == CreateMD5(model.Password))
                        {
                            HttpContext.Session.SetString("CustomerLogin", checkAccount.UserName);
                            HttpContext.Session.SetString("CustomerId", checkAccount.UserId);
                            _toastNotification.AddSuccessToastMessage("Login successfully");
                            if (Url.IsLocalUrl(returnUrl))
                                return Redirect(returnUrl);
                            else
                                return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            _toastNotification.AddErrorToastMessage("Wrong password");
                            return View(model);
                        }
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Account has been locked");
                        return View(model);
                    }
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Account not exists");
                    return View(model);
                }
            }
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(User user)
        {
            user.Role = 1;
            user.Status = 1;
            if (ModelState.IsValid)
            {
                if (AccountEmailExists(user.EmailAddress))
                {
                    _toastNotification.AddErrorToastMessage("Email alreadys exist");
                    return View(user);
                }
                else if (AccountUserNameExists(user.UserName))
                {
                    _toastNotification.AddErrorToastMessage("Username alreadys exist");
                    return View(user);
                }
                else
                {
                    user.Password = CreateMD5(user.Password);
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    EmailModel model = new EmailModel()
                    {
                        Subject = "Register ",
                        To = user.EmailAddress
                    };
                    using (MailMessage mm = new MailMessage(model.From, model.To))
                    {
                        mm.Subject = model.Subject;
                        mm.Body = BodyRegisterMail();
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
                            _toastNotification.AddSuccessToastMessage("Register account successfully");
                        }
                    }
                    return RedirectToAction("Login", "Accounts");
                }
            }
            return View(user);
        }

        // GET: Accounts/Edit/5
        public ActionResult ChangePassword(int id)
        {
            return View();
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePassword change)
        {
            var user = _context.Users.Find(change.UserId);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    user.Password = CreateMD5(change.Password);
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Change password account successfully");
                    return RedirectToAction("Details", "Accounts");
                }
                else
                {
                    return View(change);
                }
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CustomerLogin");
            HttpContext.Session.Remove("CustomerId");
            HttpContext.Session.Remove("Coupon");

            return RedirectToAction("Login", "Accounts");
        }

        public string BodyRegisterMail()
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Views\\Shared\\Mail", "RegisterSuccessMail.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
        public string BodyResetPasswordMail(string pass)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Views\\Shared\\Mail", "ForgotPasswordMail.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{Password}}", pass);
            return body;
        }
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        private bool AccountEmailExists(string email)
        {
            return _context.Users.Any(e => e.EmailAddress == email);
        }
        private bool AccountUserNameExists(string username)
        {
            return _context.Users.Any(e => e.UserName == username);
        }
    }
}