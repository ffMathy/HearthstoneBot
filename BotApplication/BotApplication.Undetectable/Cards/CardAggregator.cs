using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using BotApplication.Strategies;
using BotApplication.Strategies.Interfaces;
using Newtonsoft.Json;

namespace BotApplication.Cards
{
    internal class CardAggregator: ICardAggregator
    {
        private readonly IEnumerable<IPlayStrategy> _playStrategies;
        private readonly ILogger _logger;

        private IReadOnlyList<ICard> _cards; 

        public CardAggregator(
            IEnumerable<IPlayStrategy> playStrategies,
            ILogger logger)
        {
            _playStrategies = playStrategies;
            _logger = logger;
        }

        public async Task<IReadOnlyList<ICard>> LoadCardsAsync()
        {
            if (_cards != null) return _cards;

            var finalCards = new List<Card>();
            foreach (var file in Directory.GetFiles(@"node_modules\hearthstone-db\cards", "*.json"))
            {
                var cardInformation = await LoadCardInformation(file);
                foreach (var card in cardInformation.Cards)
                {
                    if (finalCards.All(x => x.Name != card.Name))
                    {
                        finalCards.Add(card);
                    }
                }
            }

            foreach (var card in finalCards)
            {
                card.PlayStrategies = _playStrategies
                    .Where(x => x.FitsCard(card))
                    .ToArray();
            }

            return _cards = finalCards;
        }

        private static async Task<CardListInformation> LoadCardInformation(string file)
        {
            var json = File.ReadAllText(file);
            var cardInformation = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<CardListInformation>(json));
            return cardInformation;
        }

        public async Task<IEnumerable<ICard>> GetCardCandidatesOrderedByRelevancyFromNameAsync(string name)
        {
            if (_cards == null)
            {
                await LoadCardsAsync();
            }

            name = SanitizeName(name);

            var orderedCards = _cards
                .OrderBy(x => DamerauLevenshteinDistance(name, x.Name))
                .ToArray();
            var cards = orderedCards
                .Where(x =>
                {
                    var distance = DamerauLevenshteinDistance(name, x.Name);
                    return (x.Name.Length >= 5 && distance < 5) || (x.Name.Length <= 5 && distance < 2);
                })
                .ToArray();
            if (cards.Length == 0)
            {
                var bestFailedCandidate = orderedCards.First();
                _logger.LogDebugEvent("Best failed candidate for \"" + name + "\" was " + bestFailedCandidate.Name +
                                  " with a distance of " + DamerauLevenshteinDistance(name, bestFailedCandidate.Name));
            }
            else
            {
                _logger.LogDebugEvent("Top 3 matches for \"" + name + "\" was:");
                foreach (var card in orderedCards.Take(3))
                {
                    _logger.LogDebugEvent("\t" + card.Name);
                }
            }

            return cards;
        }

        private static string SanitizeName(string name)
        {
            return name?.Trim().Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }


        public static int DamerauLevenshteinDistance(string string1, string string2)
        {
            string1 = SanitizeName(string1);
            string2 = SanitizeName(string2);

            if (string.IsNullOrEmpty(string1))
            {
                return !string.IsNullOrEmpty(string2) ? string2.Length : 0;
            }

            if (string.IsNullOrEmpty(string2))
            {
                return !string.IsNullOrEmpty(string1) ? string1.Length : 0;
            }

            var length1 = string1.Length;
            var length2 = string2.Length;

            var d = new int[length1 + 1, length2 + 1];

            for (var i = 0; i <= d.GetUpperBound(0); i++)
                d[i, 0] = i;

            for (var i = 0; i <= d.GetUpperBound(1); i++)
                d[0, i] = i;

            for (var i = 1; i <= d.GetUpperBound(0); i++)
            {
                for (var j = 1; j <= d.GetUpperBound(1); j++)
                {
                    var cost = string1[i - 1] == string2[j - 1] ? 0 : 1;

                    var del = d[i - 1, j] + 1;
                    var ins = d[i, j - 1] + 1;
                    var sub = d[i - 1, j - 1] + cost;

                    d[i, j] = Math.Min(del, Math.Min(ins, sub));

                    if (i > 1 && j > 1 && string1[i - 1] == string2[j - 2] && string1[i - 2] == string2[j - 1])
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }
}
