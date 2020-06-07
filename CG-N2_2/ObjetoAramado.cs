using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace LibraryComponent
{
  internal class ObjetoAramado : Objeto
  {
    protected List<Ponto4D> pontosLista = new List<Ponto4D>();

    public ObjetoAramado(string rotulo) : base(rotulo) { }

    protected override void DesenharAramado()
    {
      GL.LineWidth(base.Tamanho);
      GL.Color3(Color.White);
      GL.Begin(base.TipoPrimitiva);
      foreach (Ponto4D pto in pontosLista)
      {
        GL.Vertex2(pto.X, pto.Y);
      }
      GL.End();
    }

    protected void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
    }

    protected void PontosRemoverTodos()
    {
      pontosLista.Clear();
    }

    protected override void PontosExibir()
    {
      Console.WriteLine("__ Objeto: " + base.Rotulo);
      for (var i = 0; i < pontosLista.Count; i++)
      {
        Console.WriteLine("P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]");
      }
    }
  }
}