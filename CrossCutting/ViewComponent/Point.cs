using LibraryComponent;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CrossCutting
{
    public class Point : ComponentVisual<Point>, IComponentVisual<Point>
    {
        public void Create(Ponto4D point)
        {
            Create((point.X.ToInt(), point.Y.ToInt()));
        }

        public void Create((int, int) point)
        {
            if (UseColor)
                GL.Color3(Color);
            else
                GL.Color3(Red, Green, Blue);

            var contexto = PointContext.NewInstance();
            contexto.Begin(Size);
            GL.Vertex2(point.Item1, point.Item2);
            contexto.End();
        }

        public Point WithColor(Color color)
        {
            this.SetColor(color);

            return this;
        }

        public Point WithColor(double red, double green, double blue)
        {
            this.SetColor(red, green, blue);

            return this;
        }

        public Point WithSize(float size)
        {
            this.SetSize(size);

            return this;
        }
    }
}
