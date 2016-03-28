using System;
using System.Drawing;
using AForge.Imaging;
using BotApplication.Events;
using Tesseract;

namespace BotApplication.Helpers.Interfaces
{
    public interface IOcrHelper
    {
        string GetTextInRegion(
            Bitmap image,
            Rect region);

        event EventHandler<OcrTextScanPerformedEventArgs> OcrTextScanPerformed;
    }
}