/*
  Autor: Dalton Solano dos Reis
*/

using System;

namespace Biblioteca
{
    /// <summary>
    /// Classe que define as Transformacoes Geometricas no espaco 3D
    /// Uma matriz de Transformacao eh representada por uma matriz 4x4 que acumula trasnformacoes, isto eh, 
    /// para aplicar as trasnformacoes T1, T2, em seguida, T3, eh necessario multiplicar T1 * T2 * T3. 
    /// Os valores de Translacao estao na coluna mais a direita.
    ///   Organizacao dos elementos internos da Matriz
    ///               [ matrix[0] matrix[4] matrix[8]  matrix[12] ]
    ///   Transform = [ matrix[1] matrix[5] matrix[9]  matrix[13] ]
    ///               [ matrix[2] matrix[6] matrix[10] matrix[14] ]
    ///               [ matrix[3] matrix[7] matrix[11] matrix[15] ]
    /// </summary>
    public class Transformacao4D
    {
        static public readonly double DEG_TO_RAD = 0.017453292519943295769236907684886;

        /// \brief Cria uma matriz de Trasnformacao com uma matriz Identidade.
        private readonly double[] Matriz = 
            {
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            };

        public Transformacao4D()
        {
        }

        /// Atribui os valores de uma matriz Identidade a matriz de Transformacao.
        public void AtribuirIdentidade()
        {
            for (int i = 0; i < 16; ++i)
            {
                Matriz[i] = 0.0;
            }
            Matriz[0] = Matriz[5] = Matriz[10] = Matriz[15] = 1.0;
        }

        /// Atribui os valores de Translacao (tx,ty,tz) a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirTranslacao(double tx, double ty, double tz)
        {
            AtribuirIdentidade();
            Matriz[12] = tx;
            Matriz[13] = ty;
            Matriz[14] = tz;
        }

        /// Atribui o valor de Escala (Ex,Ey,Ez) a matriz de Transformacao.
        /// Elemento Neutro eh 1 (um).
        /// Se manter os valores iguais de Ex,Ey e Ez o objeto vai ser reduzido ou ampliado proporcionalmente.
        public void AtribuirEscala(double sX, double sY, double sZ)
        {
            AtribuirIdentidade();
            Matriz[0] = sX;
            Matriz[5] = sY;
            Matriz[10] = sZ;
        }

        /// Atribui o valor de Rotacao (angulo) no eixo X a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirRotacaoX(double radians)
        {
            AtribuirIdentidade();
            Matriz[5] = Math.Cos(radians);
            Matriz[9] = -Math.Sin(radians);
            Matriz[6] = Math.Sin(radians);
            Matriz[10] = Math.Cos(radians);
        }

        /// Atribui o valor de Rotacao (angulo) no eixo Y a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirRotacaoY(double radians)
        {
            AtribuirIdentidade();
            Matriz[0] = Math.Cos(radians);
            Matriz[8] = Math.Sin(radians);
            Matriz[2] = -Math.Sin(radians);
            Matriz[10] = Math.Cos(radians);
        }

        /// Atribui o valor de Rotacao (angulo) no eixo Z a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirRotacaoZ(double radians)
        {
            AtribuirIdentidade();
            Matriz[0] = Math.Cos(radians);
            Matriz[4] = -Math.Sin(radians);
            Matriz[1] = Math.Sin(radians);
            Matriz[5] = Math.Cos(radians);
        }

        public Ponto4D MultiplicarPonto(Ponto4D pto)
        {
            Ponto4D pointResult = new Ponto4D(
                Matriz[0] * pto.X + Matriz[4] * pto.Y + Matriz[8] * pto.Z + Matriz[12] * pto.W,
                Matriz[1] * pto.X + Matriz[5] * pto.Y + Matriz[9] * pto.Z + Matriz[13] * pto.W,
                Matriz[2] * pto.X + Matriz[6] * pto.Y + Matriz[10] * pto.Z + Matriz[14] * pto.W,
                Matriz[3] * pto.X + Matriz[7] * pto.Y + Matriz[11] * pto.Z + Matriz[15] * pto.W);
            return pointResult;
        }

        public Transformacao4D MultiplicarMatriz(Transformacao4D t)
        {
            Transformacao4D result = new Transformacao4D();
            for (int i = 0; i < 16; ++i)
                result.Matriz[i] =
                      Matriz[i % 4] * t.Matriz[i / 4 * 4] + Matriz[(i % 4) + 4] * t.Matriz[i / 4 * 4 + 1]
                    + Matriz[(i % 4) + 8] * t.Matriz[i / 4 * 4 + 2] + Matriz[(i % 4) + 12] * t.Matriz[i / 4 * 4 + 3];
            return result;
        }

        public double ObterElemento(int index)
        {
            return Matriz[index];
        }

        public void AtribuirElemento(int index, double value)
        {
            Matriz[index] = value;
        }

        public double[] ObterDados()
        {
            return Matriz;
        }

        public void AtribuirDados(double[] data)
        {
            int i;

            for (i = 0; i < 16; i++)
            {
                Matriz[i] = (data[i]);
            }
        }

        public void ExibeMatriz()
        {
            Console.WriteLine("______________________");
            Console.WriteLine("|" + ObterElemento(0) + " | " + ObterElemento(4) + " | " + ObterElemento(8) + " | " + ObterElemento(12));
            Console.WriteLine("|" + ObterElemento(1) + " | " + ObterElemento(5) + " | " + ObterElemento(9) + " | " + ObterElemento(13));
            Console.WriteLine("|" + ObterElemento(2) + " | " + ObterElemento(6) + " | " + ObterElemento(10) + " | " + ObterElemento(14));
            Console.WriteLine("|" + ObterElemento(3) + " | " + ObterElemento(7) + " | " + ObterElemento(11) + " | " + ObterElemento(15));
        }
    }
}