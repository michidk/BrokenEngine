using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BrokenEngine.Materials;

namespace BrokenEngine
{
    public class Scene
    {

        public struct MetaData
        {
            public string Name;
            public string Author;
            public string Description;
        }

        [XmlRoot("Material")]
        public class InstancedMaterial
        {
            public string Name;
            [XmlElement("Properties")]
            public Material Material;
        }

        [XmlElement("MetaData")]
        public MetaData Meta { get; set; }
        public List<InstancedMaterial> Materials { get; set; }
        //public GameObject SceneGraph;

        public static Scene LoadSceneFromName(string name)
        {
            var stream = ResourceManager.GetStream($"Scenes/{ name }.xml");
            var xmlRead = new XmlSerializer(typeof(Scene));
            return xmlRead.Deserialize(stream) as Scene;
        }

    }
}
