using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWBA_API.Models.Repository;
using NWBA_API.Data;

namespace NWBA_API.Models.DataManager
{
    public class LoginManager : IDataRepository<Login, int>
    {
        private readonly NwbaContext _context;

        public LoginManager(NwbaContext context)
        {
            _context = context;
        }

        public Login Get(int id)
        {
            return _context.Logins.Find(id);
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Logins.ToList();
        }

        public int Add(Login login)
        {
            _context.Logins.Add(login);
            _context.SaveChanges();

            return login.CustomerID;
        }

        public int Delete(int id)
        {
            _context.Logins.Remove(_context.Logins.Find(id));
            _context.SaveChanges();

            return id;
        }

        public int Update(int id, Login login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
