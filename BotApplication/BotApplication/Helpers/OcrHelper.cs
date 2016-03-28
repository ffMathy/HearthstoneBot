using System;
using System.Drawing;
using BotApplication.Events;
using BotApplication.Helpers.Interfaces;
using Tesseract;

namespace BotApplication.Helpers
{
    public class OcrHelper : IOcrHelper
    {
        private readonly TesseractEngine _engine;

        public OcrHelper(TesseractEngine engine)
        {
            _engine = engine;
        }

        public string GetTextInRegion(Bitmap image, Rect region)
        {
            lock (_engine)
            {
                using (var page = _engine.Process(image, region, PageSegMode.Auto))
                {
                    var text = page.GetText()?.Trim();
                    if (text == string.Empty) text = null;

                    //if (text != null)
                    {
                        OcrTextScanPerformed?.Invoke(this, new OcrTextScanPerformedEventArgs()
                        {
                            ImageUsed = image,
                            Region =
                                new Rectangle(region.X1, region.Y1, Math.Abs(region.X2 - region.X1),
                                    Math.Abs(region.Y2 - region.Y1)),
                            Text = text
                        });
                    }

                    return text;
                }
            }
        }

        public event EventHandler<OcrTextScanPerformedEventArgs> OcrTextScanPerformed;
    }
}