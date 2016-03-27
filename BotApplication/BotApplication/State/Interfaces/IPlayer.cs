using System.Collections.Generic;
using BotApplication.Cards.Interfaces;

namespace BotApplication.State.Interfaces
{
    public interface IPlayer
    {
        IReadOnlyList<ICard> CardsPlayed { get; }

        void AddCardPlayed(ICard card);
    }
}