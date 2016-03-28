using System;
using System.Drawing;
using BotApplication.Events;
using BotApplication.Helpers.Interfaces;
using HtmlAgilityPack;
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

        public OcrResult GetTextInRegion(Bitmap image, Rect region)
        {
            return GetTextInRegion(image, region, PageSegMode.AutoOsd);
        }

        public OcrResult GetTextInRegion(Bitmap image, Rect region,
            PageSegMode pageSegmentationMode)
        {
            lock (_engine)
            {
                using (var page = _engine.Process(image, region, pageSegmentationMode))
                {
                    if (page.GetMeanConfidence() < 0.5) return default(OcrResult);

                    var text = page.GetText()?.Trim();
                    if (text == string.Empty) return default(OcrResult);

                    var hOcr = page.GetHOCRText(0);
                    var detectedArea = GetAreaFromHocr(hOcr);
                    
                    return new OcrResult()
                    {
                        Area = detectedArea,
                        Text = text
                    };
                }
            }
        }

        private static Rectangle GetAreaFromHocr(string hOcr)
        {
            var document = new HtmlDocument();
            document.LoadHtml(hOcr);

            var element = document.DocumentNode.SelectSingleNode(".//*[@class='ocr_carea']");
            var title = element.Attributes["title"].Value;

            var data = title.Split(' ');
            return new Rectangle(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]));
        }

        public OcrResult GetText(Bitmap image)
        {
            return GetText(image, PageSegMode.AutoOsd);
        }

        public OcrResult GetText(Bitmap image,
            PageSegMode pageSegmentationMode)
        {
            return GetTextInRegion(image, new Rect(0, 0, image.Width, image.Height));
        }
    }
}