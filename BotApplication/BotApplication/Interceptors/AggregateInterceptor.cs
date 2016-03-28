using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public AggregateInterceptor(
            IEnumerable<IStandardInterceptor> standardInterceptors,
            IEnumerable<IOcrInterceptor> ocrInterceptors)
        {
            _standardInterceptors = standardInterceptors;
            _ocrInterceptors = ocrInterceptors;
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private async Task PostFrame(Bitmap image)
        {
            var standardInterceptorsTask = NotifyStandardInterceptorsAsync(image);

            const int minimum = 200;
            var filter = new ColorFiltering(
                new IntRange(minimum, 255),
                new IntRange(minimum, 255),
                new IntRange(250, 255));
            var ocrImage = filter.Apply(image);

            await standardInterceptorsTask;
            await NotifyOcrInterceptorsAsync(ocrImage);
        }

        private async Task NotifyOcrInterceptorsAsync(Bitmap image)
        {
            foreach (var interceptor in _ocrInterceptors)
            {
                await interceptor.OnImageReadyAsync(image);
            }
        }

        private async Task NotifyStandardInterceptorsAsync(Bitmap image)
        {
            foreach (var interceptor in _standardInterceptors)
            {
                await interceptor.OnImageReadyAsync(image);
            }
        }

        public async Task StartAsync()
        {
            while (true)
            {
                await Task.Delay(1000);
                try
                {
                    if (GetActiveWindowTitle() == "Hearthstone")
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}