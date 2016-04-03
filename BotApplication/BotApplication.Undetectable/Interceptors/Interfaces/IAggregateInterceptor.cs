using System.Drawing;
using System.Threading.Tasks;

namespace BotApplication.Interceptors.Interfaces
{
    public interface IAggregateInterceptor
    {
        Bitmap CurrentImage { get; }

        Task StartAsync();
    }
}