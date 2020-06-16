using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWBA.Attributes;

namespace NWBA.Controllers
{
    [AuthorizeCustomer]
    public class StatusCodeController : Controller
    {
        //Displays the corresponding error view.
        [HttpGet("/StatusCode/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            switch(statusCode)
            {
                case 404:
                    return RedirectToAction(nameof(Error404));

                case 500:
                    return RedirectToAction(nameof(Error500));

                case 204:
                    return RedirectToAction(nameof(Error204));

                default:
                    return View(statusCode);
            }
        }

        public IActionResult Error404()
        {
            return View();
        }

        public IActionResult Error500()
        {
            return View();
        }

        public IActionResult Error204()
        {
            return View();
        }
    }
}