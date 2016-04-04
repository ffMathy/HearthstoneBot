using System.Collections.Generic;
using System.Linq;
using BotApplication.Strategies.Interfaces;
using BotApplication.Strategies.State.Interfaces;

namespace BotApplication.Strategies.State
{
    public class EnemyPlayer: IEnemyPlayer
    {
        private readonly ILogger _logger;
        private readonly IList<ICard> _cardsPlayed;

        public IReadOnlyList<ICard> CardsPlayed => _cardsPlayed.ToArray();

        public EnemyPlayer(
            ILogger logger)
        {
            _logger = logger;
            _cardsPlayed = new List<ICard>();
        }

        public void AddCardPlayed(ICard card)
        {
            _cardsPlayed.Add(card);
            _logger.LogGameEvent(card.Name + " played by the enemy.");
        }
    }
}
