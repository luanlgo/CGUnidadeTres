/*
  Autor: Dalton Solano dos Reis
*/

namespace Biblioteca
{
    /// <summary>
    /// Classe para controlar a câmera sintética.
    /// </summary>
    public class Camera
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public double ZMin { get; set; }
        public double ZMax { get; set; }

        /// <summary>
        /// Construtor da classe inicializando com valores padrões
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <param name="zMin"></param>
        /// <param name="zMax"></param>
        public Camera(double xMin = 0, double xMax = 600, double yMin = 0, double yMax = 600, double zMin = -1, double zMax = 1)
        {
            this.XMin = xMin; this.XMax = xMax;
            this.YMin = yMin; this.YMax = yMax;
            this.ZMin = zMin; this.ZMax = zMax;
        }

        public void PanEsquerda()
        {
            XMin += 2;
            XMax += 2;
        }

        public void PanDireita()
        {
            XMin -= 2;
            XMax -= 2;
        }

        public void PanCima()
        {
            YMin -= 2;
            YMax -= 2;
        }

        public void PanBaixo()
        {
            YMin += 2;
            YMax += 2;
        }
 
        public void ZoomIn()
        {
            XMin += 2; XMax -= 2; YMin += 2; YMax -= 2;
        }
 
        public void ZoomOut()
        {
            XMin -= 2; XMax += 2; YMin -= 2; YMax += 2;
        }
    }
}