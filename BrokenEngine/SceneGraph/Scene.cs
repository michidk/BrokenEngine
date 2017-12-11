using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using BrokenEngine.Models;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.Serialization;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using OpenTK;
using OpenTK.Graphics;

namespace BrokenEngine.SceneGraph
{
    [XmlRoot]
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
        public List<Mesh> Models { get; set; }
        //public List<GameObject> SceneGraph { get; set; }
        public GameObject SceneRoot { get; set; }

        public Camera MainCamera { get; set; }

        public Scene()
        {
            
            if (Materials == null) 
                Materials = new List<Material>();

            if (Models == null)
                Models = new List<Mesh>();

            //if (SceneGraph == null)
            //    SceneGraph = new List<GameObject>();
            if (SceneRoot == null) 
                SceneRoot = new GameObject("Scene Root");
        }

        public static void GenerateTestSceneFile()
        {
            LoadScene("TestScene");
            //return;
            Globals.Logger.Debug("test");
            Scene scene = new Scene();
            var meta = new MetaData();
            meta.Name = "Test";
            meta.Description = "This is a test";
            meta.Author = "Me";
            scene.Meta = meta;

            var go = new GameObject("test", Vector3.One, null);
            //go.AddComponent(new MeshRenderer());
            //scene.SceneGraph.Add(go);
            scene.SceneRoot.AddChild(go);

            scene.Models.Add(new Mesh("test", 1, 2));

            scene.Materials.Add(new BlinnPhongMaterial(Color4.AliceBlue, Vector3.One, Color4.AliceBlue, true));

            var serializer = SceneConfigurator.GetSerializer();
            var xml = serializer.Serialize(scene);
            Globals.Logger.Debug("res: " + xml.ToString());
        }

        public static Scene LoadScene(string name)
        {
            
            // load the scene file
            var file = ResourceManager.GetString($"Scenes/{ name }.xml");

            // get the pre-configured serializer
            var serializer = SceneConfigurator.GetSerializer();

            // parse xml scene file
            var scene = serializer.Deserialize<Scene>(file);

            // load referenced resources
            foreach (var instance in scene.Materials)
            {
                //instance.LoadResources();
            }
            
            // init stuff
            

            return scene;
        }

        public static Scene CreateHardcodedScene()
        {
            Globals.Logger.Debug("Loading Resources");
            Models.Mesh airboat = ObjParser.ParseFile("Models/airboat");
            airboat.RecalculateNormals();
            Models.Mesh akm = ObjParser.ParseFile("Models/akm");
            Models.Mesh sphere = ObjParser.ParseFile("Models/sphere");
            sphere.RecalculateNormals();
            Models.Mesh cube = ObjParser.ParseFile("Models/cube");
            Models.Mesh polygon = ObjParser.ParseFile("Models/polygon");
            Models.Mesh suzanne = ObjParser.ParseFile("Models/suzanne");

            Material phong = new BlinnPhongMaterial(Color.SaddleBrown, Vector3.One, Color4.AliceBlue);
            Material toon = new ToonMaterial(Color.SaddleBrown, Vector3.One, Color4.AliceBlue, 4f);
            //phong = toon;   // quick hack to replace all phong materials by the toon material

            // create scene graph

            /*
            Globals.Logger.Debug("Loading Scene");
            var go = new GameObject("test object", Vector3.One, SceneGraph);
            go.AddComponent(new MeshRenderer(MeshUtils.CreateQuad()), false);
            new GameObject("Test", Vector3.Zero, go);
            new GameObject("Test 2", new Vector3(0, 4, -10), go).AddComponent(new MeshRenderer(MeshUtils.CreateTriangle()), false);
            new GameObject("Coordinate Origin", new Vector3(-15, -15, -15), go).AddComponent(new MeshRenderer(MeshUtils.CreateCoordinateOrigin()), false).AddComponent(new DirectionalMovement(Vector3.One, radius: 1f));

            go = new GameObject("Test 4", new Vector3(12, 0, 0), go);
            go.AddComponent(new MeshRenderer(cube, phong), false);
            new GameObject("Test 5", new Vector3(-5, 0, 0), go).AddComponent(new MeshRenderer(MeshUtils.CreateCube()), false);

            go = new GameObject("Model", new Vector3(15, 15, 10), SceneGraph);
            go.AddComponent(new MeshRenderer(airboat, phong), false);
            //go.AddComponent(MeshRenderer.CreateTestTriangle(), false);

            go = new GameObject("Model 2", new Vector3(5, 0, 0), SceneGraph);
            go.AddComponent(new MeshRenderer(sphere, phong), false);

            go = new GameObject("AKM", new Vector3(0, -5, -10), SceneGraph);
            //go.AddComponent(new MeshRenderer(airboat), false);
            go.AddComponent(new MeshRenderer(suzanne, phong), false);
            go.AddComponent(new CircularMovement(speed: 0.05f, radius: 2f), false);

            */
            Scene scene = new Scene();
            

            var cameraObj = new GameObject("Camera", new Vector3(0, 0, 0));
            scene.SceneRoot.AddChild(cameraObj);

            var camera = new Camera(500, 500, 60f);

            cameraObj.AddComponent(camera, false);
            cameraObj.AddComponent(new CameraMovement(CameraMovement.Type.FirstPerson), false);

            scene.MainCamera = camera;
            //CurrentCamera = camera;

            return scene;
        }
        
    }
}