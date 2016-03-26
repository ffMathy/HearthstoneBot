using System.Collections.Generic;
using BotApplication.Strategies;

namespace BotApplication.Cards.Interfaces
{
    public interface ICard<in TCard> 
        where TCard: ICard<TCard>
    {
        string Name { get; }
        string ImageUrl { get; }

        long Id { get; }

        IPlayStrategy<TCard> PlayStrategy { get; }
    }
}