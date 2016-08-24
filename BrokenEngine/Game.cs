using System;
using System.Drawing;
using System.IO;
using BrokenEngine.Materials;
using BrokenEngine.Mesh;
using BrokenEngine.Mesh.OBJ_Parser;
using BrokenEngine.Scene_Graph;
using BrokenEngine.Scene_Graph.Components;
using Gwen.Control;
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
        public Canvas UI;

        // UI stuff
        private Gwen.Input.OpenTK input;
        private Gwen.Renderer.OpenTK renderer;
        private Gwen.Skin.Base skin;

        // to be removed
        private bool altDown = false;
        private TreeControl sceneGraphUI;

        public Game(int resX, int resY, string title) : base(
            resX, resY, GraphicsMode.Default, title, // Game settings
            GameWindowFlags.Default, DisplayDevice.Default, // unimportant stuff
            4, 3, GraphicsContextFlags.ForwardCompatible    // opengl version
            )
        {
            Globals.Game = this;

            // register events
            Keyboard.KeyDown += OnKeyDown;
            Keyboard.KeyUp += OnKeyUp;

            Mouse.ButtonDown += OnButtonDown;
            Mouse.ButtonUp += OnButtonUp;
            Mouse.Move += OnMove;
            Mouse.WheelChanged += OnWheel;

            Globals.Logger.Info($"Using OpenGL Version { GL.GetString(StringName.Version) }");
            Globals.Logger.Info("Broken Engine Initialized!");
        }

        // called when window starts running
        protected override void OnLoad(EventArgs e)
        {
            // load objects
            Globals.Logger.Debug("Loading Resources");
            Mesh.Mesh airboat = ObjParser.ParseFile("Models/airboat");
            Mesh.Mesh akm = ObjParser.ParseFile("Models/akm");
            Mesh.Mesh sphere = ObjParser.ParseFile("Models/sphere");
            Mesh.Mesh cube = ObjParser.ParseFile("Models/cube");
            Mesh.Mesh polygon = ObjParser.ParseFile("Models/polygon");
            var uiSkin = ResourceManager.GetBytes("DefaultSkin.skin");


            // init UI
            Globals.Logger.Debug("Loading UI");
            renderer = new Gwen.Renderer.OpenTK();
            using (Stream stream = new MemoryStream(uiSkin))
                skin = new Gwen.Skin.TexturedBase(renderer, stream);
            UI = new Canvas(skin);

            input = new Gwen.Input.OpenTK(this);
            input.Initialize(UI);
            
            UI.SetSize(Width, Height);
            UI.KeyboardInputEnabled = true;

            // create ui
            sceneGraphUI = new TreeControl(UI);
            sceneGraphUI.SetBounds(10, 10, 250, 300);
            BuildSceneGraphUI();


            // create scene graph
            Globals.Logger.Debug("Loading Scene");
            var go = new GameObject("test object", Vector3.One, SceneGraph);
            go.AddComponent(new MeshRenderer(MeshUtils.CreateQuad()), false);
            new GameObject("Test", Vector3.Zero, go);
            new GameObject("Test 2", new Vector3(0, 4, -10), go).AddComponent(new MeshRenderer(MeshUtils.CreateTriangle()), false);
            new GameObject("Coordinate Origin", new Vector3(-15, -15, -15), go).AddComponent(new MeshRenderer(MeshUtils.CreateCoordinateOrigin()), false);
            go = new GameObject("Test 4", new Vector3(12, 0, 0), go);
            go.AddComponent(new MeshRenderer(cube), false);
            new GameObject("Test 5", new Vector3(-5, 0, 0), go).AddComponent(new MeshRenderer(MeshUtils.CreateCube()), false);

            go = new GameObject("Model", new Vector3(15, 15, 10), SceneGraph);
            go.AddComponent(new MeshRenderer(sphere));
            //go.AddComponent(MeshRenderer.CreateTestTriangle(), false);

            go = new GameObject("Model 2", new Vector3(5, 0, 0), SceneGraph);
            go.AddComponent(new MeshRenderer(polygon), false);

            go = new GameObject("AKM", new Vector3(0, -5, -10), SceneGraph);
            //go.AddComponent(new MeshRenderer(airboat), false);
            go.AddComponent(new MeshRenderer(akm, new PhongMaterial(Color.SaddleBrown)), false);

            var cameraObj = new GameObject("Camera", new Vector3(0, 0, 5), SceneGraph);
            // TODO: move following camera initialiion things to camera settings
            float aspectRatio = ClientSize.Width / (float)(ClientSize.Height);
            float fov = MathHelper.PiOver2;
            var camera = new Camera(Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100f));
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

            //GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);    // backface culling
            //GL.Enable(EnableCap.FramebufferSrgb);   // gamma correction -> turned off, because we dont want gamma correction over ui etc
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);    // wire frame
            GL.ClearColor(Color4.AliceBlue);
            GL.FrontFace(FrontFaceDirection.Ccw);  // Ccw = Counter-clockwise = default = right hand rule
            //GL.ShadeModel(ShadingModel.Flat);     // Flat = flat shading; not suppoted in GL4
            Globals.Logger.Info("Broken Engine Successfully Initialized!");
        }

        public void BuildSceneGraphUI()
        {
            sceneGraphUI.RemoveAll();

            foreach (var child in SceneGraph)
            {
                SubBuildSceneGraphUI(child, sceneGraphUI.AddNode(child.Name));
            }
        }

        private void SubBuildSceneGraphUI(GameObject go, TreeNode node)
        {
            foreach (var child in go)
            {
                SubBuildSceneGraphUI(child, node.AddNode(child.Name));
            }

            foreach (var comp in go.Components)
            {
                node.AddNode(comp.GetType().Name +"");
            }
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, this.Width, this.Height);

            base.OnResize(e);
        }

        // called every frame, game logic
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.Q])
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            else
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            /*
            if (Keyboard[Key.E])
                GL.ShadeModel(ShadingModel.Flat);
            else
                GL.ShadeModel(ShadingModel.Smooth);*/

            SceneGraph.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Clear the back buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CurrentCamera.Render(SceneGraph);

            // TODO: implement image effects: http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-14-render-to-texture/

            // render UI
            GL.FrontFace(FrontFaceDirection.Cw);
            UI.RenderCanvas();
            GL.FrontFace(FrontFaceDirection.Ccw);

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

            input.ProcessKeyDown(e);
        }

        private void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            altDown = false;
            input.ProcessKeyUp(e);
        }

        private void OnButtonDown(object sender, MouseButtonEventArgs args)
        {
            input.ProcessMouseMessage(args);
        }

        private void OnButtonUp(object sender, MouseButtonEventArgs args)
        {
            input.ProcessMouseMessage(args);
        }

        private void OnMove(object sender, MouseMoveEventArgs args)
        {
            input.ProcessMouseMessage(args);
        }

        private void OnWheel(object sender, MouseWheelEventArgs args)
        {
            input.ProcessMouseMessage(args);
        }

        public override void Dispose()
        {
            UI.Dispose();
            skin.Dispose();
            renderer.Dispose();

            Globals.Game = null;

            base.Dispose();
        }

    }
}