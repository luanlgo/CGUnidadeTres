using OpenTK.Graphics.OpenGL;

namespace CrossCutting
{
    public class LineSegmentContext : VisualComponentContext<LineSegmentContext>
    {
        public override void Begin()
        {
            Begin(1);
        }

        public override void Begin(float size)
        {
            SetType(PrimitiveType.Lines);

            base.Begin(size);
        }
    }
}
