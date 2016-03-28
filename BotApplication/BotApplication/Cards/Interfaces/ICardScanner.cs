using System.Drawing;
using System.Threading.Tasks;

namespace BotApplication.Cards.Interfaces
{
    public interface ICardScanner
    {
        Task<ICard> InferPlayedCardFromImageCardLocationAsync(Bitmap image, Point location);
        Task<ICard> InferEarlyGameCardFromImageCardLocationAsync(Bitmap image, Point location);
    }
}