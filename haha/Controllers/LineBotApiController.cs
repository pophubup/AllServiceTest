﻿using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using zLineBotRepository;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace haha.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LineBotApiController : ControllerBase
    {
        private ILineBot _LineBot;
        public LineBotApiController(ILineBot LineBot)
        {
            _LineBot = LineBot;
        }
        /// <summary>
        /// LineBot Webhook 的 URL 詳細設定請至 https://developers.line.biz/zh-hant/
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> POST()
        {
            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
             _LineBot.Reply(body);
            return Ok();
        }
            


    }
}
