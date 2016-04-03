using System.Drawing;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.Strategies.State.Interfaces;

namespace BotApplication.Interceptors
{
    public class LocalCardDrawnInterceptor: IInterceptor
    {
        private readonly ILocalPlayer _localPlayer;
        private readonly ICardImageScanner _cardImageScanner;
        private readonly IGameState _gameState;

        public LocalCardDrawnInterceptor(
            ILocalPlayer localPlayer,
            ICardImageScanner cardImageScanner,
            IGameState gameState)
        {
            _localPlayer = localPlayer;
            _cardImageScanner = cardImageScanner;
            _gameState = gameState;
        }

        public async Task OnImageReadyAsync(Bitmap standardImage)
        {
            if (_gameState.IsGameStarted)
            {
                var card =
                    await _cardImageScanner.InferPlayedCardFromImageCardLocationAsync(standardImage, new Point(1540, 456));
                if (card.Match != null)
                {
                    _localPlayer.AddCardToHand(card.Match);
                    await Task.Delay(2000);
                }
            }
        }
    }
}