using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BrokenEngine.Engine
{
    public class GameWindowSettings {

        public static readonly GameWindowSettings Default = new GameWindowSettings();

        public NativeWindowSettings nativeWindowSettings { get; set; } = NativeWindowSettings.Default;
        public OpenTK.Windowing.Desktop.GameWindowSettings gameWindowSettings { get; set; } = OpenTK.Windowing.Desktop.GameWindowSettings.Default;

    }
}
