using System.Drawing;

namespace CrossCutting
{
    public interface IComponentVisual<T>
    {
        T WithColor(Color color);
        T WithColor(double red, double green, double blue);
        T WithSize(float size);
    }
}
