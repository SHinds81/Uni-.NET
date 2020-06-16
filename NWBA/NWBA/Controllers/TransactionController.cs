using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA.Data;
using NWBA.Models;
using NWBA.Utilities;
using NWBA.Attributes;
using NWBA.ViewModels;

namespace NWBA.Controllers
{

    [AuthorizeCustomer]
    public class TransactionController : Controller
    {
        private readonly NwbaContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public TransactionController(NwbaContext context) => _context = context;

        public async Task<IActionResult> CreateTransaction()
        {
            //Finds the customer and redirects them to the correct view
            var customer = await _context.Customers.FindAsync(CustomerID);
            var transactionViewModel = new TransactionViewModel(customer);
            return View(transactionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionViewModel trans)
        {
            //gets the data from post and validates the input
            //if valid. add the transaction to the database.
            ModelState.Clear();

            Account account = await _context.Accounts.FindAsync(trans.Transaction.AccountNumber);
            Account destinationAccount = await _context.Accounts.FindAsync(trans.Transaction.DestinationAccountNumber);

            Dictionary<string, string> errors = new Dictionary<string, string>();
            TransactionDto.IsValid(_context, trans.Transaction, out errors);

            //shows the user the errors if any
            if (errors.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in errors)
                {
                    ModelState.AddModelError(entry.Key, entry.Value);
                }
            }

            if (!ModelState.IsValid)
            {
                return await CreateTransaction();
            }

            //creates a new transaction based on validated input
            TransactionDto transaction = new TransactionDto()
            {
                Type = trans.Transaction.Type,
                TransactionTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow,
                Amount = trans.Transaction.Amount,
                Comment = trans.Transaction.Comment,
                AccountNumber = trans.Transaction.AccountNumber,
                DestinationAccountNumber = trans.Transaction.DestinationAccountNumber,
            };

            //Add to relevant accounts
            if (transaction.Type == TransactionType.Transfer)
            {
                await account.AddTransaction(transaction);
                await destinationAccount.AddTransaction(transaction);
            }
            else
            {
                await account.AddTransaction(transaction);
            }

            //saves the database
            await _context.SaveChangesAsync();
              
            return RedirectToAction("ViewAccounts", "Customer");
        }
    }
}