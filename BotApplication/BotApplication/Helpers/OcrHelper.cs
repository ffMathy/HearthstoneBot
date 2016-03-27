using System;
using System.Drawing;
using BotApplication.Helpers.Interfaces;
using Tesseract;

namespace BotApplication.Helpers
{
    public class OcrHelper: IOcrHelper
    {
        private readonly TesseractEngine _engine;

        public OcrHelper(TesseractEngine engine)
        {
            _engine = engine;
        }

        public string GetTextInRegion(Bitmap image, Rect region)
        {
            using (var page = _engine.Process(image, region, PageSegMode.Auto))
            {
                return page.GetText();
            }
        }
    }
}