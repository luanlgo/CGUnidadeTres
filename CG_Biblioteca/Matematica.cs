/*
  Autor: Dalton Solano dos Reis
*/

using System;

namespace Biblioteca
{
    /// <summary>
    /// Classe com funções matemáticas.
    /// </summary>
    public static class Matematica
    {
        /// <summary>
        /// Função para calcular um ponto sobre o perímetro de um círculo informando um ângulo e raio.
        /// </summary>
        /// <param name="angulo"></param>
        /// <param name="raio"></param>
        /// <returns></returns>
        public static Ponto4D GerarPontosCirculo(double angulo, double raio)
        {
            return new Ponto4D()
            {
                X = raio * Math.Cos(Math.PI * angulo / 180.0),
                Y = raio * Math.Sin(Math.PI * angulo / 180.0),
                Z = 0
            };
        }

        public static Ponto4D GerarPontosCirculo(double angulo, double raio, double centerx, double centery)
        {
            Ponto4D pto = new Ponto4D();
            pto.X = (raio * Math.Cos(Math.PI * angulo / 180.0)) + centerx;
            pto.Y = (raio * Math.Sin(Math.PI * angulo / 180.0)) + centery;
            pto.Z = 0;
            return (pto);
        }

        public static double GerarPontosCirculoSimetrico(double raio)
        {
            return raio * Math.Cos(Math.PI * 45 / 180.0);
        }
    }
}