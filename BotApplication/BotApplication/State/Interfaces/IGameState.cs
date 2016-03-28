namespace BotApplication.State.Interfaces
{
    public interface IGameState
    {
        bool IsGameStarted { get; }

        void StartGame();
    }
}