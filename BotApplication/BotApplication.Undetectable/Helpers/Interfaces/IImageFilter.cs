using System.Drawing;
using AForge;
using AForge.Imaging;

namespace BotApplication.Helpers.Interfaces
{
    public interface IImageFilter
    {
        Bitmap CropBitmap(Bitmap input, Rectangle region);
        Bitmap ResizeBitmap(Bitmap bitmap, Size newSize);

        Bitmap ExcludeColorsOutsideRange(Bitmap image, Rectangle region, IntRange universalRange, RGB fillColor = null);
        Bitmap ExcludeColorsOutsideRange(Bitmap image, Rectangle region, IntRange redRange, IntRange greenRange, IntRange blueRange, RGB fillColor = null);

        Bitmap IncreaseContrast(Bitmap image, Rectangle region, int factor);

        Bitmap RemoveNoise(Bitmap image);
    }
}