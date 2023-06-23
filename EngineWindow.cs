using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Scenes;
using OpenTKEngine.Utility;
using System.ComponentModel;
using System.Diagnostics;

namespace OpenTKEngine;

public sealed class EngineWindow
{

    private readonly NativeWindow window;
    private readonly Func<EngineWindow, IScene> sceneInit;

    private bool Running;

    public EngineWindow(Func<EngineWindow, IScene> sceneInit, string title, Vector2i size)
    {
        window = new NativeWindow(
            new NativeWindowSettings
            {
                Title = title,
                Size = size,
            }
        );

        this.sceneInit = sceneInit;
        Ratio = -1;

        window.VSync = VSyncMode.Off;

        GL.Enable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha);
        GL.Enable(EnableCap.Normalize);

        window.Resize += Resize;
    }

    public double DeltaTime { get; private set; }

    public float Ratio { get; set; }

    public Vector2 MousePosition => window.MousePosition;

    public void Run()
    {
        IScene scene = sceneInit.Invoke(this);

        window.KeyUp += scene.KeyUp;
        window.KeyDown += scene.KeyDown;
        window.MouseUp += scene.MouseUp;
        window.MouseDown += scene.MouseDown;
        window.MouseMove += scene.MouseMove;
        window.MouseWheel += scene.MouseWheel;
        window.MouseEnter += scene.MouseEnter;
        window.MouseLeave += scene.MouseLeave;
        window.Closing += scene.Closing;
        window.Closing += Closing;

        Running = true;

        long prevTime = Stopwatch.GetTimestamp();
        long thisTime;

        while(Running)
        {
            thisTime = Stopwatch.GetTimestamp();
            DeltaTime = (float)(thisTime - prevTime) / Stopwatch.Frequency;

            NativeWindow.ProcessWindowEvents(false);
            scene.Update();

            prevTime = thisTime;
        }
    }

    public void Close()
    {
        Running = false;
        window.Close();
    }

    public void SwapBuffers() => window.Context.SwapBuffers();

    private void Closing(CancelEventArgs args)
    {
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, BufferHandle.Zero);
        GL.BindVertexArray(VertexArrayHandle.Zero);
        GL.UseProgram(ProgramHandle.Zero);
        Running = false;
    }

    private void Resize(ResizeEventArgs obj)
    {
        Vector2i screenSize;
        if(Ratio <= 0)
        {
            screenSize = window.ClientSize;
        }
        else
        {
            screenSize = new((int)(Ratio * window.ClientSize.Y), 0);

            if(window.ClientSize.X > screenSize.X)
            {
                screenSize.Y = window.ClientSize.Y;
            }
            else
            {
                screenSize = new(window.ClientSize.X, (int)(window.ClientSize.X / Ratio));
            }
        }

        GL.Viewport(
            (window.ClientSize.X - screenSize.X) / 2,
            (window.ClientSize.Y - screenSize.Y) / 2,
            screenSize.X, screenSize.Y);
    }
    
    public void ToggleFullscreen()
    {
        if(window.IsFullscreen) window.WindowState = WindowState.Normal;
        else window.WindowState = WindowState.Fullscreen;
    }

    public Vector2 PixelToScreen(Vector2 pixel)
    {
        return new(
            Util.Rlerp(pixel.X, 0, window.ClientSize.X, -1, 1),
            Util.Rlerp(pixel.Y, window.ClientSize.Y, 0, -1, 1));
    }
}