using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NWBA.Models;
using NWBA.Controllers;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NWBA.Web.Helper;
using NWBA.Data;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayer
{
    //Class that schedules transactions with BillPay from the database
    public class BillPayScheduler : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BillPayScheduler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }



        //Method gets a list of all upcoming payments
        //loops through them every 5 seconds
        //Processes them if they are due


        //processes a bill. pays the relevant accounts
        [HttpPost]
        private async Task ProcessBillPay(BillPay billPay)
        {
            var response = await NwbaApi.InitializeClient().GetAsync("api/accounts/");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            var accounts = JsonConvert.DeserializeObject<List<Account>>(result);


            Account account = accounts.Where(x => x.AccountNumber == billPay.AccountNumber).FirstOrDefault();
            Account destinationAccount = accounts.Where(x => x.AccountNumber == billPay.PayeeID).FirstOrDefault();
            TransactionDto transaction = new TransactionDto()
            {
                Type = TransactionType.Transfer,
                AccountNumber = billPay.AccountNumber,
                DestinationAccountNumber = billPay.PayeeID,
                Amount = (decimal)billPay.Amount,
                Comment = "Scheduled Transfer",
                TransactionTime = billPay.ScheduleDate,
            };

            if (transaction.Type == TransactionType.Transfer)
            {
                await account.AddTransaction(transaction);
                await destinationAccount.AddTransaction(transaction);
            }
            else
            {
                await account.AddTransaction(transaction);
            }

            if (billPay.Period == Period.Single)
            {
                billPay.Status = Status.PAYED;
            }
            else
            {
                switch(billPay.Period)
                {
                    case Period.Monthly:
                        billPay.ScheduleDate.AddMonths(1);
                        break;

                    case Period.Quarterly:
                        billPay.ScheduleDate.AddMonths(3);
                        break;

                    case Period.Yearly:
                        billPay.ScheduleDate.AddMonths(12);
                        break;
                }
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<NwbaContext>();

                BillPay bp = db.BillPay.Where(x => x.BillPayID == billPay.BillPayID).FirstOrDefault();
                bp.ScheduleDate = billPay.ScheduleDate;
                bp.Status = billPay.Status;

                db.Transactions.Add(transaction);

                Account ac1 = db.Accounts.Find(account.AccountNumber);
                ac1.Balance = account.Balance;

                Account ac2 = db.Accounts.Find(destinationAccount.AccountNumber);
                ac2.Balance = destinationAccount.Balance;

                await db.SaveChangesAsync();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BackgroundProcessing(stoppingToken);
        }

        protected async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            List<BillPay> bills = new List<BillPay>();
            while (true)
            {
                bills.Clear();

                var response = await NwbaApi.InitializeClient().GetAsync("api/billpays/");

                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                // Storing the response details recieved from web api.
                var result = response.Content.ReadAsStringAsync().Result;

                var billPays = JsonConvert.DeserializeObject<List<BillPay>>(result);

                foreach (BillPay billPay in billPays)
                {
                    if (billPay.Status == Status.SCHEDULED)
                    {
                        bills.Add(billPay);
                    }
                }

                //loops through
                for (int i = bills.Count - 1; i >= 0; i--)
                {
                    BillPay billPay = bills[i];

                    //Switch based on the period
                    switch (billPay.Period)
                    {
                        case Period.Single:
                            //SINGLE PAYMENT
                            if (billPay.ScheduleDate.Date <= DateTime.Now.Date && billPay.ScheduleDate.TimeOfDay.TotalSeconds <= DateTime.Now.TimeOfDay.TotalSeconds)
                            {
                                await ProcessBillPay(billPay);
                            }
                            break;

                        case Period.Monthly:
                            //MONTHLY PAYMENT
                            if (billPay.ScheduleDate.Date <= DateTime.Now.Date && billPay.ScheduleDate.TimeOfDay.TotalSeconds <= DateTime.Now.TimeOfDay.TotalSeconds)
                            {
                                await ProcessBillPay(billPay);
                            }
                            break;

                        case Period.Quarterly:
                            //QUARTERLY PAYMENT
                            if (billPay.ScheduleDate.Date <= DateTime.Now.Date && billPay.ScheduleDate.TimeOfDay.TotalSeconds <= DateTime.Now.TimeOfDay.TotalSeconds)
                            {
                                await ProcessBillPay(billPay);
                            }
                            break;

                        case Period.Yearly:
                            //YEARLY PAYMENT
                            if (billPay.ScheduleDate.Date <= DateTime.Now.Date && billPay.ScheduleDate.TimeOfDay.TotalSeconds <= DateTime.Now.TimeOfDay.TotalSeconds)
                            {
                                await ProcessBillPay(billPay);
                            }
                            break;
                        default:
                            break;
                    }
                }

                //Tick rate -> 1 seconds
                Thread.Sleep(1000);
            }
        }
    }
}