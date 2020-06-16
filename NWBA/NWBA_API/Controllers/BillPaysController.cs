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
    public class BillPaysController : ControllerBase
    {
        private readonly BillPayManager _repo;

        public BillPaysController(BillPayManager repo)
        {
            _repo = repo;
        }

        // GET: api/BillPay
        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        // GET api/BillPay
        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        // POST api/BillPay
        [HttpPost]
        public void Post([FromBody] BillPay billPay)
        {
            _repo.Add(billPay);
        }

        // PUT api/BillPay
        [HttpPut]
        public void Put([FromBody] BillPay billPay)
        {
            _repo.Update(billPay.BillPayID, billPay);
        }

        // DELETE api/BillPay
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
