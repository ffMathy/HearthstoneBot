using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotApplication.Helpers.Interfaces;
using BotApplication.State.Interfaces;

namespace BotApplication.State
{
    class GameState: IGameState
    {
        private readonly ILogger _logger;

        public GameState(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsGameStarted { get; private set; }

        public void StartGame()
        {
            IsGameStarted = true;
            _logger.LogGameEvent("Game has been started.");
        }
    }
}
