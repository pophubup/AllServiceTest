using isRock.LineBot;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace zLineBotRepository
{
    public class LazyLineBotService : ILineBot
    {
        private Bot bot;
        private IConfiguration _configuration;
        public LazyLineBotService(IConfiguration configuration)
        {
     
            _configuration = configuration;
            bot = new Bot(_configuration["LineBot:accessToken"]);
        }
        public Bot Reply(string body)
        {
            var receivedMessage = isRock.LineBot.Utility.Parsing(body);
            // 取得使用者資訊
            var lineReceuvedEvent = receivedMessage.events.FirstOrDefault();

            var userId = lineReceuvedEvent.source.userId;
            var replyToken = lineReceuvedEvent.replyToken;

            string receivedMessageType = lineReceuvedEvent.type;
            //bot.ReplyMessage(replyToken, $"receivedMessageType: {receivedMessageType}; replyToken:{replyToken}; userId:{userId};");

            // 訊息
            // 傳入的對話型態
            var conversationMode = lineReceuvedEvent.message.type.Trim().ToLower();
            isRock.LineBot.TextMessage msg;
            string responseMsg = string.Empty;
            // 文字訊息
            switch (lineReceuvedEvent.message.text)
            {
                case "測試1":
                    responseMsg = "哈哈1";
                    bot.ReplyMessage(replyToken, responseMsg);
                    break;
                case "測試2":
                    responseMsg = "哈哈2";
                    bot.ReplyMessage(replyToken, responseMsg);
                    break;
                case "測試3":
                    responseMsg = "哈哈3";
                    bot.ReplyMessage(replyToken, responseMsg);
                    break;
                case "測試4":
                    responseMsg = "哈哈4";
                    bot.ReplyMessage(replyToken, responseMsg);
                    break;
                case "測試5":
                    responseMsg = "哈哈5";
                    bot.ReplyMessage(replyToken, responseMsg);
                    break;
                case "測試6":
                    responseMsg = "哈哈6";
                    bot.ReplyMessage(replyToken, responseMsg);
                    break;
                default:
                    responseMsg = "請由選單點選功能進行操作。";
                    // 回覆用戶
                    bot.ReplyMessage(replyToken, responseMsg);
                    break;

                    
            }
            return bot;
        }
    }
}
