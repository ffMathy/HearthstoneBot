using System;
using BotApplication.Strategies.Interfaces;
using BotApplication.Strategies.State.Interfaces;

namespace BotApplication.Strategies.State
{
    public class GameState: IGameState
    {
        private readonly ILogger _logger;

        public event EventHandler TurnChanged;

        public bool IsGameStarted { get; private set; }

        public Turn CurrentTurn { get; private set; }

        public GameState(ILogger logger)
        {
            _logger = logger;
        }

        public void StartGame()
        {
            IsGameStarted = true;
            _logger.LogGameEvent("Game has been started.");
        }

        public void SwitchTurns(Turn turn)
        {
            if (turn == CurrentTurn) return;

            _logger.LogGameEvent("It is now the " + (turn == Turn.Local ? "bot's" : "enemy's") + " turn.");
            CurrentTurn = turn;

            RaiseTurnChangedEvent();
        }

        private void RaiseTurnChangedEvent()
        {
            TurnChanged?.Invoke(this, new EventArgs());
        }
    }
}
