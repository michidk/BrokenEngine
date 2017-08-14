using System.Globalization;

namespace BrokenEngine.Engine
{
    public abstract class EngineWrapper
    {

        public struct WindowSettings
        {
            public int PosX;
            public int PosY;
            public int Width;
            public int Height;
            public string Title;
        }

        public Engine Engine { get; protected set; }

        public virtual void Configure(string gameName = "Broken Engine")
        {
            Globals.GameName = gameName;
            
            // Programm wide default settings
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture; // international formatting/conventions for strings etc

            // Load config
            Globals.Logger.Info("Loading config...");
            var cfg = Config.Load();
            Globals.Config = cfg;
            Globals.Logger.Debug(cfg);
            Globals.Logger.Debug("Config loaded.");

            // Apply console position
            SetupConsoleWindow(new WindowSettings
            {
                PosX = cfg.ConsolePosX,
                PosY = cfg.ConsolePosY,
                Width = 677,
                Height = 343,
                Title = $"Broken Engine: {gameName} - Developer Console"
            }, cfg.HideConsole);

            // Configuring the game window
            SetupGameWindow(new WindowSettings
            {
                PosX = cfg.GamePosX,
                PosY = cfg.GamePosY,
                Height = cfg.Height,
                Width = cfg.Width,
                Title = $"Broken Engine: {gameName}"
            }, cfg.Fullscreen);
        }

        public virtual void Run()
        {
            var cfg = Globals.Config;

            if (Engine == null)
            {
                Globals.Logger.Error("Engine was not setup.");
                return;
            }

            // Starting the engine
            Globals.Logger.Info("Starting Broken Engine...");
            using (Engine)
            {
                //engine.Run(0.0, 0.0); // 0 = infinite, to measure performance   
                Engine.Run(cfg.FPS, cfg.FPS);
            }
        }

        public abstract void SetupConsoleWindow(WindowSettings settings, bool hide);
        public virtual void SetupGameWindow(WindowSettings settings, bool fullscreen)
        {
            Engine = new Engine(settings, fullscreen);
        }

    }
}