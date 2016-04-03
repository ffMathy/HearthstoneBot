using System.Drawing;
using Tesseract;

namespace BotApplication.Helpers.Interfaces
{
    public interface IOcrHelper
    {
        OcrResult GetTextInRegion(
            Bitmap image,
            Rect region);

        OcrResult GetTextInRegion(
            Bitmap image,
            Rect region,
            PageSegMode pageSegmentationMode);

        OcrResult GetText(
            Bitmap image);

        OcrResult GetText(
            Bitmap image,
            PageSegMode pageSegmentationMode);
    }
}