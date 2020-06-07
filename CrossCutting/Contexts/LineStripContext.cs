using OpenTK.Graphics.OpenGL;

namespace CrossCutting
{
    public class LineStripContext : VisualComponentContext<LineStripContext>
    {
        public override void Begin()
        {
            Begin(1);
        }

        public override void Begin(float size)
        {
            SetType(PrimitiveType.LineStrip);

            base.Begin(size);
        }
    }
}
