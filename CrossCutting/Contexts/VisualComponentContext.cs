using OpenTK.Graphics.OpenGL;
using System;

namespace CrossCutting
{
    public class VisualComponentContext<T> where T : new()
    {
        private bool HasBegun { get; set; }
        public static PrimitiveType Type { get; private set; }

        public static T NewInstance()
        {
            return new T();
        }

        public virtual void SetType(PrimitiveType type)
        {
            Type = type;
        }

        public virtual void Begin()
        {
            Begin(1);
        }

        public virtual void Begin(float size)
        {
            if (HasBegun)
                throw new InvalidOperationException("Outro contexto já foi inicializado.");

            HasBegun = true;
            DefinirTamanhos(size, Type);
            GL.Begin(Type);
        }

        private void DefinirTamanhos(float size, PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.Lines:
                    GL.LineWidth(size);
                    break;
                case PrimitiveType.Points:
                    GL.PointSize(size);
                    break;
                default:
                    break;
            }
        }

        public virtual void End()
        {
            if (!HasBegun)
                throw new InvalidOperationException("Nenhum contexto inicializado.");

            GL.End();
            HasBegun = false;
        }
    }
}
