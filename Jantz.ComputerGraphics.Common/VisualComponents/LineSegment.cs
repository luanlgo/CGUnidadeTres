using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Jantz.ComputerGraphics.Common
{
    public class LineSegment : VisualComponent<LineSegment>, IVisualComponent<LineSegment>
    {
        public void Create((double, double) pointOne, (double, double) pointTwo)
        {
            Create((pointOne.Item1.ToInt(), pointOne.Item2.ToInt()), (pointTwo.Item1.ToInt(), pointTwo.Item2.ToInt()));
        }

        /// <summary>
        /// Cria uma linha, não possui Begin e End.
        /// </summary>
        /// <param name="pointOne"></param>
        /// <param name="pointTwo"></param>
        public void Create((int, int) pointOne, (int, int) pointTwo)
        {
            if (UseColor)
                GL.Color3(Color);
            else
                GL.Color3(Red, Green, Blue);
            GL.LineWidth(Size);
            GL.Vertex2(pointOne.Item1, pointOne.Item2);
            GL.Vertex2(pointTwo.Item1, pointTwo.Item2);
        }

        public LineSegment WithColor(Color color)
        {
            this.SetColor(color);

            return this;
        }

        public LineSegment WithColor(double red, double green, double blue)
        {
            this.SetColor(red, green, blue);

            return this;
        }

        public LineSegment WithSize(float size)
        {
            this.SetSize(size);

            return this;
        }
    }
}
