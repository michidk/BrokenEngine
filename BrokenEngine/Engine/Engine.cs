using System;
using System.Drawing;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.SceneGraph;
using BrokenEngine.Utils.Attributes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace BrokenEngine.Engine
{
    public class Engine : GameWindow
    {

        public Camera CurrentCamera { get { return CurrentScene.MainCamera; }} //TODO: find a fix for that
        public Scene CurrentScene { get; set; }


        public Engine(EngineWrapper.WindowSettings settings, bool fullscreen) : this(settings.PosX, settings.PosY, settings.Width, settings.Height, fullscreen, settings.Title)
        {
            Globals.Engine = this;
        }

        public Engine(int posX, int posY, int width, int height, bool fullscreen, string title) : base(
            width, height, GraphicsMode.Default, title,         // Engine settings
            fullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default, 
            DisplayDevice.Default,                              // unimportant stuff
            4, 3, GraphicsContextFlags.ForwardCompatible        // opengl version
            )
        {
            if (posX > 0)
                this.X = posX;
            if (posY > 0)
                this.Y = posY;

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
            //SceneGraph = new GameObject("root", Vector3.Zero);
            BuildTestScene();

            // init logic
            Globals.Logger.Debug("Starting Engine Logic");
            //SceneGraph.Start();
            
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
            //Scene.GenerateTestSceneFile();

            Scene.GenerateTestSceneFile();

            //CurrentScene = Scene.CreateHardcodedScene();
            CurrentScene = Scene.LoadScene("TestScene");
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CurrentCamera.Resize(Width, Height);

            base.OnResize(e);
        }

        // called every frame, Engine logic
        [Bullshit(Reason = "make a global manager gameobject, which handles the PolygonMode stuff")]
        protected void OnUpdate(object sender, FrameEventArgs e)
        {
            if (Keyboard[Key.Q]) 
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            else
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            CurrentScene.SceneRoot.Update((float) e.Time);
        }

        protected void OnRender(object sender, FrameEventArgs e)
        {
            // Clear the back buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            CurrentCamera.Render(CurrentScene.SceneRoot);

            // TODO: implement image effects: http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-14-render-to-texture/

            // swap backbuffer
            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            // destroy whole scene graph
            CurrentScene.SceneRoot.Destroy();
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
            Globals.Engine = null;

            base.Dispose();
        }

    }
}