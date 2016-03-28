

using System.Drawing;
using System.Threading.Tasks;

namespace BotApplication.Interceptors.Interfaces
{
    public interface IInterceptor
    {
        Task OnImageReadyAsync(
            Bitmap standardImage);
    }
}