using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWBA_API.Models.Repository;
using NWBA_API.Data;

namespace NWBA_API.Models.DataManager
{
    public class LoginAttemptManager : IDataRepository<LoginAttempt, int>
    {
        private readonly NwbaContext _context;

        public LoginAttemptManager(NwbaContext context)
        {
            _context = context;
        }

        public LoginAttempt Get(int id)
        {
            return _context.LoginAttempts.Find(id);
        }

        public IEnumerable<LoginAttempt> GetAll()
        {
            return _context.LoginAttempts.ToList();
        }

        public int Add(LoginAttempt loginAttempt)
        {
            _context.LoginAttempts.Add(loginAttempt);
            _context.SaveChanges();

            return loginAttempt.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Logins.Remove(_context.Logins.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, LoginAttempt loginAttempt)
        {
            _context.Update(loginAttempt);
            _context.SaveChanges();

            return id;
        }
    }
}
