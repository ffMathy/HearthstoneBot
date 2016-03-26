using System.Collections.Generic;
using BotApplication.Cards.Interfaces;

namespace BotApplication.Players.Interfaces
{
    public interface ILocalPlayer: IPlayer
    {
        IReadOnlyList<ICard> CardsInHand { get; }
    }
}