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
        private readonly IEnumerable<IInterceptor> _interceptors;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public AggregateInterceptor(
            IEnumerable<IInterceptor> interceptors)
        {
            _interceptors = interceptors;
        }

        private static string GetActiveWindowTitle()
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
            foreach (var interceptor in _interceptors)
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
                        var bounds = Screen.GetWorkingArea(Point.Empty);
                        var titleBarHeight = 0; //SystemInformation.CaptionHeight;
                        using (var bitmap = new Bitmap(bounds.Width, bounds.Height - titleBarHeight))
                        {
                            using (var graphics = Graphics.FromImage(bitmap))
                            {
                                graphics.CopyFromScreen(new Point(0, titleBarHeight), Point.Empty, bounds.Size);
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