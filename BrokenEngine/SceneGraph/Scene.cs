using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BrokenEngine.Assets;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using BrokenEngine.Models;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.Serilization;
using BrokenEngine.Utils.Attributes;
using OpenTK;
using OpenTK.Graphics;

namespace BrokenEngine.SceneGraph
{
    public class Scene
    {
        
        public struct MetaData
        {
            public string Name;
            public string Author;
            public string Description;
        }


        public MetaData Meta { get; set; }

        //public List<Asset> Assets { get; set; } = new List<Asset>();

        public GameObject SceneRoot { get; set; } = new GameObject("Scene Root");

        public Camera MainCamera { get; set; }



        public static void GenerateTestSceneFile()
        {
            //LoadScene("TestScene");
            //return;
            Globals.Logger.Debug("test");
            Scene scene = new Scene();
            var meta = new MetaData
            {
                Name = "Test",
                Description = "This is a test",
                Author = "Me"
            };
            scene.Meta = meta;

            scene.SceneRoot.AddChild(new GameObject("Test 5", new Vector3(-5, 0, 0)).AddComponent(new MeshRenderer(new Model("a123", "Models/cube"), new Material("testssss", "Shaders/vertex_color.glsl", new VertexColorShader())), false));

            var cameraObj = new GameObject("Camera", new Vector3(0, 0, 0));
            scene.SceneRoot.AddChild(cameraObj);

            var camera = new Camera(500, 500, 60f);

            cameraObj.AddComponent(camera, false);
            cameraObj.AddComponent(new CameraMovement(CameraMovement.Type.FirstPerson), false);
            //scene.Materials.Add(new BlinnPhongShader(Color4.AliceBlue, Vector3.One, Color4.AliceBlue, true));

            String xml = SceneParser.Write(scene);

            File.WriteAllText("GeneratedScene.xml", xml);
            Globals.Logger.Debug("res: " + xml.ToString());
        }

        public static Scene LoadScene(string name)
        {
            
            // load the scene file
            var file = ResourceManager.GetString($"Scenes/{ name }.xml");
            var scene = SceneParser.Read(file);

            foreach (var child in scene.SceneRoot.Children)
            {
                if (child.Name == "Camera")
                {
                    foreach (var comp in child.Components)
                    {
                        if (comp is Camera)
                        {
                            scene.MainCamera = comp as Camera;
                            continue;
                        }
                    }
                    continue;
                }
            }

            // init all GameObjects
            scene.SceneRoot.Initialize();
            
            return scene;
        }

        public static Scene CreateHardcodedScene()
        {
            /*
            Globals.Logger.Debug("Loading Resources");
            Models.Mesh airboat = ObjParser.ParseFile("Models/airboat");
            airboat.RecalculateNormals();
            Models.Mesh akm = ObjParser.ParseFile("Models/akm");
            Models.Mesh sphere = ObjParser.ParseFile("Models/sphere");
            sphere.RecalculateNormals();
            Models.Mesh cube = ObjParser.ParseFile("Models/cube");
            Models.Mesh polygon = ObjParser.ParseFile("Models/polygon");
            Models.Mesh suzanne = ObjParser.ParseFile("Models/suzanne");

            Shader phong = new BlinnPhongShader(Color.SaddleBrown, Vector3.One, Color4.AliceBlue);
            Shader toon = new ToonShader(Color.SaddleBrown, Vector3.One, Color4.AliceBlue, 4f);
            //phong = toon;   // quick hack to replace all phong materials by the toon shader

            // create scene graph

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
    

            Scene scene = new Scene();
            scene.SceneRoot.AddChild(new GameObject("Test 5", new Vector3(-5, 0, 0)).AddComponent(new MeshRenderer(new Model("a123", "Models/cube"), new Material("testssss", new VertexColorShader())), false));
            
            var cameraObj = new GameObject("Camera", new Vector3(0, 0, 0));
            scene.SceneRoot.AddChild(cameraObj);

            var camera = new Camera(500, 500, 60f);

            cameraObj.AddComponent(camera, false);
            cameraObj.AddComponent(new CameraMovement(CameraMovement.Type.FirstPerson), false);

            scene.MainCamera = camera;
            //CurrentCamera = camera;
                    */

            return null;
        }
        
    }
}