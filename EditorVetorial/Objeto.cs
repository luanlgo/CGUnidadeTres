using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System;

namespace Biblioteca
{
    public abstract class Objeto
    {
        public string Rotulo { get; set; }
        public PrimitiveType TipoPrimitiva { get; set; }
        public float Tamanho { get; set; } = 2;
        public BBox bBox = new BBox();
        public BBox BBox { get => bBox; set => bBox = value; }
        public List<Objeto> Objetos { get; set; } = new List<Objeto>();

        private Transformacao4D matriz = new Transformacao4D();
        /// Matrizes temporarias que sempre sao inicializadas com matriz Identidade entao podem ser "static".
        private static Transformacao4D matrizTmpTranslacao = new Transformacao4D();
        private static Transformacao4D matrizTmpTranslacaoInversa = new Transformacao4D();
        private static Transformacao4D matrizTmpEscala = new Transformacao4D();
        private static Transformacao4D matrizTmpRotacao = new Transformacao4D();
        private static Transformacao4D matrizGlobal = new Transformacao4D();

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
            GL.MultMatrix(matriz.ObterDados());
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
            matriz = matrizTranslate.MultiplicarMatriz(matriz);
        }
        public void EscalaXY(double Sx, double Sy)
        {
            Transformacao4D matrizScale = new Transformacao4D();
            matrizScale.AtribuirEscala(Sx, Sy, 1.0);
            matriz = matrizScale.MultiplicarMatriz(matriz);
        }

        public void EscalaXYBBox(double escala)
        {
            matrizGlobal.AtribuirIdentidade();
            Ponto4D pontoPivo = bBox.obterCentro;

            matrizTmpTranslacao.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z); // Inverter sinal
            matrizGlobal = matrizTmpTranslacao.MultiplicarMatriz(matrizGlobal);

            matrizTmpEscala.AtribuirEscala(escala, escala, 1.0);
            matrizGlobal = matrizTmpEscala.MultiplicarMatriz(matrizGlobal);

            matrizTmpTranslacaoInversa.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
            matrizGlobal = matrizTmpTranslacaoInversa.MultiplicarMatriz(matrizGlobal);

            matriz = matriz.MultiplicarMatriz(matrizGlobal);
        }
        public void RotacaoZ(double angulo)
        {
            matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
            matriz = matrizTmpRotacao.MultiplicarMatriz(matriz);
        }
        public void RotacaoZBBox(double angulo)
        {
            matrizGlobal.AtribuirIdentidade();
            Ponto4D pontoPivo = bBox.obterCentro;

            matrizTmpTranslacao.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z); // Inverter sinal
            matrizGlobal = matrizTmpTranslacao.MultiplicarMatriz(matrizGlobal);

            matrizTmpRotacao.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
            matrizGlobal = matrizTmpRotacao.MultiplicarMatriz(matrizGlobal);

            matrizTmpTranslacaoInversa.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
            matrizGlobal = matrizTmpTranslacaoInversa.MultiplicarMatriz(matrizGlobal);

            matriz = matriz.MultiplicarMatriz(matrizGlobal);
        }
    }
}