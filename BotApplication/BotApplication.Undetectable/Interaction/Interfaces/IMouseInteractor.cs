
using System.Drawing;

namespace BotApplication.Interaction.Interfaces
{
    public interface IMouseInteractor
    {
        Point CurrentLocation { get; }

        Point MoveMouseHumanly(Point targetPoint);
    }
}