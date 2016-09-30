using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BrokenEngine.Utils;

namespace BrokenEngine
{
    public class Config
    {
        private const string FILE_PATH = "Config/Config.xml";

        public float FPS = 60.0f;
        public int Width = 1280;
        public int Height = 720;
        public bool Fullscreen = false;
        public int GamePosX = -1;
        public int GamePosY = -1;
        public bool HideConsole = false;
        public int ConsolePosX = -1;
        public int ConsolePosY = -1;


        public override string ToString()
        {
            return ReflectionUtils.ListFields(this);
        }

        public static Config Load()
        {
            var stream = ResourceManager.GetStream(FILE_PATH);
            var xmlRead = new XmlSerializer(typeof(Config));
            return xmlRead.Deserialize(stream) as Config;
        }

    }
}
