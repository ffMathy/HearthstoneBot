using System.Collections.Generic;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;

namespace BotApplication.State.Interfaces
{
    public interface IPlayer
    {
        IReadOnlyList<ICard> CardsPlayed { get; }

        void AddCardPlayed(ICard card);
    }
}