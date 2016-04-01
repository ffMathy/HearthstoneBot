using System.Collections.Generic;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;

namespace BotApplication.State.Interfaces
{
    public interface ILocalPlayer: IPlayer
    {
        IReadOnlyList<ICard> CardsInHand { get; }

        void AddCardToHand(ICard card);
        void ClearCardsInHand();
    }
}