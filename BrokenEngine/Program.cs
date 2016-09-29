using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BrokenEngine
{

    // T O  D O ' S:
    // TODO: improve program start: read parameters, etc..
    // TODO: Component Events: oninit, onawake, onstart
    // TODO: Load textures to gpu
    // TODO: Shade using Normal map
    // TODO: create timemanager, which keeps track of time since startup etc
    // TODO: gpu instancing
    // TODO: shader hotreloading

    // BUGS:
    // none :o

    class Program
    {

        // that stuff is needed, because on my laptop the console always hides behind the game
        #region Windows Console Handle
        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        public static IntPtr Handle
        {
            get
            {
                //Initialize();
                return GetConsoleWindow();
            }
        }

        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        public static void SetConsolePosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }
        #endregion

        /// <summary>
        /// Entry point for this program.
        /// </summary>
        /// <param name="args">
        /// - Frames per Second (float)
        /// - Resolution Width (float)
        /// - Resolution Height (float)
        /// - Fullscreen Enabled (boolean)
        /// - Console Position X (float)
        /// - Console Position Y (float)
        /// </param>
        static void Main(string[] args)
        {
            // Programm wide default settings
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture; // international formatting/conventions for strings etc

            // Parse Arguments
            // defaults
            float fps = 60.0f;
            float resX = 1280f;
            float resY = 720f;
            bool fullscreen = false;
            float consolePosX = -1;
            float consolePosY = -1;

            // TODO: parse parameters
            // parsing, on error ignore
            //float.TryParse(args);

            //SetConsolePosition(1280-500, 1500, 1000, 200);

            // TODO: also parse game window size (ref Game.cs#L34)

            // Starting the engine
            Globals.Logger.Info("Starting Broken Engine...");
            using (var game = new Game(1280, 720, false, "Broken Engine: OpenGL Test"))
            {
                //game.Run(0.0, 0.0); // 0 = infinite, to measure performance   
                game.Run(60.0, 60.0);
            }
        }
        
    }
}
