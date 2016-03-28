using System.Drawing;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.State.Interfaces;

namespace BotApplication.Interceptors
{
    public class LocalCardDrawnInterceptor: IOcrInterceptor
    {
        private readonly ILocalPlayer _localPlayer;
        private readonly ICardScanner _cardScanner;
        private readonly IGameState _gameState;

        public LocalCardDrawnInterceptor(
            ILocalPlayer localPlayer,
            ICardScanner cardScanner,
            IGameState gameState)
        {
            _localPlayer = localPlayer;
            _cardScanner = cardScanner;
            _gameState = gameState;
        }

        public async Task OnImageReadyAsync(Bitmap image)
        {
            if (_gameState.IsGameStarted)
            {
                var card =
                    await _cardScanner.InferPlayedCardFromImageCardLocationAsync(image, new Point(1540, 599));
                if (card != null)
                {
                    await _localPlayer.AddCardToHandAsync(card);
                }
            }
        }
    }
}