using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using BotApplication.State.Interfaces;

namespace BotApplication.State
{
    class EnemyPlayer: IEnemyPlayer
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
