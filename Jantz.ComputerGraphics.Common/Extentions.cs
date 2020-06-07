using System;

namespace Jantz.ComputerGraphics.Common
{
    public static class Extentions
    {
        public static int ToInt(this object value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }
    }
}
