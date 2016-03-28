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
        private readonly ICardImageScanner _cardImageScanner;
        private readonly IGameState _gameState;

        public StartOfGameInterceptor(
            ILocalPlayer player,
            ICardImageScanner cardImageScanner,
            IGameState gameState)
        {
            _player = player;
            _cardImageScanner = cardImageScanner;
            _gameState = gameState;
        }

        private void StartGameAndAddStartingCards(params ICard[] cards)
        {
            Console.WriteLine("Adding starting hand.");
            foreach (var card in cards)
            {
                _player.AddCardToHand(card);
            }
            _gameState.StartGame();
        }

        public async Task OnImageReadyAsync(Bitmap standardImage)
        {
            if (!_gameState.IsGameStarted)
            {
                var fourStructureCard1 = await _cardImageScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(479, 485));
                var fourStructureCard2 = await _cardImageScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(795, 485));
                var fourStructureCard3 = await _cardImageScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(1114, 485));
                var fourStructureCard4 = await _cardImageScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(1433, 485));

                var isFourStructure = fourStructureCard1.Match != null && fourStructureCard2.Match != null && fourStructureCard3.Match != null && fourStructureCard4.Match != null;
                if (isFourStructure)
                {
                    StartGameAndAddStartingCards(
                        fourStructureCard1.Match,
                        fourStructureCard2.Match,
                        fourStructureCard3.Match,
                        fourStructureCard4.Match);
                }
                else {
                    var threeStructureCard1 = await _cardImageScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(531, 485));
                    var threeStructureCard2 = await _cardImageScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(956, 485));
                    var threeStructureCard3 = await _cardImageScanner.InferEarlyGameCardFromImageCardLocationAsync(standardImage, new Point(1382, 485));

                    var isThreeStructure = threeStructureCard1.Match != null && threeStructureCard2.Match != null &&
                                           threeStructureCard3.Match != null;
                    if (isThreeStructure)
                    {
                        StartGameAndAddStartingCards(
                            threeStructureCard1.Match,
                            threeStructureCard2.Match,
                            threeStructureCard3.Match);
                    } else if (Debugger.IsAttached)
                    {
                        _gameState.StartGame();
                    }
                }
            }
        }
    }
}
