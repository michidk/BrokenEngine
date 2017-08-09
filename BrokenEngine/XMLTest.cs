using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace BrokenEngine
{
    class XMLTest
    {

        public static void Test()
        {
            var serializer = new ConfigurationContainer().Create();
            var scene = new Scene();
            scene.Meta = new Scene.MetaData(){Author = "Author", Description = "Description", Name = "Name"};
    
            var xml = serializer.Serialize(scene);
            Console.WriteLine(xml);

            var obj = serializer.Deserialize<Scene>(ResourceManager.GetString($"Scenes/TestScene.xml"));


        }

    }

    class Test
    {
        public string Abc ="test";
    }
}
