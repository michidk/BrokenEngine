using BrokenEngine.Components;
using NLog;

namespace BrokenEngine
{
    public static class Globals
    {

        public static string GameName = "Broken Engine";
        public static Logger Logger = LogManager.GetLogger(GameName);

        public static Game Game;
        public static Camera CurrentCamera => Game.CurrentCamera;

    }
}