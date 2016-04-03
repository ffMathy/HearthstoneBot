using System.Drawing;
using System.Threading.Tasks;

namespace BotApplication.Cards.Interfaces
{
    public interface ICardImageScanner
    {
        Task<CardImageScanResult> InferPlayedCardFromImageCardLocationAsync(Bitmap image, Point location);
        Task<CardImageScanResult> InferEarlyGameCardFromImageCardLocationAsync(Bitmap image, Point location);
        Task<CardImageScanResult> InferHoveredCardFromImageCardLocationAsync(Bitmap image, Point location);
    }
}