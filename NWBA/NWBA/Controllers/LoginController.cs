using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA.Data;
using NWBA.Models;
using SimpleHashing;

namespace NWBA.Controllers
{
    [Route("/Nwba/SecureLogin")]
    public class LoginController : Controller
    {
        private readonly NwbaContext _context;

        public LoginController(NwbaContext context) => _context = context;

        public IActionResult Login() => View();

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logs the customer out

            foreach (var cookie in HttpContext.Request.Cookies)
            {
                HttpContext.Response.Cookies.Delete(cookie.Key);
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}