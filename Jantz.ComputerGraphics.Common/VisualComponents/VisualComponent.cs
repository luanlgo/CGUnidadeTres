using System.Drawing;

namespace Jantz.ComputerGraphics.Common
{
    public abstract class VisualComponent<T> where T : new()
    {
        protected bool UseColor { get; set; }
        public Color Color { get; protected set; }
        public float Size { get; protected set; }
        public double Red { get; protected set; }
        public double Green { get; protected set; }
        public double Blue { get; protected set; }

        public static T NewInstance()
        {
            return new T();
        }

        protected virtual VisualComponent<T> SetColor(Color color)
        {
            Color = color;
            UseColor = true;
            return this;
        }

        protected virtual VisualComponent<T> SetColor(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            UseColor = false;
            return this;
        }

        protected virtual VisualComponent<T> SetSize(float size)
        {
            Size = size;
            return this;
        }
    }
}
