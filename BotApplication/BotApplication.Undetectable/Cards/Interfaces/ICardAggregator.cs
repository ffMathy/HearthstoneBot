using System.Collections.Generic;
using System.Threading.Tasks;
using BotApplication.Strategies.Interfaces;

namespace BotApplication.Cards.Interfaces
{
    public interface ICardAggregator
    {
        Task<IReadOnlyList<ICard>> LoadCardsAsync();

        Task<IEnumerable<ICard>> GetCardCandidatesOrderedByRelevancyFromNameAsync(string name);
    }
}