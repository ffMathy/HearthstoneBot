namespace BotApplication.Helpers.Interfaces
{
    public interface ILogger
    {
        void LogGameEvent(string text);
        void LogDebugEvent(string text);
    }
}