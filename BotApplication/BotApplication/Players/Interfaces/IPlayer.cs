using System.Collections.Generic;
using BotApplication.Cards.Interfaces;

namespace BotApplication.Players.Interfaces
{
    public interface IPlayer
    {
        IReadOnlyList<ICard> CardsPlayed { get; }
    }
}