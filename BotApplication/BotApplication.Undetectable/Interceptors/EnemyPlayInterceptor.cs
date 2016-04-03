using System.Drawing;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.Strategies.State.Interfaces;

namespace BotApplication.Interceptors
{
    public class EnemyPlayInterceptor: IInterceptor
    {
        private readonly IEnemyPlayer _enemyPlayer;
        private readonly ICardImageScanner _cardImageScanner;
        private readonly IGameState _gameState;

        public EnemyPlayInterceptor(
            IEnemyPlayer enemyPlayer,
            ICardImageScanner cardImageScanner,
            IGameState gameState)
        {
            _enemyPlayer = enemyPlayer;
            _cardImageScanner = cardImageScanner;
            _gameState = gameState;
        }

        public async Task OnImageReadyAsync(Bitmap standardImage)
        {
            if (_gameState.IsGameStarted)
            {
                var card =
                    await _cardImageScanner.InferPlayedCardFromImageCardLocationAsync(standardImage, new Point(359, 347));
                if (card.Match != null)
                {
                    _enemyPlayer.AddCardPlayed(card.Match);
                    await Task.Delay(2000);
                }
            }
        }
    }
}