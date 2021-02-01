using System;
using BrokenEngine.Engine;
using OpenTK.Mathematics;

namespace Example
{
    class Program
    {
        // T O  D O ' S:
        // TODO: Component Events: oninit, onawake, onstart
        // TODO: Load textures to gpu
        // TODO: Shade using Normal map
        // TODO: create timemanager, which keeps track of time since startup etc
        // TODO: gpu instancing
        // TODO: shader hotreloading
        // TODO: xml scene loading
        // TODO: prefab system
        // TODO: better toon shading (http://prideout.net/blog/?p=22)

        public static GameEngine Engine;
        public static Config Config;

        static void Main(string[] args)
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("nlog.config");

            Console.WriteLine("Loading Config...");
            Config = Config.Load();
            Console.WriteLine(Config);
            Console.WriteLine("Config loaded.");

            Console.WriteLine("Initializing Engine...");
            Engine = new GameEngine(new EngineConfiguration()
            {
                GameName = "My Test Game",
                Size = new Vector2i(Config.Width, Config.Height),
                Fullscreen = Config.Fullscreen
            });
            Console.WriteLine("Engine initialized.");
            Engine.Run();
        }
    }
}
