using System.Collections.Generic;

namespace BotApplication.Strategies.Interfaces
{
    public interface ICard
    {
        string Name { get; }

        //bool Collectible { get; }

        int? Attack { get; }
        int? Health { get; }

        long Id { get; }

        IReadOnlyCollection<IPlayStrategy> PlayStrategies { get; }
    }
}