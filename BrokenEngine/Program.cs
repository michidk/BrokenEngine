using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BrokenEngine
{

    // T O  D O ' S:
    // TODO: report FrontFacing.CW only works with gwen to gwen dot net repo
    // TODO: oninit, onawake,onstart
    // TODO: Load textures to gpu
    // TODO: Shade using Normal map
    // TODO: create timemanager, which keeps track of time since startup etc
    // TODO: gpu instanciating
    // TODO: other todos

    // BUGS:
    // fix UI scaling (see github issue)
    // wrong specular highlight (wrong cam position?)

    // NOTES:
    // https://github.com/opentk/opentk/blob/master/Source/Examples/OpenGL/3.x/HelloGL3.cs

    class Program
    {

        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();


        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            SetConsolePosition(1280-500, 1500, 1000, 200);

            Globals.Logger.Info("Starting Broken Engine...");
            using (var game = new Game(1280, 720, "Broken Engine: OpenGL Test"))
            {
                //game.Run(0.0, 0.0); // 0 = infinite, to measure performance   
                game.Run(60.0, 60.0);
            }
        }
        
        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        public static void SetConsolePosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        public static IntPtr Handle
        {
            get
            {
                //Initialize();
                return GetConsoleWindow();
            }
        }
    }
}
