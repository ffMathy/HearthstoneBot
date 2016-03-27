using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotApplication.Cards.Interfaces
{
    public interface ICardAggregator
    {
        Task<IReadOnlyList<ICard>> LoadCardsAsync();
    }
}