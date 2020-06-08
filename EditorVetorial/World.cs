using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using LibraryComponent;
using System.Collections.Generic;
using OpenTK.Input;
using CrossCutting;

namespace EditorVetorial
{
    class World : CustomGameWindow
    {
        private ListPontos _pontos;
        private int _mode;
        private int _tmode;
        private int _ptTipo;
        private Ponto4D _lastPonto;
        private Ponto4D _currentPonto;

        private readonly List<ListPontos> _pontosList = new List<ListPontos>();
        private readonly List<PrimitiveType> _tiposLista = new List<PrimitiveType>();

        private Ponto4D _mouse;
        private Ponto4D _mouseEscala;
        private ListPontos _pontosPai;

        public World(int width, int height) : base(width, height)
        {
            _mode = 2;
            _ptTipo = 0;
            _tmode = 0;

            _tiposLista.Add(PrimitiveType.Lines);
            _tiposLista.Add(PrimitiveType.Points);
            _tiposLista.Add(PrimitiveType.LineLoop);
            _tiposLista.Add(PrimitiveType.LineStrip);
            _tiposLista.Add(PrimitiveType.Triangles);
            _tiposLista.Add(PrimitiveType.TriangleStrip);
            _tiposLista.Add(PrimitiveType.TriangleFan);
            _tiposLista.Add(PrimitiveType.Quads);
            _tiposLista.Add(PrimitiveType.QuadStrip);
            _tiposLista.Add(PrimitiveType.Polygon);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(System.Drawing.Color.Gray);

            _pontos = new ListPontos("amarado", Color(), _tiposLista[_ptTipo]);
            _pontosPai = new ListPontos("amarado", Color(), _tiposLista[_ptTipo]);
            _pontosList.Add(_pontos);
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

            GL.Color3(System.Drawing.Color.Red);
            GL.LineWidth(10);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(0, 0); GL.Vertex2(200, 0);
            GL.Color3(System.Drawing.Color.Green);
            GL.Vertex2(0, 0); GL.Vertex2(0, 200);
            GL.End();

            foreach (ListPontos o in _pontosList)
            {
                o.Desenhar();
            }

            _pontosPai.DesenharPonto();

            //desenha bbox em torno de objeto selecionado
            if (_pontos != null && (_mode == 1 || _mode == 3)) {
                double MaiorX = 0;
                double MaiorY = 0;
                double MenorX = 999999999;
                double MenorY = 999999999;
                foreach (var item in _pontos.pontosLista)
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
                _pontos.BBox.Atualizar(MaiorX,MaiorY,MenorX,MenorY);
                _pontos.BBox.Desenhar(System.Drawing.Color.Yellow);
                _pontos.BBox.ProcessarCentro();
            }

            //desenha bbox em torno do vertice selecionado
            if (_currentPonto != null && _mode == 0) {
                _pontos.BBox.Atualizar(_currentPonto.X, _currentPonto.Y, _currentPonto.X, _currentPonto.Y);
                _pontos.BBox.Desenhar(System.Drawing.Color.Yellow);
                _pontos.BBox.ProcessarCentro();
            }
            
            SwapBuffers();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            var x = GetXEventMouse(e);
            var y = GetYEventMouse(e);
            Console.WriteLine(x.ToString(), y.ToString());
            if (_mode == 2)
            {
                _lastPonto = new Ponto4D(x, y, 0, 1);
                _currentPonto = new Ponto4D(x, y, 0, 1);
                _pontos.PontosAdicionar(_lastPonto);
                _pontos.PontosAdicionar(_currentPonto);
                _pontos.DesenharPonto();
            }
            else if (_mode == 1)
            {
                SelecionarPoligono(x, y);
            }
            else if (_mode == 3)
            {
                SelecionarPoligono(x, y);
                if (_pontosPai == null) {
                    _pontosPai = new ListPontos("ponto", Color(), _tiposLista[0]);
                    foreach (var item in _pontos.pontosLista)
                    {
                        _pontosPai.PontosAdicionar(item);
                    }
                    _pontos.PontosRemoverTodos();
                    _pontosPai.DesenharPonto();
                } else {
                    foreach (var item in _pontos.pontosLista)
                    {
                        _pontosPai.PontosAdicionar(item);
                    }
                    _pontos.PontosRemoverTodos();
                }
            }
            else
            {
                SelecionarVertice(x, y);
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            var x = (e.Position.X / 2);
            var y = 600 - (e.Position.Y / 2);

            if (_currentPonto != null)
            {
                _currentPonto.X = x;
                _currentPonto.Y = y;
            }

            if (_mode == 1 || _mode == 3)
            {
                //grava posicoes anteriores do mouse para escala
                if (_mouseEscala == null)
                    _mouseEscala = new Ponto4D(x,y);

                //grava posicoes anteriores do mouse para translacao
                if (_mouse == null) {
                    _mouse = new Ponto4D(x,y);

                    _pontos.BBox.ProcessarCentro();
                    var MouseX = (_pontos.BBox.ObterCentro().X - x)*-1;
                    var MouseY = (_pontos.BBox.ObterCentro().Y - y)*-1;
                    _pontos.TranslacaoXY(MouseX, MouseY);
                }

                var valorX = (_mouse.X - x)*-1;
                var valorY = (_mouse.Y - y)*-1;

                if (_tmode == 0) {
                    _pontos.TranslacaoXY(valorX, valorY);
                } else if (_tmode == 1) {
                    _pontos.RotacaoZBBox(valorX);
                } else if (_tmode == 2) {
                    var valorMultiplicacao = 1.5;
                    if (x != _mouse.X) {
                        if (x > _mouse.X)
                            _pontos.EscalaXYBBox(1/valorMultiplicacao);
                        else
                            _pontos.EscalaXYBBox(1*valorMultiplicacao);
                    }

                }
                _mouse.X = x;
                _mouse.Y = y;
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
                    _pontos.TranslacaoXY(10, 0);
                    break;
                case Key.Right:
                    _pontos.TranslacaoXY(-10, 0);
                    break;
                case Key.Up:
                    _pontos.TranslacaoXY(0, -10);
                    break;
                case Key.Down:
                    _pontos.TranslacaoXY(0, 10);
                    break;
                case Key.PageUp:
                    _pontos.EscalaXY(2, 2);
                    break;
                case Key.PageDown:
                    _pontos.EscalaXY(0.5, 0.5);
                    break;
                case Key.Number9:
                    _pontos.EscalaXYBBox(0.5);
                    break;
                case Key.Number0:
                    _pontos.EscalaXYBBox(2);
                    break;
                case Key.Number1:
                    _pontos.RotacaoZ(10);
                    break;
                case Key.Number2:
                    _pontos.RotacaoZ(-10);
                    break;
                case Key.Number3:
                    _pontos.RotacaoZBBox(10);
                    break;
                case Key.Number4:
                    _pontos.RotacaoZBBox(-10);
                    break;
                case Key.R:
                    _pontos.Cor[0] = _pontos.Cor[0] + 0.1;
                    break;
                case Key.G:
                    _pontos.Cor[1] = _pontos.Cor[1] + 0.1;
                    break;
                case Key.B:
                    _pontos.Cor[2] = _pontos.Cor[2] + 0.1;
                    break;
                case Key.V:
                    RemoveV();
                    break;
                case Key.P:
                    _pontos = _pontosPai;
                    _pontos.Desenhar();
                    _pontos.PontosExibirObjeto();
                    break;
                case Key.Z:
                    if (_tmode < 3)
                        _tmode = 99;
                    else
                        _tmode = 0;
                    break;
                case Key.C:
                    if (_mode == 0)
                    {
                        _mode = 1;
                    }
                    else if (_mode == 1)
                    {
                        _mode = 2;
                        ColocaRastro();
                    }
                    else if (_mode == 2)
                    {
                        _mode = 3;
                        TiraRastro();
                    }
                    else
                    {
                        _mode = 0;
                    }
                    break;
                case Key.L:
                    if (_tmode == 0)
                    {
                        _tmode = 1;
                    }
                    else if (_tmode == 1)
                    {
                        _tmode = 2;
                        _mouse = null;
                    }
                    else
                    {
                        _tmode = 0;
                    }
                    break;
                case Key.T:
                    TrocaPrimitiva();
                    break;
                case Key.N:
                    NovoObjeto();
                    break;
                case Key.X:
                    _currentPonto = null;
                    break;
                default:
                    break;
            }
        }

        public double[] Color()
        {
            Random random = new Random();
            return new double[] { random.NextDouble(), random.NextDouble(), random.NextDouble() };
        }

        public void CriarVertice()
        {
            _pontos = new ListPontos("ponto", Color(), _tiposLista[_ptTipo]);
            _pontosList.Add(_pontos);
        }

        public void SelecionarVertice(double x, double y)
        {
            Ponto4D pto_prox = new Ponto4D(999999999,999999999,0);
            double ptoi_prox = 999999999;
            foreach (ListPontos objeto in _pontosList)
            {
                foreach (Ponto4D pto in objeto.pontosLista)
                {
                    double x1 = x;
                    double y1 = y;
                    double x2 = pto.X;
                    double y2 = pto.Y;
                    double distanciaEuclidiana = Math.Sqrt(((x2-x1)*(x2-x1))+((y2-y1)*(y2-y1)));
                    if (distanciaEuclidiana < ptoi_prox) {
                        pto_prox = pto;
                        ptoi_prox = distanciaEuclidiana;
                        _pontos = objeto;
                    }
                }
            }
            _currentPonto = pto_prox;
        }

        public void NovoObjeto()
        {
            TiraRastro();
            CriarVertice();
        }

        public void TiraRastro()
        {
            _currentPonto = null;
            _lastPonto = _currentPonto;
            if (_pontos.pontosLista.Count > 0)
                _pontos.pontosLista.RemoveAt(_pontos.pontosLista.Count - 1);
        }

        public void RemoveV()
        {
            for (int i = 0; i < _pontos.pontosLista.Count; i++)
            {
                if (_pontos.pontosLista[i].X == _currentPonto.X && _pontos.pontosLista[i].Y == _currentPonto.Y)
                    _pontos.pontosLista.RemoveAt(i);
            }
            TiraRastro();
        }

        public void ColocaRastro()
        {
            _currentPonto = new Ponto4D(300, 300, 0, 1);
            _pontos.PontosAdicionar(_currentPonto);
        }

        public void SelecionarPoligono(int x, int yi)
        {
            int count = 0;

            foreach (ListPontos item in _pontosList)
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
                    _pontos = item;
                    break;
                }
            }
        }

        public void TrocaPrimitiva()
        {
            _ptTipo = _ptTipo >= 9? 0: _ptTipo++;
        }

        private int GetXEventMouse(MouseButtonEventArgs e) => (e.Position.X / 2);

        private int GetYEventMouse(MouseButtonEventArgs e) => 600 - (e.Position.Y / 2);
    }
}
