using System;
using System.Globalization;

namespace BrokenEngine
{

    // T O  D O ' S:
    // TODO: copy resources after build
    // TODO: Fix logging
    // TODO: fix UI

    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            Globals.Logger.Info("Starting Broken Engine...");
            using (var game = new Game(1280, 720, "OpenGL Test"))
            {
                //game.Run(60.0); // 60 FPS
                game.Run(0.0, 0.0); // 0 = infinite, to measure performance   
            }
        }
    }
}
