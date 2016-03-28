using System;

namespace BotApplication.State.Interfaces
{
    public interface IGameState
    {
        event EventHandler TurnChanged;

        bool IsGameStarted { get; }

        Turn CurrentTurn { get; }

        void StartGame();
        void SwitchTurns(Turn local);
    }
}