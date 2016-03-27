using System.Collections.Generic;
using BotApplication.Strategies;

namespace BotApplication.Cards.Interfaces
{
    public interface ICard
    {
        string Name { get; }
        string ImageUrl { get; }

        long Id { get; }

        IPlayStrategy PlayStrategy { get; }
    }
}