using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NWBA.Data;
using SimpleHashing;
using NWBA.Web.Helper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NWBA.Models
{
    public enum LoginStatus
    {
        OPEN = 1,
        LOCKED = 2,
        TEMP_LOCKED = 3,
    }

    public class Login
    {
        public static int INACTIVE_TIMEOUT = 60;

        [Range(10000000, 99999999, ErrorMessage = "Login ID must be 8 digits long")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string LoginID { get; set; }

        [Range(1000, 9999, ErrorMessage = "Customer ID must be 4 digits long")]
        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        public LoginStatus Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LockedUntil { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        [DataType(DataType.Date)]
        public DateTime ModifyTime { get; set; }

        //Attempts to log the user in
        public async Task<bool> LoginUser(NwbaContext _context, string _password, string loginID, Controller controller)
        {
            bool successfull = false;
            if (Status == LoginStatus.OPEN || (Status == LoginStatus.TEMP_LOCKED && LockedUntil <= DateTime.UtcNow))
            {
                bool correctPassword = PBKDF2.Verify(PasswordHash, _password);

                //Creates a new login attempt and adds that to the database via the API
                LoginAttempt loginAttempt = new LoginAttempt
                {
                    CustomerID = _context.Logins.Where(x => x.LoginID == loginID).FirstOrDefault().CustomerID,
                    LoginTime = DateTime.Now,
                    Successfull = correctPassword,
                };

                var content = new StringContent(JsonConvert.SerializeObject(loginAttempt), Encoding.UTF8, "application/json");
                await NwbaApi.InitializeClient().PostAsync("api/loginattempts", content);

                //If the login details are correct. log the user in.
                if (correctPassword)
                {
                    // Logs in customer.
                    controller.Response.Cookies.Append("LoggedIn", "", new CookieOptions()
                    {
                        Expires = DateTime.MaxValue
                    });
                    controller.HttpContext.Session.SetInt32("LoggedIn", 1);
                    controller.HttpContext.Session.SetInt32(nameof(CustomerID), CustomerID);
                    controller.HttpContext.Session.SetString(nameof(Customer.Name), Customer.Name);


                    //If the user was previously temporary locked, set to open.
                    if (Status == LoginStatus.TEMP_LOCKED)
                    {
                        Status = LoginStatus.OPEN;
                    }

                    successfull = true;
                }
                else
                {
                    controller.ModelState.AddModelError("Login", "Login Failed");
                }
            }

            return successfull;
        }

        //Locks this user if 3 incorrect login attempts has been made
        public void UpdateLoginAttempts(NwbaContext _context, bool successfull)
        {
            List<LoginAttempt> loginAttempts = _context.LoginAttempts.OrderBy(x => x.LoginTime).ToList();

            //check the last 3 login attempts. if all are failed, log user for X seconds
            bool shouldLock = true;
            for (int i = Math.Max(0, loginAttempts.Count() - 3); i < loginAttempts.Count(); ++i)
            {
                if (loginAttempts[i].Successfull == true)
                {
                    shouldLock = false;
                }
            }

            if (shouldLock && Status != LoginStatus.TEMP_LOCKED)
            {
                LockedUntil = DateTime.UtcNow.AddSeconds(INACTIVE_TIMEOUT);
                Status = LoginStatus.TEMP_LOCKED;
            }


        }

    }
}
