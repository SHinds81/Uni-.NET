using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWBA_Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWBA_Admin.ViewModels
{
    public class BillPayViewModel
    {
        public BillPayDto BillPay { get; set; }

        public List<SelectListItem> PeriodList
        {
            get
            {
                List<SelectListItem> types = new List<SelectListItem>();
                var values = Enum.GetValues(typeof(Period));

                foreach (var value in values)
                {
                    switch (value)
                    {
                        case Period.Single:
                            types.Add(new SelectListItem($"Single", value.ToString()));
                            break;

                        case Period.Monthly:
                            types.Add(new SelectListItem($"Monthly", value.ToString()));
                            break;

                        case Period.Yearly:
                            types.Add(new SelectListItem($"Yearly", value.ToString()));
                            break;

                        case Period.Quarterly:
                            types.Add(new SelectListItem($"Quarterly", value.ToString()));
                            break;
                    }
                }

                return types;
            }
            set
            {

            }
        }

        public List<SelectListItem> StatusList
        {
            get
            {
                List<SelectListItem> types = new List<SelectListItem>();
                var values = Enum.GetValues(typeof(Status));

                foreach (var value in values)
                {
                    switch (value)
                    {
                        case Status.BLOCKED:
                            types.Add(new SelectListItem($"Blocked", value.ToString()));
                            break;

                        case Status.CANCELLED:
                            types.Add(new SelectListItem($"Cancelled", value.ToString()));
                            break;

                        case Status.SCHEDULED:
                            types.Add(new SelectListItem($"Scheduled", value.ToString()));
                            break;

                        case Status.PAYED:
                            types.Add(new SelectListItem($"Payed", value.ToString()));
                            break;

                        case Status.FAILED:
                            types.Add(new SelectListItem($"Failed", value.ToString()));
                            break;
                    }
                }

                return types;
            }
            set
            {

            }
        }

        public BillPayViewModel() { }

        public BillPayViewModel(BillPayDto billPay)
        {
            BillPay = billPay;
        }

    }

}
