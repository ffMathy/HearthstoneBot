using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging.Filters;
using BotApplication.Interceptors.Interfaces;
using Parallel = System.Threading.Tasks.Parallel;
using Point = System.Drawing.Point;

namespace BotApplication.Interceptors
{
    public class AggregateInterceptor : IAggregateInterceptor
    {
        private readonly IEnumerable<IStandardInterceptor> _standardInterceptors;
        private readonly IEnumerable<IOcrInterceptor> _ocrInterceptors;

        public AggregateInterceptor(
            IEnumerable<IStandardInterceptor> standardInterceptors,
            IEnumerable<IOcrInterceptor> ocrInterceptors)
        {
            _standardInterceptors = standardInterceptors;
            _ocrInterceptors = ocrInterceptors;
        }

        private async Task PostFrame(Bitmap image)
        {
            var standardInterceptorsTask = NotifyStandardInterceptorsAsync(image);

            const int minimum = 200;
            var filter = new ColorFiltering(
                new IntRange(minimum, 255),
                new IntRange(minimum, 255),
                new IntRange(minimum, 255));
            var ocrImage = filter.Apply(image);
            
            await standardInterceptorsTask;
            await NotifyOcrInterceptorsAsync(ocrImage);
        }

        private async Task NotifyOcrInterceptorsAsync(Bitmap image)
        {
            await Task.WhenAll(_ocrInterceptors.Select(x => x.OnImageReadyAsync(image)));
        }

        private async Task NotifyStandardInterceptorsAsync(Bitmap image)
        {
            await Task.WhenAll(_standardInterceptors.Select(x => x.OnImageReadyAsync(image)));
        }

        public async Task StartAsync()
        {
            while (true)
            {
                try
                {
                    var bounds = Screen.GetBounds(Point.Empty);
                    using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                        }

                        await PostFrame(bitmap);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}