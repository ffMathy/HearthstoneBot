using System.Drawing;
using System.Threading.Tasks;

namespace BotApplication.Cards.Interfaces
{
    public interface ICardImageService
    {
        Task<Image> GetCardImageAsync(ICard card);
    }
}