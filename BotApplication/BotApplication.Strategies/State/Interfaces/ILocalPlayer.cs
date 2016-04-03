using System.Collections.Generic;
using BotApplication.Strategies.Interfaces;

namespace BotApplication.Strategies.State.Interfaces
{
    public interface ILocalPlayer: IPlayer
    {
        IReadOnlyList<ICard> CardsInHand { get; }

        void AddCardToHand(ICard card);
        void ClearCardsInHand();
    }
}