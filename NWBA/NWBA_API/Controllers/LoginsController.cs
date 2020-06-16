using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NWBA_API.Models.DataManager;
using NWBA_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace NWBA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly LoginManager _repo;

        public LoginsController(LoginManager repo)
        {
            _repo = repo;
        }

        // GET: api/login
        [HttpGet]
        public IEnumerable<Login> Get()
        {
            return _repo.GetAll();
        }

        // GET api/login
        [HttpGet("{id}")]
        public Login Get(int id)
        {
            return _repo.Get(id);
        }

        // POST api/login
        [HttpPost]
        public void Post([FromBody] Login login)
        {
            _repo.Add(login);
        }

        // PUT api/login
        [HttpPut]
        public void Put([FromBody] Login login)
        {
            _repo.Update(int.Parse(login.LoginID), login);
        }

        // DELETE api/login
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
