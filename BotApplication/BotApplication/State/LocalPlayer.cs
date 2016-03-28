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
    class LocalPlayer: ILocalPlayer
    {
        private readonly ILogger _logger;
        private readonly IList<ICard> _cardsPlayed;
        private readonly IList<ICard> _cardsInHand;

        public IReadOnlyList<ICard> CardsPlayed => _cardsPlayed.ToArray();
        public IReadOnlyList<ICard> CardsInHand => _cardsInHand.ToArray();

        public LocalPlayer(ILogger logger)
        {
            _logger = logger;
            _cardsPlayed = new List<ICard>();
            _cardsInHand = new List<ICard>();
        }

        public async Task AddCardPlayedAsync(ICard card)
        {
            _cardsInHand.Remove(card);
            _cardsPlayed.Add(card);
            _logger.LogGameEvent(card.Name + " played by the bot.");
            await Task.Delay(2000);
        }

        public async Task AddCardToHandAsync(ICard card)
        {
            _cardsInHand.Add(card);
            _logger.LogGameEvent(card.Name + " added to the bot's hand.");
            await Task.Delay(2000);
        }
    }
}
