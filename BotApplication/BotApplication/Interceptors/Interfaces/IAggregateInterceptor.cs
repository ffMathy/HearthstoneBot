using System.Threading.Tasks;

namespace BotApplication.Interceptors.Interfaces
{
    public interface IAggregateInterceptor
    {
        Task StartAsync();
    }
}