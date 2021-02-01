using System;
using System.Drawing;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.SceneGraph;
using BrokenEngine.Serilization;
using BrokenEngine.Utils.Attributes;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using NLog;
using System.Globalization;

namespace BrokenEngine.Engine
{
    public class GameEngine : GameWindow
    {

        public static GameEngine Instance;

        public static EngineConfiguration Config = EngineConfiguration.Default;

        public static Logger Logger = LogManager.GetLogger(Config.GameName);

        public Camera CurrentCamera { get { return CurrentScene.MainCamera; } } //TODO: find a fix for that
        public Scene CurrentScene { get; set; }

        public GameEngine(EngineConfiguration config) : base(new OpenTK.Windowing.Desktop.GameWindowSettings()
        {
            IsMultiThreaded = true
        }, new NativeWindowSettings()
        {
            Title = $"Broken Engine: {config.GameName}",
            Size = config.Size,
            IsFullscreen = config.Fullscreen,
        })

        {
            // TODO: remove later
            Instance = this;
            // TODO: improve
            Config = config;

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;  // international formatting/conventions for strings etc
            // NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("nlog.config"); // TODO: load Nlog config from BE resources

            // register events
            // Keyboard.KeyDown += OnKeyDown;

            Globals.Logger.Info($"Using OpenGL Version { GL.GetString(StringName.Version) }");
            Globals.Logger.Info(Config.GameName + " Initialized!");
        }

        // called when window starts running
        protected override void OnLoad()
        {
            //SceneGraph = new GameObject("root", Vector3.Zero);
            BuildTestScene();

            CurrentScene.SceneRoot.Initialize();

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

            // init logic
            Globals.Logger.Debug("Starting Engine Logic");
            CurrentScene.SceneRoot.Start();
            base.OnLoad();
        }

        private void BuildTestScene()
        {
            Scene.GenerateTestSceneFile();

            // Scene.GenerateTestSceneFile();

            // CurrentScene = Scene.CreateHardcodedScene();
            CurrentScene = Scene.LoadScene("TestScene");
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // TODO
            // GL.Viewport(0, 0, Width, Height);
            // CurrentCamera.Resize(Width, Height);

            base.OnResize(e);
        }

        // called every frame, Engine logic
        // [Bullshit(Reason = "make a global manager gameobject, which handles the PolygonMode stuff")]
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // if (Keyboard[Key.Q])
            //     GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            // else
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            CurrentScene.SceneRoot.Update((float) e.Time);

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Clear the back buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // CurrentCamera.Render(CurrentScene.SceneRoot); //TODO

            // TODO: implement image effects: http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-14-render-to-texture/

            // swap backbuffer
            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUnload()
        {
            // destroy whole scene graph
            CurrentScene.SceneRoot.Destroy();
            base.OnUnload();
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            // if (e.Key == Key.Escape)
            //     Exit();
            // else if (Keyboard[Key.AltLeft] && e.Key == Key.Enter)
            //     if (WindowState == WindowState.Fullscreen)
            //         WindowState = WindowState.Normal;
            //     else
            //         WindowState = WindowState.Fullscreen;
        }

        public override void Close()
        {
            base.Close();
        }

    }
}
