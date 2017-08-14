using BrokenEngine.Components;
using NLog;

namespace BrokenEngine
{
    public static class Globals
    {

        public static string GameName = "Broken Engine";
        public static Logger Logger = LogManager.GetLogger(GameName);

        public static Config Config;
        public static Engine.Engine Engine;
        public static Camera CurrentCamera => Engine.CurrentCamera;

    }
}