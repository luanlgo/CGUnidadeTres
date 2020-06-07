/*
  Autor: Dalton Solano dos Reis
*/

using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Biblioteca
{
    public class BBox
    {
        public double MenorX { get; private set; }
        public double MenorY { get; private set; }
        public double MenorZ { get; private set; }
        public double MaiorX { get; private set; }
        public double MaiorY { get; private set; }
        public double MaiorZ { get; private set; }
        public Ponto4D Centro { get; private set; } = new Ponto4D();

        public BBox(double menorX = 0, double menorY = 0, double menorZ = 0, double maiorX = 0, double maiorY = 0, double maiorZ = 0)
        {
            this.MenorX = menorX; this.MenorY = menorY; this.MenorZ = menorZ;
            this.MaiorX = maiorX; this.MaiorY = maiorY; this.MaiorZ = maiorZ;
        }

        public void Atribuir(Ponto4D pto)
        {
            this.MenorX = pto.X; this.MenorY = pto.Y; this.MenorZ = pto.Z;
            this.MaiorX = pto.X; this.MaiorY = pto.Y; this.MaiorZ = pto.Z;
        }

        public void Atualizar(Ponto4D pto)
        {
            Atualizar(pto.X, pto.Y, pto.Z);
        }

        public void Atualizar(double x, double y, double z)
        {
            if (x < MenorX)
                MenorX = x;
            else
            {
                if (x > MaiorX) MaiorX = x;
            }
            if (y < MenorY)
                MenorY = y;
            else
            {
                if (y > MaiorY) MaiorY = y;
            }
            if (z < MenorZ)
                MenorZ = z;
            else
            {
                if (z > MaiorZ) MaiorZ = z;
            }
        }

        public void ProcessarCentro()
        {
            Centro.X = (MaiorX + MenorX) / 2;
            Centro.Y = (MaiorY + MenorY) / 2;
            Centro.Z = (MaiorZ + MenorZ) / 2;
        }

        public void Desenhar()
        {
            Desenhar(Color.Brown);
        }

        public void Desenhar(Color cor)
        {
            GL.Color3(cor);

            GL.PointSize(5);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(Centro.X, Centro.Y);
            GL.End();

            GL.LineWidth(3);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(MenorX, MaiorY, MenorZ);
            GL.Vertex3(MaiorX, MaiorY, MenorZ);
            GL.Vertex3(MaiorX, MenorY, MenorZ);
            GL.Vertex3(MenorX, MenorY, MenorZ);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(MenorX, MenorY, MenorZ);
            GL.Vertex3(MenorX, MenorY, MaiorZ);
            GL.Vertex3(MenorX, MaiorY, MaiorZ);
            GL.Vertex3(MenorX, MaiorY, MenorZ);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(MaiorX, MaiorY, MaiorZ);
            GL.Vertex3(MenorX, MaiorY, MaiorZ);
            GL.Vertex3(MenorX, MenorY, MaiorZ);
            GL.Vertex3(MaiorX, MenorY, MaiorZ);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(MaiorX, MenorY, MenorZ);
            GL.Vertex3(MaiorX, MaiorY, MenorZ);
            GL.Vertex3(MaiorX, MaiorY, MaiorZ);
            GL.Vertex3(MaiorX, MenorY, MaiorZ);
            GL.End();
        }
    }
}