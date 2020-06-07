using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Biblioteca;
using System.Collections.Generic;
using OpenTK.Input;
using Jantz.ComputerGraphics.Common;

namespace EditorVetorial
{
    class World : WorldBase
    {
        private Camera Camera { get; set; }
        private List<ObjetoAramado> objetosLista = new List<ObjetoAramado>();
        private ObjetoAramado oa;
        private int mode;
        private int tmode;
        private int ptTipo;
        private Ponto4D lastPonto;
        private Ponto4D currentPonto;
        private List<PrimitiveType> tiposLista = new List<PrimitiveType>();

        private Ponto4D mouse;
        private Ponto4D mouseEscala;
        private ObjetoAramado pai;

        public World(int width, int height) : base(width, height)
        {
            Camera = new Camera();
            this.mode = 2;
            this.ptTipo = 0;
            this.tmode = 0;

            this.tiposLista.Add(PrimitiveType.Lines);
            this.tiposLista.Add(PrimitiveType.Points);
            this.tiposLista.Add(PrimitiveType.LineLoop);
            this.tiposLista.Add(PrimitiveType.LineStrip);
            this.tiposLista.Add(PrimitiveType.Triangles);
            this.tiposLista.Add(PrimitiveType.TriangleStrip);
            this.tiposLista.Add(PrimitiveType.TriangleFan);
            this.tiposLista.Add(PrimitiveType.Quads);
            this.tiposLista.Add(PrimitiveType.QuadStrip);
            this.tiposLista.Add(PrimitiveType.Polygon);

            Console.WriteLine("Create Mode Ativo!");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Gray);

            this.oa = new ObjetoAramado(randomString(), color(), this.tiposLista[ptTipo]);
            this.pai = new ObjetoAramado(randomString(), color(), this.tiposLista[ptTipo]);
            objetosLista.Add(oa);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(Camera.XMin, Camera.XMax, Camera.YMin, Camera.YMax, Camera.ZMin, Camera.ZMax);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Color3(Color.Red);
            GL.LineWidth(10);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(0, 0); GL.Vertex2(200, 0);
            GL.Color3(Color.Green);
            GL.Vertex2(0, 0); GL.Vertex2(0, 200);
            GL.End();

            foreach (ObjetoAramado o in objetosLista)
            {
                o.Desenhar();
            }

            this.pai.DesenharAramado();

            //desenha bbox em torno de objeto selecionado
            if (this.oa != null && (this.mode == 1 || this.mode == 3)) {
                double MaiorX = 0;
                double MaiorY = 0;
                double MenorX = 999999999;
                double MenorY = 999999999;
                foreach (var item in this.oa.pontosLista)
                {
                    if (item.X > MaiorX)
                        MaiorX = item.X;
                    if (item.Y > MaiorY)
                        MaiorY = item.Y;

                    if (item.X < MenorX)
                        MenorX = item.X;
                    if (item.Y < MenorY)
                        MenorY = item.Y;
                }
                this.oa.BBox.Atualizar(MaiorX,MaiorY,MenorX,MenorY);
                this.oa.BBox.Desenhar(Color.Yellow);
                this.oa.BBox.ProcessarCentro();
            }

            //desenha bbox em torno do vertice selecionado
            if (this.currentPonto != null && this.mode == 0) {
                this.oa.BBox.Atualizar(this.currentPonto.X,this.currentPonto.Y,this.currentPonto.X,this.currentPonto.Y);
                this.oa.BBox.Desenhar(Color.Yellow);
                this.oa.BBox.ProcessarCentro();
            }
            
