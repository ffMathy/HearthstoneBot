using System.Collections.Generic;
using System.Linq;
using BotApplication.Strategies.Interfaces;
using BotApplication.Strategies.State.Interfaces;

namespace BotApplication.Strategies.State
{
    public class LocalPlayer: ILocalPlayer
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

        public void AddCardPlayed(ICard card)
        {
            _cardsInHand.Remove(card);
            _cardsPlayed.Add(card);
            _logger.LogGameEvent(card.Name + " played by the bot.");
        }

        public void AddCardToHand(ICard card)
        {
            _cardsInHand.Add(card);
            _logger.LogGameEvent(card.Name + " added to the bot's hand.");
        }

        public void ClearCardsInHand()
        {
            _cardsInHand.Clear();
        }
    }
}
