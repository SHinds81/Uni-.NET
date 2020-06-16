using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWBA_Admin.Models;

namespace NWBA_Admin.Attributes
{
    //Authorizes a customer, checks if the logged in flag is set
    public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var customerID = context.HttpContext.Session.GetInt32("LoggedIn");
            if (!customerID.HasValue)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
           
        }
    }
}
