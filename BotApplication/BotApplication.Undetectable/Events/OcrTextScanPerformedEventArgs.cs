using System.Drawing;

namespace BotApplication.Events
{
    public class OcrTextScanPerformedEventArgs
    {
        public Bitmap ImageUsed { get; set; } 

        public Rectangle Region { get; set; }

        public string Text { get; set; }
    }
}