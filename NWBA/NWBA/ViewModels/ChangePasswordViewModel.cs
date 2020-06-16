using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NWBA.Models;

namespace NWBA.ViewModels
{
    public class ChangePasswordViewModel
    {
        public virtual Customer Customer { get; set; }
        public virtual Login Login { get; set; }

        [Display(Name = "New Password")]
        public string NewHash { get; set; }
        [Display(Name = "Repeat Password")]
        public string RepeatNewHash { get; set; }

        public ChangePasswordViewModel() { }

        public ChangePasswordViewModel(Customer customer, Login login)
        {
            Customer = customer;
            Login = login;
        }
    }
}