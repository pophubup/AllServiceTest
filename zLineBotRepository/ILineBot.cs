using isRock.LineBot;
namespace zLineBotRepository
{
    public interface ILineBot
    {
        Bot Reply(string bodyContent);
    }
}
