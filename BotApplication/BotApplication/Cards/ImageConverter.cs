using System.Drawing;
using System.Drawing.Imaging;
using BotApplication.Cards.Interfaces;

namespace BotApplication.Cards
{
    class ImageConverter : IImageConverter
    {
        public Bitmap ConvertToFormat(Image image, PixelFormat format)
        {
            var copy = new Bitmap(image.Width, image.Height, format);
            using (var graphics = Graphics.FromImage(copy))
            {
                graphics.DrawImage(image, 
                    new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }
    }
}
