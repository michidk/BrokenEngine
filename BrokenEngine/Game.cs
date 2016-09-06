using System;
using System.Drawing;
using System.IO;
using BrokenEngine.Materials;
using BrokenEngine.Mesh;
using BrokenEngine.Mesh.OBJ_Parser;
using BrokenEngine.Scene_Graph;
using BrokenEngine.Scene_Graph.Components;
using BrokenEngine.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace BrokenEngine
{
    public class Game : GameWindow
    {

        public readonly GameObject SceneGraph = new GameObject("root", Vector3.Zero);
        public Camera CurrentCamera;
        
        private bool altDown = false;

        public Game(int resX, int resY, string title) : base(
            resX, resY, GraphicsMode.Default, title, // Game settings
            GameWindowFlags.Default, DisplayDevice.Default, // unimportant stuff
            4, 3, GraphicsContextFlags.ForwardCompatible    // opengl version
            )
        {
            Globals.Game = this;
            Globals.GameName = title;

            // window pos
            this.X = 200;
            this.Y = 150;

            // register events
            // UI
            Keyboard.KeyDown += OnKeyDown;
            Keyboard.KeyUp += OnKeyUp;

            UpdateFrame += OnUpdate;
            RenderFrame += OnRender;

            Globals.Logger.Info($"Using OpenGL Version { GL.GetString(StringName.Version) }");
            Globals.Logger.Info(Globals.GameName + " Initialized!");
        }

        // called when window starts running
        protected override void OnLoad(EventArgs e)
        {
            // load objects
            Globals.Logger.Debug("Loading Resources");
            Mesh.Mesh airboat = ObjParser.ParseFile("Models/airboat");
            airboat.RecalculateNormals();
            Mesh.Mesh akm = ObjParser.ParseFile("Models/akm");
            Mesh.Mesh sphere = ObjParser.ParseFile("Models/sphere");
            sphere.RecalculateNormals();
            Mesh.Mesh cube = ObjParser.ParseFile("Models/cube");
            Mesh.Mesh polygon = ObjParser.ParseFile("Models/polygon");

            // create scene graph
            Globals.Logger.Debug("Loading Scene");
            var go = new GameObject("test object", Vector3.One, SceneGraph);
            go.AddComponent(new MeshRenderer(MeshUtils.CreateQuad()), false);
            new GameObject("Test", Vector3.Zero, go);
            new GameObject("Test 2", new Vector3(0, 4, -10), go).AddComponent(new MeshRenderer(MeshUtils.CreateTriangle()), false);
            new GameObject("Coordinate Origin", new Vector3(-15, -15, -15), go).AddComponent(new MeshRenderer(MeshUtils.CreateCoordinateOrigin()), false).AddComponent(new DirectionalMovement(Vector3.One, radius: 1f));

            go = new GameObject("Test 4", new Vector3(12, 0, 0), go);
            go.AddComponent(new MeshRenderer(cube, new PhongMaterial(Color.SaddleBrown)), false);
            new GameObject("Test 5", new Vector3(-5, 0, 0), go).AddComponent(new MeshRenderer(MeshUtils.CreateCube()), false);

            go = new GameObject("Model", new Vector3(15, 15, 10), SceneGraph);
            go.AddComponent(new MeshRenderer(airboat, new PhongMaterial(Color.SaddleBrown)), false);
            //go.AddComponent(MeshRenderer.CreateTestTriangle(), false);
            
            go = new GameObject("Model 2", new Vector3(5, 0, 0), SceneGraph);
            go.AddComponent(new MeshRenderer(sphere, new PhongMaterial(Color.SaddleBrown)), false);

            go = new GameObject("AKM", new Vector3(0, -5, -10), SceneGraph);
            //go.AddComponent(new MeshRenderer(airboat), false);
            go.AddComponent(new MeshRenderer(akm, new PhongMaterial(Color.SaddleBrown)), false);
            go.AddComponent(new CircularMovement(radius:2f), false);

            var cameraObj = new GameObject("Camera", new Vector3(0, 0, 0), SceneGraph);
            
            var camera = new Camera(ClientSize.Width, ClientSize.Height, 60f);
            
            cameraObj.AddComponent(camera, false);
            cameraObj.AddComponent(new CameraMovement(CameraMovement.Type.FirstPerson), false);


            // init logic
            Globals.Logger.Debug("Starting Game Logic");
            CurrentCamera = camera;

            SceneGraph.Start();


            // GL settings
            Globals.Logger.Debug("Apply Settings");
            // turn vsync of to measure performance by counting frames
            //this.VSync = VSyncMode.Adaptive;

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);    // backface culling
            GL.Enable(EnableCap.FramebufferSrgb);   // textures are not in linear space
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);    // wire frame
            GL.ClearColor(Color4.AliceBlue);
            GL.FrontFace(FrontFaceDirection.Ccw);  // Ccw = Counter-clockwise = default = right hand rule
            //GL.ShadeModel(ShadingModel.Flat);     // Flat = flat shading; not suppoted in GL4
            Globals.Logger.Info("Broken Engine Successfully Initialized!");
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CurrentCamera.Resize(Width, Height);

            base.OnResize(e);
        }

        // called every frame, game logic
        [Bullshit]
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
            else if (e.Key == Key.AltLeft)
                altDown = true;
            else if (altDown && e.Key == Key.Enter)
                if (WindowState == WindowState.Fullscreen)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Fullscreen;
        }

        private void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            altDown = false;
        }

        public override void Dispose()
        {
            Globals.Game = null;

            base.Dispose();
        }

    }
}