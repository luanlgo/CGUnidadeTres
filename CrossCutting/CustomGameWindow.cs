using LibraryComponent;
using OpenTK;

namespace CrossCutting
{
    public class CustomGameWindow : GameWindow
    {
        public Camera Camera { get; private set; }

        public CustomGameWindow(int width, int height) : base(width, height)
        {
            Camera = new Camera();
        }

        public CustomGameWindow WithTitle(string title)
        {
            this.Title = title;

            return this;
        }
    }
}
