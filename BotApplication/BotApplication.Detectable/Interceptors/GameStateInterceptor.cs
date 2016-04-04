using Bot.Interceptors.Interfaces;
using BotApplication.Strategies.State;
using BotApplication.Strategies.State.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Interceptors
{
    class GameStateInterceptor : IInterceptor
    {
        private readonly IGameState gameState;
        private readonly GameState hearthstoneGameState;

        public GameStateInterceptor(
            IGameState gameState)
        {
            this.gameState = gameState;
            this.hearthstoneGameState = GameState.Get();
        }

        private void OnCurrentPlayerChanged(Player player, object userData)
        {
            var isCurrentPlayer = hearthstoneGameState.IsFriendlySidePlayerTurn();
            if(isCurrentPlayer)
            {
                gameState.SwitchTurns(Turn.Local);
            } else
            {
                gameState.SwitchTurns(Turn.Enemy);
            }
        }

        public void BeginInterception()
        {
            hearthstoneGameState.RegisterCurrentPlayerChangedListener(OnCurrentPlayerChanged);
        }
    }
}
