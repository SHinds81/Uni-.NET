using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NWBA.Data;
using NWBA.Models;
using Microsoft.AspNetCore.Http;
using SimpleHashing;
using System.Net.Http;
using System.Text;
using NWBA.Web.Helper;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace NWBA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NwbaContext _context;

        public HomeController(ILogger<HomeController> logger, NwbaContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Main page is the view accounts page.
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("LoggedIn") == 1)
            {
                return RedirectToAction("ViewAccounts", "Customer");
            }
            else
            {
                return View();
            }

        }

        public IActionResult CatchAll()
        {
            Response.StatusCode = 404;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? id)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        //logs the user in
        public async Task<IActionResult> Login(string loginID, string password)
        {
            //gets their login model
            var login = await _context.Logins.FindAsync(loginID);
            //checks if valid

            if (login != null)
            {
                //Logs the user in
                bool successfull = await login.LoginUser(_context, password, loginID, this);
                //Updates the login attempts
                login.UpdateLoginAttempts(_context, successfull);
            }
            
            _context.SaveChanges();


            //redirects to the main page
            return RedirectToAction(nameof(Index));
        }
    }
}
