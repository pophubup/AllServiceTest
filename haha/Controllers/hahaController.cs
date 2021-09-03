using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace haha.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class hahaController : ControllerBase
    {
        IConfiguration _Configuration; 
        public hahaController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        // GET api/<hahaController>/5
        [HttpGet("{name}")]
        public string Getyyyy(string name)
        {
            
            return _Configuration[name];
        }
       

    }
}
