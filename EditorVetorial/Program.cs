using System;

namespace EditorVetorial
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new World(600, 600)
                    .WithTitle("Unidade 3")
                    .Run(1.0 / 60.0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
