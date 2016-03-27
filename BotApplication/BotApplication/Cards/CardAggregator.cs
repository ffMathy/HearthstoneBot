using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using Newtonsoft.Json;

namespace BotApplication.Cards
{
    class CardAggregator: ICardAggregator
    {
        public async Task<IReadOnlyList<ICard>> LoadCardsAsync()
        {
            var json = File.ReadAllText(
                @"node_modules\hearthstone-db\cards\all-cards.json");
            var cardInformation = await Task.Factory.StartNew(() => 
                JsonConvert.DeserializeObject<CardListInformation>(json));
            return cardInformation.Cards;
        }
    }
}
