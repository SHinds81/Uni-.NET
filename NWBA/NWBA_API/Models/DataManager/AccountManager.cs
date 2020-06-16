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
    public class AccountManager : IDataRepository<Account, int>
    {
        private readonly NwbaContext _context;

        public AccountManager(NwbaContext context)
        {
            _context = context;
        }

        public Account Get(int id)
        {
            return _context.Accounts.Find(id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        public int Add(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.AccountNumber;
        }

        public int Delete(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Account account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }
    }
}
