using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BrokenEngine.Engine
{
    public class EngineConfiguration {

        public static readonly EngineConfiguration Default = new EngineConfiguration();

        public string GameName { get; set; } = "Broken Engine";
        public Vector2i Size { get; set; } = new Vector2i(640, 360);
        public bool Fullscreen { get; set; } = false;
        public Vector2i ConsolePosition { get; set; } = new Vector2i(0, 0);
        public Vector2i ConsoleSize { get; set; } = new Vector2i(677, 343);
        public bool HideConsole { get; set; } = true;

    }
}
