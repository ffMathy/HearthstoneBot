using System.Drawing;
using BotApplication.Strategies.Interfaces;

namespace BotApplication.Cards
{
    public struct CardImageScanResult
    {
        public Rectangle CardPosition { get; set; }
        public ICard Match { get; set; }  
    }
}