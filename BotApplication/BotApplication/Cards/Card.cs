using BotApplication.Cards.Interfaces;
using BotApplication.Strategies;
using Newtonsoft.Json;

namespace BotApplication.Cards
{
    class Card: ICard
    {
        public Card(
            string name, 
            string imageUrl, 
            long id)
        {
            Name = name;
            ImageUrl = imageUrl;
            Id = id;
        }

        public string Name { get; }

        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; }

        public long Id { get; }

        public IPlayStrategy PlayStrategy { get; set; }
    }
}
