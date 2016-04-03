using System;
using System.Diagnostics;
using System.Drawing;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using BotApplication.Helpers.Interfaces;

namespace BotApplication.Helpers
{
    public class ImageFilter: IImageFilter
    {
        public Bitmap CropBitmap(Bitmap input, Rectangle region)
        {
            try
            {
                var target = new Bitmap(region.Width, region.Height);

                using (var graphics = Graphics.FromImage(target))
                {
                    graphics.DrawImage(input, new Rectangle(0, 0, target.Width, target.Height),
                        region,
                        GraphicsUnit.Pixel);
                }

                return target;
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                throw;
            }
        }

        public Bitmap ResizeBitmap(Bitmap input, Size newSize)
        {
            var target = new Bitmap(newSize.Width, newSize.Height);
            using (var graphics = Graphics.FromImage(target))
            {
                graphics.DrawImage(input, new Rectangle(0, 0, target.Width, target.Height),
                                 new Rectangle(0, 0, input.Width, input.Height), 
                                 GraphicsUnit.Pixel);
            }

            return target;
        }

        public Bitmap ExcludeColorsOutsideRange(Bitmap image, Rectangle region, IntRange universalRange, RGB fillColor = null)
        {
            return ExcludeColorsOutsideRange(image, region, universalRange, universalRange, universalRange, fillColor);
        }

        public Bitmap ExcludeColorsOutsideRange(Bitmap image, Rectangle region, IntRange redRange, IntRange greenRange,
            IntRange blueRange, RGB fillColor = null)
        {
            if (fillColor == null) fillColor = new RGB(Color.Black);

            var target = CropBitmap(image, region);

            var filter = new ColorFiltering
            {
                Red = redRange,
                Green = greenRange,
                Blue = blueRange,
                FillColor = fillColor
            };

            filter.ApplyInPlace(target);

            return target;
        }

        public Bitmap IncreaseContrast(Bitmap image, Rectangle region, int factor)
        {
            var target = CropBitmap(image, region);

            var filter = new ContrastCorrection(factor);
            filter.ApplyInPlace(target);

            return target;
        }
        
        public Bitmap RemoveNoise(Bitmap image)
        {
            var filter = new BilateralSmoothing();
            filter.KernelSize = 7;
            filter.SpatialFactor = 10;
            filter.ColorFactor = 60;
            filter.ColorPower = 0.5;

            filter.ApplyInPlace(image);

            return image;
        }

    }
}