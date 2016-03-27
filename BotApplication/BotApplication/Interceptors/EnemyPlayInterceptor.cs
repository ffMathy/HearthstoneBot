using System.Drawing;
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
        private readonly IOcrHelper _helper;
        private readonly IEnemyPlayer _enemyPlayer;
        private readonly ICardAggregator _cardAggregator;

        public EnemyPlayInterceptor(
            IOcrHelper helper,
            IEnemyPlayer enemyPlayer,
            ICardAggregator cardAggregator)
        {
            _helper = helper;
            _enemyPlayer = enemyPlayer;
            _cardAggregator = cardAggregator;
        }

        public async Task OnImageReadyAsync(Bitmap image)
        {
            var cardName = _helper.GetTextInRegion(image, new Rect(330, 550, 660 - 330, 620 - 550));
            var card = await _cardAggregator.GetCardByNameAsync(cardName);
            if (card != null)
            {
                _enemyPlayer.AddCardPlayed(card);
            }
        }
    }
}