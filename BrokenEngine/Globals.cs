using NLog;

using BrokenEngine.Engine;
using BrokenEngine.Components;

namespace BrokenEngine
{
    public static class Globals
    {

        public static EngineConfiguration Config => GameEngine.Config;
        public static Logger Logger => GameEngine.Logger;

        public static GameEngine Engine => GameEngine.Instance;
        public static Camera CurrentCamera => Engine.CurrentCamera;

    }
}
