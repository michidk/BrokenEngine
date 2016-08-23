using System;
using System.Globalization;

namespace BrokenEngine
{

    // T O  D O ' S:
    // 

    // NOTES:
    // https://github.com/opentk/opentk/blob/master/Source/Examples/OpenGL/3.x/HelloGL3.cs

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
