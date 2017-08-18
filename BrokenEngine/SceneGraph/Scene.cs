using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using OpenTK;

namespace BrokenEngine.SceneGraph
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


        [XmlElement("MetaData")]
        public MetaData Meta { get; set; }
        public List<Material> Materials { get; set; }
        public List<GameObject> SceneGraph { get; set; }
        

        public Scene()
        {
            
            if (Materials == null) 
                Materials = new List<Material>();

            if (SceneGraph == null)
                SceneGraph = new List<GameObject>();
            
        }

        public static void GenerateTestSceneFile()
        {

            Globals.Logger.Debug("test");
            Scene scene = new Scene();
            var meta = new MetaData();
            meta.Name = "Test";
            meta.Description = "This is a test";
            meta.Author = "Me";
            scene.Meta = meta;

            var go = new GameObject("test", Vector3.One, null);
            //go.AddComponent(new MeshRenderer());
            scene.SceneGraph.Add(go);

            Globals.Logger.Debug("t2est");

            var serializer = SceneXMLConfigurator.GetSerializer();

            var writer = new StringWriter();
            serializer.Serialize(writer, scene);
            Globals.Logger.Debug(writer.GetStringBuilder().ToString());
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
                instance.LoadResources();
            }
            
            // init stuff
            

            return scene;
        }
        
    }
}