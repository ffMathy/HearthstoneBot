﻿using System.Drawing;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.State.Interfaces;
using Tesseract;

namespace BotApplication.Interceptors
{
    public class EnemyPlayInterceptor: IOcrInterceptor
    {
        private readonly IEnemyPlayer _enemyPlayer;
        private readonly ICardScanner _cardScanner;
        private readonly IGameState _gameState;

        public EnemyPlayInterceptor(
            IEnemyPlayer enemyPlayer,
            ICardScanner cardScanner,
            IGameState gameState)
        {
            _enemyPlayer = enemyPlayer;
            _cardScanner = cardScanner;
            _gameState = gameState;
        }

        public async Task OnImageReadyAsync(Bitmap image)
        {
            if (_gameState.IsGameStarted)
            {
                var card =
                    await _cardScanner.InferPlayedCardFromImageCardLocationAsync(image, new Point(299, 355));
                if (card != null)
                {
                    await _enemyPlayer.AddCardPlayedAsync(card);
                }
            }
        }
    }
}