using System.Collections.Generic;
using System.Drawing;
using BotApplication.Cards.Interfaces;

namespace BotApplication.Cards
{
    public struct CardImageScanResult
    {
        public Rectangle CardPosition { get; set; }
        public ICard Match { get; set; }  
    }
}