using System.Collections.Generic;
using BotApplication.Strategies.Interfaces;

namespace BotApplication.Strategies
{
    public class Card: ICard
    {
        public Card(
            string name, 
            long id, 
            bool collectible, 
            int? attack, 
            int? health)
        {
            Name = name;
            Id = id;
            Collectible = collectible;
            Attack = attack;
            Health = health;
        }

        public string Name { get; }

        public bool Collectible { get; }

        public int? Attack { get; }
        public int? Health { get; }

        public long Id { get; }

        public IReadOnlyCollection<IPlayStrategy> PlayStrategies { get; set; }
    }
}
