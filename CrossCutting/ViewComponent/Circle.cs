using LibraryComponent;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace CrossCutting
{
    public class Circle : ComponentVisual<Circle>, IComponentVisual<Circle>
    {
        private const int DEGRES = 360;

        public void Create(double radius, int amountPoints)
        {
            Create(radius, amountPoints, (0, 0));
        }

        public void Create(double radius, int amountPoints, (double, double) center)
        {
            if (amountPoints < 1)
                throw new ArgumentException("A quantidade de pontos não deve ser menor ou igual a zero.", nameof(amountPoints));

            int pause = DEGRES / amountPoints;

            if (UseColor)
                GL.Color3(Color);
            else
                GL.Color3(Red, Green, Blue);

            var contexto = CircleContext.NewInstance();
            contexto.Begin(Size);

            for (double i = 0; i < DEGRES; i += pause)
                CreatePoint(i, radius, center);

            contexto.End();
        }

        public void CreateRadiusLine(double angle, double radius, (int, int) point)
        {
            var contexto = LineSegmentContext.NewInstance();
            contexto.Begin(Size);

            var a = GraphicMath.GerarPontosCirculo(angle, radius, point.Item1, point.Item2);
            LineSegment.NewInstance()
                       .WithColor(Color)
                       .Create((point.Item1, point.Item2), (a.X.ToInt(), a.Y.ToInt()));

            contexto.End();
        }

        private void CreatePoint(double ponta, double radius, (double, double) center)
        {
            var a = GraphicMath.GerarPontosCirculo(ponta, radius, center.Item1, center.Item2);
            GL.Vertex2(a.X, a.Y);
        }

        public static bool isInside(double radius, int amountPoints, Ponto4D check, int quem)
        {
            if (amountPoints < 1)
                throw new ArgumentException("A quantidade de pontos não deve ser menor ou igual a zero.", nameof(amountPoints));

            int pause = DEGRES / amountPoints;

            bool pego = false;

            for (double i = 0; i < DEGRES; i += pause)
            {
                if (quem == 0 && ((i >= 0 && i <= 45) || (i >= 315 && i <= 360)))
                {
                    var a = GraphicMath.GerarPontosCirculo(i, radius, 0, 0);

                    if (check.X < a.X && ((check.Y >= a.Y && check.Y <= 0) || (check.Y <= a.Y && check.Y >= 0)))
                    {
                        pego = true;
                    }
                }

                if (quem == 1 && (i >= 135 && i <= 220))
                {
                    var a = GraphicMath.GerarPontosCirculo(i, radius, 0, 0);

                    if (check.X > a.X && ((check.Y >= a.Y && check.Y <= 0) || (check.Y <= a.Y && check.Y >= 0)))
                    {
                        pego = true;
                    }
                }

                if (quem == 2 && (i >= 220 && i <= 315))
                {
                    var a = GraphicMath.GerarPontosCirculo(i, radius, 0, 0);

                    if (check.Y > a.Y && ((check.X >= a.X && check.X <= 0) || (check.X <= a.X && check.X >= 0)))
                    {
                        pego = true;
                    }
                }

                if (quem == 3 && (i >= 45 && i <= 135))
                {
                    var a = GraphicMath.GerarPontosCirculo(i, radius, 0, 0);

                    if (check.Y < a.Y && ((check.X >= a.X && check.X <= 0) || (check.X <= a.X && check.X >= 0)))
                    {
                        pego = true;
                    }
                }

            }

            return !pego;
        }

        public Circle WithColor(Color color)
        {
            this.SetColor(color);

            return this;
        }

        public Circle WithColor(double red, double green, double blue)
        {
            this.SetColor(red, green, blue);

            return this;
        }

        public Circle WithSize(float size)
        {
            this.SetSize(size);

            return this;
        }
    }
}
