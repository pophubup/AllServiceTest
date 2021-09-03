using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace haha.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        public TestController()
        {

        }
        // GET api/<TestController>/5
        [HttpGet("{name}")]
        public string Getyyyyyyy(string name)
        {
            return "value";
        }

      
    }
}
