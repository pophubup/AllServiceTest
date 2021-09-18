using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isRock;
using System.IO;
using isRock.LineBot;
using Microsoft.Extensions.Configuration;
using zLineBotRepository;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace haha.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LineBotApiController : ControllerBase
    {
        private readonly IConfiguration _Configuration;
        private ILineBot _LineBot;
        public LineBotApiController(IConfiguration Configuration, ILineBot LineBot)
        {
            _Configuration = Configuration;
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
            Bot bot = _LineBot.Reply(body);
            // 剖析JSON
            //var receivedMessage = isRock.LineBot.Utility.Parsing(body);
            // 取得使用者資訊
            //var lineReceuvedEvent = receivedMessage.events.FirstOrDefault();

            //isRock.LineBot.Bot bot = new isRock.LineBot.Bot(_Configuration["LineBot:accessToken"]);

            //var userId = lineReceuvedEvent.source.userId;
            //var replyToken = lineReceuvedEvent.replyToken;

            //string receivedMessageType = lineReceuvedEvent.type;
            ////bot.ReplyMessage(replyToken, $"receivedMessageType: {receivedMessageType}; replyToken:{replyToken}; userId:{userId};");

            //// 訊息
            //// 傳入的對話型態
            //var conversationMode = lineReceuvedEvent.message.type.Trim().ToLower();
            //isRock.LineBot.TextMessage msg;
            //string responseMsg = string.Empty;
            //// 文字訊息
            //switch (lineReceuvedEvent.message.text)
            //{
            //    case "測試1":
            //        responseMsg = "哈哈1";
            //        bot.ReplyMessage(replyToken, responseMsg);
            //        break;
            //    case "測試2":
            //        responseMsg = "哈哈2";
            //        bot.ReplyMessage(replyToken, responseMsg);
            //        break;
            //    case "測試3":
            //        responseMsg = "哈哈3";
            //        bot.ReplyMessage(replyToken, responseMsg);
            //        break;
            //    case "測試4":
            //        responseMsg = "哈哈4";
            //        bot.ReplyMessage(replyToken, responseMsg);
            //        break;
            //    case "測試5":
            //        responseMsg = "哈哈5";
            //        bot.ReplyMessage(replyToken, responseMsg);
            //        break;
            //    case "測試6":
            //        responseMsg = "哈哈6";
            //        bot.ReplyMessage(replyToken, responseMsg);
            //        break;
            //    default:
            //        responseMsg = "請由選單點選功能進行操作。";
            //        // 回覆用戶
            //        bot.ReplyMessage(replyToken, responseMsg);
            //        break;
            //}


            return Ok();
        }
            


    }
}
