using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CrossCutting
{
    public class Triangle : ComponentVisual<Triangle>, IComponentVisual<Triangle>
    {
        /// <summary>
        /// Cria um triângulo, possui Begin e End.
        /// </summary>
        /// <param name="pointOne"></param>
        /// <param name="pointTwo"></param>
        /// <param name="pointThree"></param>
        public void Create((int, int) pointOne, (int, int) pointTwo, (int, int) pointThree)
        {
            var contexto = LineSegmentContext.NewInstance();
            contexto.Begin(Size);
            CriarLinha(pointThree, pointOne);
            CriarLinha(pointOne, pointTwo);
            CriarLinha(pointTwo, pointThree);
            contexto.End();
        }

        private void CriarLinha((int, int) pointOne, (int, int) pointTwo)
        {
            var linha = LineSegment.NewInstance();
            if (UseColor)
                linha.WithColor(Color);
            else
                linha.WithColor(Red, Green, Blue);
            linha.Create(pointOne, pointTwo);
        }

        public Triangle WithColor(Color color)
        {
            this.SetColor(color);

            return this;
        }

        public Triangle WithColor(double red, double green, double blue)
        {
            this.SetColor(red, green, blue);

            return this;
        }

        public Triangle WithSize(float size)
        {
            this.SetSize(size);

            return this;
        }
    }
}
