using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RRWAPI
{
    public class Generic2PointFloat
    {
        public float X { get; set; }
        public float Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Generic2PointFloat(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public Generic2PointFloat(float x, float y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
    }
    //public class WindowWatcher
    //{
    //    public event Action WindowLostFocus;
    //    public event Action WindowGotFocus;
    //    public event Action<RECT, Generic2PointFloat> OnUpdateWindowBounds;

    //    public Generic2PointFloat WindowLocation;
    //    public Generic2PointFloat WindowSize;
    //    public Process GameProcess;
    //    public WindowRectangle WindowFrame;
    //    public Generic2PointFloat ScreenMid;
    //    //public HwndObject GameWindowHwnd;

    //    private System.Threading.Timer ForegroundWindowTimer;

    //    private bool inFocus = false;
    //    private int PID_SELF = 0;
    //    //private System.Windows.Form RenderWindow;
    //    public WindowWatcher(Process gprocess)
    //    {
    //        GameProcess = gprocess;
    //        PID_SELF = Process.GetCurrentProcess().Id;
    //        //UpdateWindowInformation();

    //        //ForegroundWindowTimer.Interval = 500;
    //        //ForegroundWindowTimer.Tick += ForegroundWindowTimer_Tick;
    //        //ForegroundWindowTimer.Start();
    //    }



    //    private void UpdateWindowInformation()
    //    {
    //        GameWindowFrame = GetWindowByHandle();
    //        //GameWindowHwnd = HwndObject.GetWindowByTitle(GameProcess.MainWindowTitle);

    //        if (GameWindowFrame == CurrentWindowFrame)
    //            return;

    //        CurrentWindowFrame.Size = GameWindowFrame.Size;
    //        CurrentWindowFrame.Location = GameWindowFrame.Location;
    //        //WindowFrame.Width = GameWindowFrame.Size.Width;
    //        //WindowFrame.Height = GameWindowFrame.Size.Height;
    //        //WindowFrame.Top = GameWindowFrame.Location.Y;
    //        //WindowFrame.Left = GameWindowFrame.Location.X;
    //        //WindowSize.Width = WindowFrame.Width;
    //        //WindowSize.Height = WindowFrame.Height;
    //        //WAPI.GetWindowRect(GameProcess.MainWindowHandle, out RECT data);
    //        int h, w;
    //        w = CurrentWindowFrame.Right - CurrentWindowFrame.Left + 1;
    //        h = CurrentWindowFrame.Bottom - CurrentWindowFrame.Top + 1;
    //        ScreenMid = new Generic2PointFloat(w / 2, (h / 2) - 5);
    //        OnUpdateWindowBounds?.Invoke(CurrentWindowFrame, ScreenMid);
    //        //RenderWindow.UpdateMargins(WindowLocation, new System.Drawing.Size(GameWindowHwnd.Size.Width, GameWindowHwnd.Size.Height));
    //        //Drawing.UpdatePixelSize(newSize);
    //    }

    //    private RECT GetWindowByHandle()
    //    {
    //        WAPI.GetWindowRect(GameProcess.MainWindowHandle, out RECT windowBounds);
    //        return windowBounds;
    //    }
    //}
    //public struct Margins
    //{
    //    public int Left, Right, Top, Bottom;
    //}

    //public struct WindowRectangle
    //{
    //    public int Height;
    //    public int Width;
    //    public int Top;
    //    public int Left;
    //}
}
//[StructLayout(LayoutKind.Sequential)]
//public struct RECT
//{
//    public int Left;
//    public int Top;
//    public int Right;
//    public int Bottom;
//}

