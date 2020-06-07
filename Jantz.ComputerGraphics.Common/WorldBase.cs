using Biblioteca;
using OpenTK;

namespace Jantz.ComputerGraphics.Common
{
    public class WorldBase : GameWindow
    {
        public Camera Camera { get; private set; }

        public WorldBase(int width, int height) : base(width, height)
        {
            Camera = new Camera();
        }

        public WorldBase WithTitle(string title)
        {
            this.Title = title;

            return this;
        }
    }
}
