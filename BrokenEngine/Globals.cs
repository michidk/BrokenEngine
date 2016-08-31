using Gwen.Control;
using NLog;

namespace BrokenEngine
{
    public static class Globals
    {

        public static string GameName = "Broken Engine";

        public static Game Game;
        public static Logger Logger = LogManager.GetLogger(GameName);

    }
}