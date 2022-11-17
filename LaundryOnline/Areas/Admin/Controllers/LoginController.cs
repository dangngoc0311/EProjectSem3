using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LaundryOnline.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace LaundryOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly LaundryOnlineContext _context;
        private readonly IToastNotification _toastNotification;

        public LoginController(LaundryOnlineContext context, IToastNotification toastrNotification)
        {
            _context = context;
            _toastNotification = toastrNotification;
        }

        public IActionResult Index(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel model)
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
                    if (checkAccount.Role == 0)
                    {
                        if (checkAccount.Password == CreateMD5(model.Password))
                        {
                            var identity = new ClaimsIdentity(new[]
                     {
                        new Claim(ClaimTypes.Name, checkAccount.EmailAddress),
                        new Claim(ClaimTypes.NameIdentifier, checkAccount.UserId),
                        new Claim(ClaimTypes.Role,"admin")
                    }, "BkapSecurityScheme");
                            var principal = new ClaimsPrincipal(identity);
                            HttpContext.SignInAsync("BkapSecuritySchema", principal);
                            _toastNotification.AddSuccessToastMessage("Login successfully");
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
                        _toastNotification.AddErrorToastMessage("Not have a game to your account");
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

        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync("BkapSecuritySchema");
            //_toastNotification.AddSuccessToastMessage("Đăng xuất thành công");

            return RedirectToAction("Index", "Home");
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

    }
}