            this.SwapBuffers();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {

            var x = (e.Position.X / 2);
            var y = 600 - (e.Position.Y / 2);

            if (this.mode == 2)
            {
                this.lastPonto = new Ponto4D(x, y, 0, 1);
                this.currentPonto = new Ponto4D(x, y, 0, 1);
                this.oa.PontosAdicionar(this.lastPonto);
                this.oa.PontosAdicionar(this.currentPonto);
                this.oa.DesenharAramado();
            }
            else if (this.mode == 1)
            {
                this.selecionarPoligono(x, y);
            }
            else if (this.mode == 3)
            {
                this.selecionarPoligono(x, y);
                if (this.pai == null) {
                    this.pai = new ObjetoAramado(randomString(), color(), this.tiposLista[0]);
                    foreach (var item in this.oa.pontosLista)
                    {
                        this.pai.PontosAdicionar(item);
                    }
                    this.oa.PontosRemoverTodos();
                    this.pai.DesenharAramado();
                } else {
                    foreach (var item in this.oa.pontosLista)
                    {
                        this.pai.PontosAdicionar(item);
                    }
                    this.oa.PontosRemoverTodos();
                }
                // this.selecionarPoligono(x, y);
                // if (this.pai == null) {
                //     this.pai = new ObjetoAramado(randomString(), color(), this.tiposLista[0]);
                //     this.pai.FilhoAdicionar(this.oa);
                //     this.pai.Desenhar();
                // } else {
                //     this.pai.FilhoAdicionar(this.oa);
                //     this.pai.Desenhar();
                // }
            }
            else
            {
                this.selecionarVertice(x, y);
            }

        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {

            var x = (e.Position.X / 2);
            var y = 600 - (e.Position.Y / 2);

            if (this.currentPonto != null)
            {
                this.currentPonto.X = x;
                this.currentPonto.Y = y;
            }

            if (this.mode == 1 || this.mode == 3)
            {
                //grava posicoes anteriores do mouse para escala
                if (this.mouseEscala == null)
                    this.mouseEscala = new Ponto4D(x,y);

                //grava posicoes anteriores do mouse para translacao
                if (this.mouse == null) {
                    this.mouse = new Ponto4D(x,y);

                    this.oa.BBox.ProcessarCentro();
                    var MouseX = (this.oa.BBox.obterCentro.X - x)*-1;
                    var MouseY = (this.oa.BBox.obterCentro.Y - y)*-1;
                    this.oa.TranslacaoXY(MouseX, MouseY);
                }

                var valorX = (this.mouse.X - x)*-1;
                var valorY = (this.mouse.Y - y)*-1;

                if (this.tmode == 0) {
                    this.oa.TranslacaoXY(valorX, valorY);
                } else if (this.tmode == 1) {
                    this.oa.RotacaoZBBox(valorX);
                } else if (this.tmode == 2) {
                    var valorMultiplicacao = 1.5;
                    if (x != this.mouse.X) {
                        if (x > this.mouse.X)
                            this.oa.EscalaXYBBox(1/valorMultiplicacao);
                        else
                            this.oa.EscalaXYBBox(1*valorMultiplicacao);
                    }

                }

                this.mouse.X = x;
                this.mouse.Y = y;
                
            }

        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Exit();
                    break;
                case Key.Left:
                    this.oa.TranslacaoXY(-10, 0);     // N3-Exe10: translação
                    break;
                case Key.Right:
                    this.oa.TranslacaoXY(10, 0);      // N3-Exe10: translação
                    break;
                case Key.Up:
                    this.oa.TranslacaoXY(0, 10);      // N3-Exe10: translação
                    break;
                case Key.Down:
                    this.oa.TranslacaoXY(0, -10);     // N3-Exe10: translação
                    break;
                case Key.PageUp:
                    this.oa.EscalaXY(2, 2);
                    break;
                case Key.PageDown:
                    this.oa.EscalaXY(0.5, 0.5);
                    break;
                case Key.Number9:
                    this.oa.EscalaXYBBox(0.5);        // N3-Exe11: escala
                    break;
                case Key.Number0:
                    this.oa.EscalaXYBBox(2);          // N3-Exe11: escala
                    break;
                case Key.Number1:
                    this.oa.RotacaoZ(10);
                    break;
                case Key.Number2:
                    this.oa.RotacaoZ(-10);
                    break;
                case Key.Number3:
                    this.oa.RotacaoZBBox(10);         // N3-Exe12: rotação
                    break;
                case Key.Number4:
                    this.oa.RotacaoZBBox(-10);        // N3-Exe12: rotação
                    break;
                case Key.R:
                    this.oa.Cor[0] = this.oa.Cor[0] + 0.1;
                    break;
                case Key.G:
                    this.oa.Cor[1] = this.oa.Cor[1] + 0.1;
                    break;
                case Key.B:
                    this.oa.Cor[2] = this.oa.Cor[2] + 0.1;
                    break;
                case Key.V:
                    removeV();
                    break;
                case Key.P:
                    Console.WriteLine("Atribuido pai a selecao");
                    this.oa = this.pai;
                    this.oa.Desenhar();
                    this.oa.PontosExibirObjeto();
                    break;
                case Key.Z:
                    if (this.tmode < 3)
                        this.tmode = 99;
                    else
                        this.tmode = 0;
                    break;
                case Key.C:
                    if (this.mode == 0)
                    {
                        Console.WriteLine("Select Mode Ativo!");
                        this.mode = 1;
                        // this.tiraRastro();
                    }
                    else if (this.mode == 1)
                    {
                        Console.WriteLine("Create Mode Ativo!");
                        this.mode = 2;
                        this.colocaRastro();
                    }
                    else if (this.mode == 2)
                    {
                        Console.WriteLine("GrafoScene Mode Ativo!");
                        this.mode = 3;
                        this.tiraRastro();
                    }
                    else
                    {
                        Console.WriteLine("Select Vertice Mode Ativo!");
                        this.mode = 0;
                    }
                    break;
                case Key.L:
                    if (this.tmode == 0)
                    {
                        Console.WriteLine("Rotação Ativa!");
                        this.tmode = 1;
                    }
                    else if (this.tmode == 1)
                    {
                        Console.WriteLine("Escala Ativa!");
                        this.tmode = 2;
                        this.mouse = null;
                    }
                    else
                    {
                        Console.WriteLine("Translação Ativa!");
                        this.tmode = 0;
                    }
                    break;
                case Key.T:
                    Console.WriteLine("Primitiva alterada!");
                    trocaPrimitiva();
                    break;
                case Key.N:
                    novoObjeto();
                    break;
                case Key.X:
                    this.currentPonto = null;
                    break;
                default:
                    break;
            }
        }

