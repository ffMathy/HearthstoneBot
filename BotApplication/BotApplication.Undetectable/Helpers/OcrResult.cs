using System.Drawing;

namespace BotApplication.Helpers
{
    public struct OcrResult
    {
        public string Text { get; set; } 
        public Rectangle Area { get; set; }
    }
}