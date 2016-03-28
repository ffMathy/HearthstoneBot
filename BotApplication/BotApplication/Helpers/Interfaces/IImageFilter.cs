using System.Drawing;
using AForge;

namespace BotApplication.Helpers.Interfaces
{
    public interface IImageFilter
    {
        Bitmap CropBitmap(Bitmap input, Rectangle region);
        Bitmap ResizeBitmap(Bitmap bitmap, Size newSize);

        Bitmap ExcludeColorsOutsideRange(Bitmap image, Rectangle region, IntRange universalRange);
        Bitmap ExcludeColorsOutsideRange(Bitmap image, Rectangle region, IntRange redRange, IntRange greenRange, IntRange blueRange);

        Bitmap IncreaseContrast(Bitmap image, Rectangle region, int factor);

        Bitmap RemoveNoise(Bitmap image);
    }
}