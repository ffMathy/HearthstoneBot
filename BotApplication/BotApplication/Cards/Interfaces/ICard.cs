using System.Collections.Generic;
using BotApplication.Strategies;
using BotApplication.Strategies.Interfaces;

namespace BotApplication.Cards.Interfaces
{
    public interface ICard
    {
        string Name { get; }
        string ImageUrl { get; }

        long Id { get; }

        IReadOnlyCollection<IPlayStrategy> PlayStrategies { get; }
    }
}