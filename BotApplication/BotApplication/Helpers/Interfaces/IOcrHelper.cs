using System.Drawing;
using AForge.Imaging;
using Tesseract;

namespace BotApplication.Helpers.Interfaces
{
    public interface IOcrHelper
    {
        string GetTextInRegion(
            Bitmap image,
            Rect region);
    }
}