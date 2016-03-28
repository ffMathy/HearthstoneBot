using System.Collections.Generic;
using BotApplication.Cards.Interfaces;
using BotApplication.Strategies;
using BotApplication.Strategies.Interfaces;
using Newtonsoft.Json;

namespace BotApplication.Cards
{
    internal class Card: ICard
    {
        public Card(
            string name, 
            string imageUrl, 
            long id, 
            bool collectible, 
            int? attack, 
            int? health)
        {
            Name = name;
            ImageUrl = imageUrl;
            Id = id;
            Collectible = collectible;
            Attack = attack;
            Health = health;
        }

        public string Name { get; }

        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; }

        public bool Collectible { get; }

        public int? Attack { get; }
        public int? Health { get; }

        public long Id { get; }

        public IReadOnlyCollection<IPlayStrategy> PlayStrategies { get; set; }
    }
}
