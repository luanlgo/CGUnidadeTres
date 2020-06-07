using LibraryComponent;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace CrossCutting
{
    public class LineStrip : ComponentVisual<LineStrip>, IComponentVisual<LineStrip>
    {
        private (int, int) UltimoPonto { get; set; }

        public void Create(double amountPoints, Ponto4D pontoA, Ponto4D pontoB, Ponto4D pontoC, Ponto4D pontoD)
        {
            Create(amountPoints,
                   (pontoA.X.ToInt(), pontoA.Y.ToInt()),
                   (pontoB.X.ToInt(), pontoB.Y.ToInt()),
                   (pontoC.X.ToInt(), pontoC.Y.ToInt()),
                   (pontoD.X.ToInt(), pontoD.Y.ToInt()));
        }

        public void Create(double amountPoints, (int, int) pontoA, (int, int) pontoB, (int, int) pontoC, (int, int) pontoD)
        {
            UltimoPonto = (pontoD.Item1, pontoD.Item2);

            if (UseColor)
                GL.Color3(Color);
            else
                GL.Color3(Red, Green, Blue);

            var contexto = LineStripContext.NewInstance();
            contexto.Begin(Size);

            for (double intervalo = 0; intervalo <= 1; intervalo += 1D / amountPoints)
            {
                var ab = GerarPonto(pontoA, pontoB, intervalo);
                var bc = GerarPonto(pontoB, pontoC, intervalo);
                var cd = GerarPonto(pontoC, UltimoPonto, intervalo);
                var abc = GerarPonto(ab, bc, intervalo);
                var bcd = GerarPonto(bc, cd, intervalo);
                var abcd = GerarPonto(abc, bcd, intervalo);
                GL.Vertex2(abcd.Item1, abcd.Item2);
            }

            GL.Vertex2(UltimoPonto.Item1, UltimoPonto.Item2);

            contexto.End();
        }

        private (double, double) GerarPonto((double, double) pontoA, (double, double) pontoB, double intervalo)
        {
            var xR = pontoA.Item1 + (pontoB.Item1 - pontoA.Item1) * intervalo;
            var yR = pontoA.Item2 + (pontoB.Item2 - pontoA.Item2) * intervalo;
            return (xR, yR);
        }

        public LineStrip WithColor(Color color)
        {
            this.SetColor(color);

            return this;
        }

        public LineStrip WithColor(double red, double green, double blue)
        {
            this.SetColor(red, green, blue);

            return this;
        }

        public LineStrip WithSize(float size)
        {
            this.SetSize(size);

            return this;
        }
    }
}
