using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWBA_Admin.Web.Helper;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NWBA_Admin.Models;
using NWBA_Admin.Attributes;

namespace NWBA_Admin.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //returns the edit page of a customer
        public async Task<IActionResult> EditCustomer(int? id)
        {
            var response = await NwbaApi.InitializeClient().GetAsync("api/customers/" + id);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction($"StatusCode/{response.StatusCode}");

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<CustomerDto>(result);

            response = await NwbaApi.InitializeClient().GetAsync("api/logins/");

            result = response.Content.ReadAsStringAsync().Result;
            var logins = JsonConvert.DeserializeObject<List<Login>>(result);

            try
            {
                var login = logins.Where(x => x.CustomerID == customer.CustomerID).FirstOrDefault();

                var customerViewModel = new EditCustomerViewModel(customer, login.Status);
                return View(customerViewModel);
            }
            catch
            {
                return RedirectToAction($"StatusCode/" + 204);
            }
        }

        //Views the details of a customer
        public async Task<IActionResult> ViewCustomer(int? id)
        {
            var response = await NwbaApi.InitializeClient().GetAsync("api/customers/" + id);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction($"StatusCode/{response.StatusCode}");

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<CustomerDto>(result);

            return View(customer);
        }

        //edits the customer
        [HttpPost]
        public async Task<IActionResult> EditCustomer(EditCustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customerViewModel.Customer), Encoding.UTF8, "application/json");
                var response = NwbaApi.InitializeClient().PutAsync("api/customers", content).Result;

                response = await NwbaApi.InitializeClient().GetAsync("api/logins/");

                var result = response.Content.ReadAsStringAsync().Result;
                var logins = JsonConvert.DeserializeObject<List<Login>>(result);

                var login = logins.Where(x => x.CustomerID == customerViewModel.Customer.CustomerID).FirstOrDefault();
                login.Status = customerViewModel.LoginStatus;
                login.LockedUntil = customerViewModel.LockedUntil;

                content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                response = NwbaApi.InitializeClient().PutAsync("api/logins", content).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception();


                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(ViewCustomers));
            }


            return RedirectToAction(nameof(ViewCustomers));
        }

        //Views all the customers
        public async Task<IActionResult> ViewCustomers()
        {
            var response = await NwbaApi.InitializeClient().GetAsync("api/customers/");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(result);
            return View(customers);
        }
    }
}