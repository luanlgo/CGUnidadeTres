using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Biblioteca
{
    internal abstract class Objeto
    {
        protected string Rotulo { get; set; }
        protected PrimitiveType TipoPrimitiva { get; set; } = PrimitiveType.LineLoop;
        protected float Tamanho { get; set; } = 2;
        private BBox BoundedBox { get; set; } = new BBox();
        private List<Objeto> Objetos { get; set; } = new List<Objeto>();

        protected Objeto(string rotulo)
        {
            this.Rotulo = rotulo;
        }
        
        public void Desenhar()
        {
            DesenharAramado();
            for (var i = 0; i < Objetos.Count; i++)
                Objetos[i].Desenhar();
        }

        protected abstract void DesenharAramado();

        public void FilhoAdicionar(Objeto filho)
        {
            this.Objetos.Add(filho);
        }

        public void FilhoRemover(Objeto filho)
        {
            this.Objetos.Remove(filho);
        }

        protected abstract void PontosExibir();

        public void PontosExibirObjeto()
        {
            PontosExibir();
            for (var i = 0; i < Objetos.Count; i++)
                Objetos[i].PontosExibirObjeto();
        }
    }
}