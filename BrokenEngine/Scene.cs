using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BrokenEngine.Materials;
using ExtendedXmlSerialization;

namespace BrokenEngine
{
    [XmlRoot(Namespace = "BrokenEngine.Scene")]
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

        public static Scene LoadSceneFromName(string name)
        {
            // parse xml scene file
            var xml = ResourceManager.GetString($"Scenes/{ name }.xml");
            var serializer = new ExtendedXmlSerializer();   // we use ExtendedXml.. because we do not want to register every possible child of Material
            var scene = serializer.Deserialize<Scene>(xml);

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
