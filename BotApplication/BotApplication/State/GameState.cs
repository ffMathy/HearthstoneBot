using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotApplication.State.Interfaces;

namespace BotApplication.State
{
    class GameState: IGameState
    {
        public bool IsGameStarted { get; private set; }

        public void StartGame()
        {
            IsGameStarted = true;
            Console.WriteLine("Game has been started.");
        }
    }
}
