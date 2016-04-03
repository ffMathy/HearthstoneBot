using System.Drawing;
using System.Windows.Forms;
using BotApplication.Interaction.Interfaces;

namespace BotApplication.Interaction
{
    class MouseInteractor: IMouseInteractor
    {
        public Point CurrentLocation => Cursor.Position;

        public Point MoveMouseHumanly(Point targetPoint)
        {
            Cursor.Position = targetPoint;
            return targetPoint;
        }
    }
}
