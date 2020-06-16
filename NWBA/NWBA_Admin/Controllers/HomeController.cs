using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NWBA_Admin.Models;

namespace NWBA_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        //logs the user in
        public IActionResult Login(string loginID, string password)
        {
            bool successfull = (loginID == "admin" && password == "admin");

            if (!successfull)
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return RedirectToAction(nameof(Index));
            }

            // Logs in customer.
            HttpContext.Session.SetInt32("LoggedIn", 1);

            //redirects to the main page
            return RedirectToAction(nameof(Index));
        }
    }
}
