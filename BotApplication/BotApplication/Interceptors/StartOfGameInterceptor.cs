using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.State.Interfaces;
using Tesseract;

namespace BotApplication.Interceptors
{
    class StartOfGameInterceptor : IInterceptor
    {
        private readonly ILocalPlayer _player;
        private readonly ICardScanner _cardScanner;
        private readonly IGameState _gameState;

        public StartOfGameInterceptor(
            ILocalPlayer player,
            ICardScanner cardScanner,
            IGameState gameState)
        {
            _player = player;
            _cardScanner = cardScanner;
            _gameState = gameState;
        }

        private async Task StartGameAndAddStartingCardsAsync(params ICard[] cards)
        {
            Console.WriteLine("Adding starting hand.");
            foreach (var card in cards)
            {
                await _player.AddCardToHandAsync(card);
            }
            _gameState.StartGame();
        }

        public async Task OnImageReadyAsync(Bitmap standardImage)
        {
            if (!_gameState.IsGameStarted)
            {
                var fourStructureCard1 = await _cardScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(440, 485));
                var fourStructureCard2 = await _cardScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(769, 485));
                var fourStructureCard3 = await _cardScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(1105, 485));
                var fourStructureCard4 = await _cardScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(1439, 485));

                var isFourStructure = fourStructureCard1 != null && fourStructureCard2 != null && fourStructureCard3 != null && fourStructureCard4 != null;
                if (isFourStructure)
                {
                    await StartGameAndAddStartingCardsAsync(
                        fourStructureCard1,
                        fourStructureCard2,
                        fourStructureCard3,
                        fourStructureCard4);
                }
                else {
                    var threeStructureCard1 = await _cardScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(531, 485));
                    var threeStructureCard2 = await _cardScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(956, 485));
                    var threeStructureCard3 = await _cardScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(1382, 485));

                    var isThreeStructure = threeStructureCard1 != null && threeStructureCard2 != null &&
                                           threeStructureCard3 != null;
                    if (isThreeStructure)
                    {
                        await StartGameAndAddStartingCardsAsync(
                            threeStructureCard1,
                            threeStructureCard2,
                            threeStructureCard3);
                    } else if (Debugger.IsAttached)
                    {
                        //_gameState.StartGame();
                    }
                }
            }
        }
    }
}
