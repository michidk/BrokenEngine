using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BrokenEngine.Materials;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace BrokenEngine
{
    //[XmlRoot(Namespace = "BrokenEngine.Scene")]
    public class Scene
    {
        
        public struct MetaData
        {
            public string Name;
            public string Author;
            public string Description;
        }

        [XmlRoot("Material")]
        public class MaterialInstance
        {
            public string Name;
            public string Shader;
            [XmlElement("Properties")]
            public Material Material;
        }

        [XmlElement("MetaData")]
        public MetaData Meta { get; set; }
        public List<MaterialInstance> Materials { get; set; }
        public List<GameObject> SceneGraph { get; set; }
        

        public Scene()
        {
        }

        /*
        public static Scene LoadSceneFromName(string name)
        {
            

            // parse xml scene file
            var file = ResourceManager.GetString($"Scenes/{ name }.xml");
            var serializer = new ConfigurationContainer().Create();
            var scene = serializer.Deserialize<Scene>(file);

            // load referenced resources
            //foreach (var instance in scene.Materials)
            {
               // instance.Material.Initialize(instance.Shader);
            }

            // init stuff
            

            return scene;
        }
        */
    }
}
