using System.Collections.Generic;
using System.Xml.Serialization;
using BrokenEngine.Materials;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Types;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using OpenTK.Graphics;

namespace BrokenEngine.Scene
{
    [XmlRoot("Scene")]
    public class Scene
    {
        
        [XmlRoot]
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
        
        public static Scene LoadScene(string name)
        {
            
            // load the scene file
            var file = ResourceManager.GetString($"Scenes/{ name }.xml");

            // get the pre-configured serializer
            var serializer = SceneXMLConfigurator.GetSerializer();

            // parse xml scene file
            var scene = serializer.Deserialize<Scene>(file);

            // load referenced resources
            foreach (var instance in scene.Materials)
            {
               instance.Material.Initialize(instance.Shader);
            }

            // init stuff
            

            return scene;
        }
        
    }
}
