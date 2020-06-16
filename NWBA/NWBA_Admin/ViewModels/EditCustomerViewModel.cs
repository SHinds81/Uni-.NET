using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWBA_Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NWBA_Admin
{
    public class EditCustomerViewModel
    {
        public CustomerDto Customer { get; set; }
        public LoginStatus LoginStatus { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LockedUntil { get; set; }

        public EditCustomerViewModel() { }
        
        public EditCustomerViewModel(CustomerDto customer, LoginStatus loginStatus)
        {
            Customer = customer;
            LoginStatus = loginStatus;
        }

        public List<SelectListItem> LoginStatusTypes
        {
            get
            {
                List<SelectListItem> types = new List<SelectListItem>();
                var values = Enum.GetValues(typeof(LoginStatus));

                foreach (var value in values)
                {
                    switch (value)
                    {
                        case LoginStatus.LOCKED:
                            types.Add(new SelectListItem($"Locked", value.ToString()));
                            break;

                        case LoginStatus.OPEN:
                            types.Add(new SelectListItem($"Open", value.ToString()));
                            break;

                        case LoginStatus.TEMP_LOCKED:
                            types.Add(new SelectListItem($"Temporary Locked", value.ToString()));
                            break;

                    }
                }

                return types;
            }
            set
            {

            }
        }

    }
}
