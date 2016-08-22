using System;
using System.Globalization;

namespace OpenGLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            using (var game = new Game(1280, 720, "OpenGL Test"))
            {
                //game.Run(60.0); // 60 FPS
                game.Run(0.0, 0.0); // 0 = infinite, to measure performance   
            }
        }
    }
}
