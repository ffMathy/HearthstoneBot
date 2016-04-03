using System.Collections.Generic;
using BotApplication.Strategies.Interfaces;

namespace BotApplication.Strategies.State.Interfaces
{
    public interface IPlayer
    {
        IReadOnlyList<ICard> CardsPlayed { get; }

        void AddCardPlayed(ICard card);
    }
}