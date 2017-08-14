using System;
using System.Runtime.InteropServices;

namespace BrokenEngine.Engine
{
    public class WindowsEngineWrapper : EngineWrapper
    {

        public override void SetupConsoleWindow(WindowSettings settings, bool hide)
        {
            if (settings.PosX > 0 && settings.PosY > 0 && settings.Width > 0 && 0 > settings.Height)
                SetWindowPos(Handle, IntPtr.Zero, settings.PosX, settings.PosY, settings.Width, settings.Height, SWP_NOZORDER | SWP_NOACTIVATE);
            ShowWindow(Handle, hide ? SW_HIDE : SW_SHOW);
            if (!string.IsNullOrEmpty(settings.Title))
            Console.Title = settings.Title;
        }

        #region Windows Console Handle
        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        public static IntPtr Handle => GetConsoleWindow();
        #endregion

    }
}