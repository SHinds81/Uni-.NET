using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NWBA_Admin.Models;
using NWBA_Admin.Web.Helper;
using NWBA_Admin.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using NWBA_Admin.Attributes;

namespace NWBA_Admin.Controllers
{
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {

        //gets all the billpays
        public async Task<IActionResult> Index()
        {
            var response = await NwbaApi.InitializeClient().GetAsync("api/billpays/");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var billPays = JsonConvert.DeserializeObject<List<BillPayDto>>(result);

            return View(billPays);
        }

        //Displays the detail of one bill pay
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await NwbaApi.InitializeClient().GetAsync("api/billpays/" + id);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var billPay = JsonConvert.DeserializeObject<BillPayDto>(result);

            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await NwbaApi.InitializeClient().GetAsync("api/billpays/" + id);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var billPay = JsonConvert.DeserializeObject<BillPayDto>(result);

            return View(new BillPayViewModel(billPay));
        }

        //Updates the updated billpay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BillPayViewModel billPayViewModel)
        {
            if (id != billPayViewModel.BillPay.BillPayID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                billPayViewModel.BillPay.ModifyDate = DateTime.UtcNow;
                var content = new StringContent(JsonConvert.SerializeObject(billPayViewModel.BillPay), Encoding.UTF8, "application/json");
                var response = await NwbaApi.InitializeClient().PutAsync("api/billpays", content);

                return RedirectToAction(nameof(Index));
            }
            return View(billPayViewModel);
        }

        // GET: BillPay/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await NwbaApi.InitializeClient().GetAsync("api/billpays/" + id);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var billPay = JsonConvert.DeserializeObject<BillPayDto>(result);

            return View(billPay);
        }

        // POST: BillPay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await NwbaApi.InitializeClient().DeleteAsync("api/billpays/" + id);
            return RedirectToAction("Index");
        }

    }
}
