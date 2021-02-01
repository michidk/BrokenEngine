using System.Xml.Serialization;
using BrokenEngine;
using BrokenEngine.Utils;

namespace Example
{
    public class Config
    {
        private const string FILE_PATH = "config.xml";

        public int Width = 1280;
        public int Height = 720;
        public bool Fullscreen = false;


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
