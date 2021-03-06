using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zModelLayer.ViewModels;
using zWebCrawlingRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace haha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebCrawlingController : ControllerBase
    {
        private IDATOWebCrawling _webCrawling;
        public WebCrawlingController(IDATOWebCrawling webCrawling)
        {
            _webCrawling = webCrawling;
        }
       /// <summary>
       /// 撈取英雄聯盟頁面資訊
       /// </summary>
       /// <param name="name">遊戲者名稱</param>
       /// <param name="endpage">最後頁面</param>
       /// <returns></returns>
        [HttpGet]
        public  IEnumerable<EveryPage> Get([FromQuery] string name = "c.y", [FromQuery] int endpage = 1)
        {
            return  _webCrawling.GetDataFromWebElement(name, endpage);
        }

       
    }
}
