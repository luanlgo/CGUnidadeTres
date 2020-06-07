using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Jantz.ComputerGraphics.Common
{
    public interface IVisualComponent<T>
    {
        T WithColor(Color color);
        T WithColor(double red, double green, double blue);
        T WithSize(float size);
    }
}
