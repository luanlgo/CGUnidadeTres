using System;

namespace Biblioteca
{
    internal class Retangulo : ObjetoAramado
    {
        private Ponto4D PontoInferiorEsquerda { get; set; }
        private Ponto4D PontoSuperiorDireita { get; set; }

        public Retangulo(string rotulo, Ponto4D pontoInfEsq, Ponto4D pontoSupDir) : base(rotulo)
        {
            this.PontoInferiorEsquerda = pontoInfEsq;
            this.PontoSuperiorDireita = pontoSupDir;
            GerarPtosRetangulo();
        }

        private void GerarPtosRetangulo()
        {
            base.PontosRemoverTodos();
            base.PontosAdicionar(PontoInferiorEsquerda);
            base.PontosAdicionar(new Ponto4D(PontoSuperiorDireita.X, PontoInferiorEsquerda.Y));
            base.PontosAdicionar(PontoSuperiorDireita);
            base.PontosAdicionar(new Ponto4D(PontoInferiorEsquerda.X, PontoSuperiorDireita.Y));
        }

        public void MoverPtoSupDir(Ponto4D ptoMover)
        {
            PontoSuperiorDireita = ptoMover;
            GerarPtosRetangulo();
        }
    }
}