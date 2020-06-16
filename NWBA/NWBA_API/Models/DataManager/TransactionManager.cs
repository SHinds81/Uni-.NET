using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWBA_API.Models.Repository;
using NWBA_API.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NWBA_API.Models.DataManager
{
    public class TransactionManager : IDataRepository<Transaction, int>
    {
        private readonly NwbaContext _context;

        public TransactionManager(NwbaContext context)
        {
            _context = context;
        }

        public Transaction Get(int id)
        {
            return _context.Transactions.Find(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        public int Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction.AccountNumber;
        }

        public int Delete(int id)
        {
            _context.Transactions.Remove(_context.Transactions.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
