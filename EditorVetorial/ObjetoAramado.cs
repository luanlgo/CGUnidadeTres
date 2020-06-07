using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Biblioteca
{
  public class ObjetoAramado : Objeto
  {
    public List<Ponto4D> pontosLista = new List<Ponto4D>();
    public double[] Cor;
    public ObjetoAramado(string rotulo) : base(rotulo) { }
    public ObjetoAramado(string rotulo,double[] cor, PrimitiveType p) : base(rotulo,p) {
      this.Cor = cor;
    }

    public override void DesenharAramado()
    {
      GL.LineWidth(base.Tamanho);
      GL.Color3(this.Cor[0],this.Cor[1],this.Cor[2]);
      GL.Begin(base.TipoPrimitiva);
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex2(pto.X, pto.Y);
      }
      GL.End();
    }

    public void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
    }

    public void PontosRemoverTodos()
    {
      pontosLista.Clear();
    }

    public override void PontosExibir()
    {
      Console.WriteLine("__ Objeto: " + base.Rotulo);
      for (var i = 0; i < pontosLista.Count; i++)
      {
        Console.WriteLine("P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]");
      }
    }
  }
}