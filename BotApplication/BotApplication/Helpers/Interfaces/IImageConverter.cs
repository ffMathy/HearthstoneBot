using System.Drawing;
using System.Drawing.Imaging;

namespace BotApplication.Cards.Interfaces
{
    public interface IImageConverter
    {
        Bitmap ConvertToFormat(Image image, PixelFormat format);
    }
}