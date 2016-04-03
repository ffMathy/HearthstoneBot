using System.Drawing;
using System.Drawing.Imaging;

namespace BotApplication.Helpers.Interfaces
{
    public interface IImageConverter
    {
        Bitmap ConvertToFormat(Image image, PixelFormat format);
    }
}