using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Strategies.Interfaces;
using Newtonsoft.Json;

namespace BotApplication.Cards
{
    internal class CardAggregator: ICardAggregator
    {
        private readonly IEnumerable<IPlayStrategy> _playStrategies;

        private IReadOnlyList<ICard> _cards; 

        public CardAggregator(
            IEnumerable<IPlayStrategy> playStrategies)
        {
            _playStrategies = playStrategies;
        }

        public async Task<IReadOnlyList<ICard>> LoadCardsAsync()
        {
            if (_cards != null) return _cards;

            var json = File.ReadAllText(
                @"node_modules\hearthstone-db\cards\all-cards.json");
            var cardInformation = await Task.Factory.StartNew(() => 
                JsonConvert.DeserializeObject<CardListInformation>(json));

            foreach (var card in cardInformation.Cards)
            {
                card.PlayStrategies = _playStrategies
                    .Where(x => x.FitsCard(card))
                    .ToArray();
            }

            return _cards = cardInformation.Cards;
        }

        public async Task<ICard> GetCardByNameAsync(string name)
        {
            if (_cards == null)
            {
                await LoadCardsAsync();
            }

            return _cards.SingleOrDefault(x => x.Name == name);
        }
    }
}
