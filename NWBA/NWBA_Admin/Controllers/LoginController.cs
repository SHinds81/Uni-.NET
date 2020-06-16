using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using Newtonsoft.Json;

namespace NWBA_Admin.Controllers
{
    [Route("/Nwba/SecureLogin")]
    public class LoginController : Controller
    {
        public IActionResult Login() => View();

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logs the customer out
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


    }
}