        public double[] color()
        {
            Random random = new Random();
            return new double[] { random.NextDouble(), random.NextDouble(), random.NextDouble() };
        }

        public void criarVertice()
        {
            this.oa = new ObjetoAramado(randomString(), color(), this.tiposLista[ptTipo]);
            this.objetosLista.Add(oa);
        }

        public void selecionarVertice(double x, double y)
        {
            Ponto4D pto_prox = new Ponto4D(999999999,999999999,0);
            double ptoi_prox = 999999999;
            double distanciaEuclidiana = 999999999;
            foreach (ObjetoAramado objeto in this.objetosLista)
            {
                foreach (Ponto4D pto in objeto.pontosLista)
                {
                    double x1 = x;
                    double y1 = y;
                    double x2 = pto.X;
                    double y2 = pto.Y;
                    distanciaEuclidiana = Math.Sqrt(((x2-x1)*(x2-x1))+((y2-y1)*(y2-y1)));
                    if (distanciaEuclidiana < ptoi_prox) {
                        pto_prox = pto;
                        ptoi_prox = distanciaEuclidiana;
                        this.oa = objeto;
                    }
                }
            }
            this.currentPonto = pto_prox;
        }

        public void novoObjeto()
        {
            this.tiraRastro();
            this.criarVertice();
        }

        public void tiraRastro()
        {
            this.currentPonto = null;
            this.lastPonto = this.currentPonto;
            if (this.oa.pontosLista.Count > 0)
                this.oa.pontosLista.RemoveAt(this.oa.pontosLista.Count - 1);
        }

        public void removeV()
        {
            for (int i = 0; i < this.oa.pontosLista.Count; i++)
            {
                if (this.oa.pontosLista[i].X == this.currentPonto.X && this.oa.pontosLista[i].Y == this.currentPonto.Y)
                    this.oa.pontosLista.RemoveAt(i);
            }
            tiraRastro();
        }

        public void colocaRastro()
        {
            this.currentPonto = new Ponto4D(300, 300, 0, 1);
            this.oa.PontosAdicionar(this.currentPonto);
        }

        public void selecionarPoligono(int x, int yi)
        {
            int count = 0;

            foreach (ObjetoAramado item in this.objetosLista)
            {

                for (int i = 0; i < item.pontosLista.Count; i++)
                {
                    if (i + 1 < item.pontosLista.Count)
                    {
                        double y1 = item.pontosLista[i].Y;
                        double x1 = item.pontosLista[i].X;
                        double y2 = item.pontosLista[i+1].Y;
                        double x2 = item.pontosLista[i+1].X;
                        double ti = (yi-y1)/(y2-y1);
                        double xi = x1+(x2-x1)*ti;
                        if (ti >= 0 && ti <= 1 && xi > x)
                            count++;
                    }

                }
                
                if (!((count%2)==0)) {
                    Console.WriteLine("Poligono "+item.Rotulo+" selecionado!");
                    this.oa = item;
                    break;
                }
                
            }

        }

        public void trocaPrimitiva()
        {
            if (this.ptTipo >= 9)
                this.ptTipo = 0;
            else
                this.ptTipo++;
        }

        public static string randomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
            stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

    }
}
