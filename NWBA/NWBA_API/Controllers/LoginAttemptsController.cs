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
    public class LoginAttemptsController : ControllerBase
    {
        private readonly LoginAttemptManager _repo;

        public LoginAttemptsController(LoginAttemptManager repo)
        {
            _repo = repo;
        }

        // GET: api/loginattempts
        [HttpGet]
        public IEnumerable<LoginAttempt> Get()
        {
            return _repo.GetAll();
        }

        // GET api/loginattempts
        [HttpGet("{id}")]
        public LoginAttempt Get(int id)
        {
            return _repo.Get(id);
        }

        // POST api/loginattempts
        [HttpPost]
        public void Post([FromBody] LoginAttempt loginAttempt)
        {
            _repo.Add(loginAttempt);
        }

        // PUT api/loginattempts
        [HttpPut]
        public void Put([FromBody] LoginAttempt loginAttempt)
        {
            _repo.Update(loginAttempt.LoginAttemptID, loginAttempt);
        }

        // DELETE api/loginattempts
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
