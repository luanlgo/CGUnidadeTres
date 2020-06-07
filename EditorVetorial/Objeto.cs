using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace LibraryComponent
{
    public abstract class Objeto
    {
        public string Rotulo { get; set; }
        public PrimitiveType TipoPrimitiva { get; set; }
        public float Tamanho { get; set; } = 2;
        public BBox bBox = new BBox();
        public BBox BBox { get => bBox; set => bBox = value; }
        public List<Objeto> Objetos { get; set; } = new List<Objeto>();

        private Transformacao4D _matriz = new Transformacao4D();
        /// Matrizes temporarias que sempre sao inicializadas com matriz Identidade entao podem ser "static".
        private static Transformacao4D _matrizTmpTranslacao = new Transformacao4D();
        private static Transformacao4D _matrizTmpTranslacaoInversa = new Transformacao4D();
        private static Transformacao4D _matrizTmpEscala = new Transformacao4D();
        private static Transformacao4D _matrizTmpRotacao = new Transformacao4D();
        private static Transformacao4D _matrizGlobal = new Transformacao4D();

        public Objeto(string rotulo)
        {
            this.Rotulo = rotulo;
            this.TipoPrimitiva = PrimitiveType.LineLoop;
        }

        public Objeto(string rotulo, PrimitiveType p)
        {
            this.Rotulo = rotulo;
            this.TipoPrimitiva = p;
        }
        
        public void Desenhar()
        {
            GL.PushMatrix();                                    // N3-Exe14: grafo de cena
            GL.MultMatrix(_matriz.ObterDados());
            DesenharAramado();
            for (var i = 0; i < Objetos.Count; i++)
                Objetos[i].Desenhar();
            GL.PopMatrix();           
        }

        public abstract void DesenharAramado();

        public void FilhoAdicionar(Objeto filho)
        {
            this.Objetos.Add(filho);
        }

        public void FilhoRemover(Objeto filho)
        {
            this.Objetos.Remove(filho);
        }

        public abstract void PontosExibir();

        public void PontosExibirObjeto()
        {
            PontosExibir();
            for (var i = 0; i < Objetos.Count; i++)
                Objetos[i].PontosExibirObjeto();
        }

        public void TranslacaoXY(double tx, double ty)
        {
            Transformacao4D matrizTranslate = new Transformacao4D();
            matrizTranslate.AtribuirTranslacao(tx, ty, 0);
            _matriz = matrizTranslate.MultiplicarMatriz(_matriz);
        }
        public void EscalaXY(double Sx, double Sy)
        {
            Transformacao4D matrizScale = new Transformacao4D();
            matrizScale.AtribuirEscala(Sx, Sy, 1.0);
            _matriz = matrizScale.MultiplicarMatriz(_matriz);
        }

        public void EscalaXYBBox(double escala)
        {
            _matrizGlobal.AtribuirIdentidade();
            Ponto4D pontoPivo = bBox.ObterCentro();

            _matrizTmpTranslacao.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z); // Inverter sinal
            _matrizGlobal = _matrizTmpTranslacao.MultiplicarMatriz(_matrizGlobal);

            _matrizTmpEscala.AtribuirEscala(escala, escala, 1.0);
            _matrizGlobal = _matrizTmpEscala.MultiplicarMatriz(_matrizGlobal);

            _matrizTmpTranslacaoInversa.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
            _matrizGlobal = _matrizTmpTranslacaoInversa.MultiplicarMatriz(_matrizGlobal);

            _matriz = _matriz.MultiplicarMatriz(_matrizGlobal);
        }
        public void RotacaoZ(double angulo)
        {
            _matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
            _matriz = _matrizTmpRotacao.MultiplicarMatriz(_matriz);
        }
        public void RotacaoZBBox(double angulo)
        {
            _matrizGlobal.AtribuirIdentidade();
            Ponto4D pontoPivo = bBox.ObterCentro();

            _matrizTmpTranslacao.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z); // Inverter sinal
            _matrizGlobal = _matrizTmpTranslacao.MultiplicarMatriz(_matrizGlobal);

            _matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
            _matrizGlobal = _matrizTmpRotacao.MultiplicarMatriz(_matrizGlobal);

            _matrizTmpTranslacaoInversa.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
            _matrizGlobal = _matrizTmpTranslacaoInversa.MultiplicarMatriz(_matrizGlobal);

            _matriz = _matriz.MultiplicarMatriz(_matrizGlobal);
        }
    }
}