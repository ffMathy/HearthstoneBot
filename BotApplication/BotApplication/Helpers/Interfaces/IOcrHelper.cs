using System;
using System.Drawing;
using BotApplication.Events;
using Tesseract;

namespace BotApplication.Helpers.Interfaces
{
    public interface IOcrHelper
    {
        string GetTextInRegion(
            Bitmap image,
            Rect region);

        string GetTextInRegion(
            Bitmap image,
            Rect region,
            PageSegMode pageSegmentationMode);

        string GetText(
            Bitmap image);

        string GetText(
            Bitmap image,
            PageSegMode pageSegmentationMode);

        event EventHandler<OcrTextScanPerformedEventArgs> OcrTextScanPerformed;
    }
}