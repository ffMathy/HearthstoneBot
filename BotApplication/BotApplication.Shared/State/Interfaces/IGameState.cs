using System;

namespace BotApplication.Strategies.State.Interfaces
{
    public interface IGameState
    {
        bool IsGameStarted { get; }

        Turn CurrentTurn { get; }

        void StartGame();
        void SwitchTurns(Turn local);
    }
}