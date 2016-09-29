using System;
using System.Drawing;
using System.IO;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using BrokenEngine.Mesh;
using BrokenEngine.Mesh.Mesh_Parser;
using BrokenEngine.Utils;
using BrokenEngine.Utils.Attributes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace BrokenEngine
{
    public class Game : GameWindow
    {

        public Camera CurrentCamera { get; set; }
        public GameObject SceneGraph { get; private set; }
        

        public Game(int resX, int resY, bool fullscreen, string title) : base(
            resX, resY, GraphicsMode.Default, title,        // Game settings
            fullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default, 
            DisplayDevice.Default,                          // unimportant stuff
            4, 3, GraphicsContextFlags.ForwardCompatible    // opengl version
            )
        {
            Globals.Game = this;
            Globals.GameName = title;

            // TODO: move to constructor and make it a programm parameter
            this.X = 200;
            this.Y = 150;

            // register events
            Keyboard.KeyDown += OnKeyDown;

            UpdateFrame += OnUpdate;
            RenderFrame += OnRender;

            Globals.Logger.Info($"Using OpenGL Version { GL.GetString(StringName.Version) }");
            Globals.Logger.Info(Globals.GameName + " Initialized!");
        }

        // called when window starts running
        protected override void OnLoad(EventArgs e)
        {
            SceneGraph = new GameObject("root", Vector3.Zero);
            BuildTestScene();

            // init logic
            Globals.Logger.Debug("Starting Game Logic");
            SceneGraph.Start();
            
            // GL settings
            Globals.Logger.Debug("Apply Settings");
            // turn vsync off to measure performance by counting frames
            // this.VSync = VSyncMode.Adaptive;

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);          // backface culling
            GL.Enable(EnableCap.FramebufferSrgb);   // textures are not in linear space
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.FrontFace(FrontFaceDirection.Ccw);   // Ccw = Counter-clockwise = default = right hand rule
            GL.ClearColor(Color4.AliceBlue);

            Globals.Logger.Info("Broken Engine Successfully Initialized!");
        }

        // this scene might look a little bit odd, because there is only testing stuff in it
        private void BuildTestScene()
        {
            Globals.Logger.Debug("Loading Resources");
            Mesh.Mesh airboat = ObjParser.ParseFile("Models/airboat");
            airboat.RecalculateNormals();
            Mesh.Mesh akm = ObjParser.ParseFile("Models/akm");
            Mesh.Mesh sphere = ObjParser.ParseFile("Models/sphere");
            sphere.RecalculateNormals();
            Mesh.Mesh cube = ObjParser.ParseFile("Models/cube");
            Mesh.Mesh polygon = ObjParser.ParseFile("Models/polygon");
            Mesh.Mesh suzanne = ObjParser.ParseFile("Models/suzanne");

            Material phong = new BlinnPhongMaterial(Color.SaddleBrown, Vector3.One, Color4.AliceBlue);
            Material toon = new ToonMaterial(Color.SaddleBrown, Vector3.One, Color4.AliceBlue, 4f);
            //phong = toon;   // quick hack to replace all phong materials by the toon material

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

            var cameraObj = new GameObject("Camera", new Vector3(0, 0, 0), SceneGraph);

            var camera = new Camera(ClientSize.Width, ClientSize.Height, 60f);

            cameraObj.AddComponent(camera, false);
            cameraObj.AddComponent(new CameraMovement(CameraMovement.Type.FirstPerson), false);

            CurrentCamera = camera;
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CurrentCamera.Resize(Width, Height);

            base.OnResize(e);
        }

        // called every frame, game logic
        [Bullshit(Reason = "make a global manager gameobject, which handles the PolygonMode stuff")]
        protected void OnUpdate(object sender, FrameEventArgs e)
        {
            if (Keyboard[Key.Q]) 
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            else
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            SceneGraph.Update((float) e.Time);
        }

        protected void OnRender(object sender, FrameEventArgs e)
        {
            // Clear the back buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CurrentCamera.Render(SceneGraph);

            // TODO: implement image effects: http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-14-render-to-texture/

            // swap backbuffer
            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            // destroy whole scene graph
            SceneGraph.Destroy();
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Exit();
            else if (Keyboard[Key.AltLeft] && e.Key == Key.Enter)
                if (WindowState == WindowState.Fullscreen)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Fullscreen;
        }

        public override void Dispose()
        {
            Globals.Game = null;

            base.Dispose();
        }

    }
}