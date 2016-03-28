﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.State.Interfaces;

namespace BotApplication.State
{
    class EnemyPlayer: IEnemyPlayer
    {
        private readonly IList<ICard> _cardsPlayed;

        public IReadOnlyList<ICard> CardsPlayed => _cardsPlayed.ToArray();

        public EnemyPlayer()
        {
            _cardsPlayed = new List<ICard>();
        }

        public async Task AddCardPlayedAsync(ICard card)
        {
            _cardsPlayed.Add(card);
            Console.WriteLine(card.Name + " played by the enemy.");
            await Task.Delay(2000);
        }
    }
}
