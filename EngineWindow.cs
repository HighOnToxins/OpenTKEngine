using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Scenes;
using OpenTKEngine.Utility;
using System.Diagnostics;

namespace OpenTKEngine;

public sealed class EngineWindow
{

    private readonly GameWindow window;
    private readonly IScene scene;

    public EngineWindow(IScene scene, string title, Vector2i size)
    {
        window = new GameWindow(GameWindowSettings.Default,
            new NativeWindowSettings
            {
                Title = title,
                Size = size,
            }
        );

        this.scene = scene;
        Ratio = -1;

        window.Load += Load;
    }

    public float Ratio { get; set; }

    private long _prevTime;
    private long _thisTime;
    public double DeltaTime { get; private set; }

    public void Run()
    {
        _prevTime = Stopwatch.GetTimestamp();
        _thisTime = Stopwatch.GetTimestamp();

        window.Run();
    }

    private void Load()
    {
        GL.Enable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha);
        GL.Enable(EnableCap.Normalize);

        window.Resize += Resize;
        window.RenderFrame += RenderFrame;

        window.Unload += scene.Unload;

        window.KeyUp += scene.KeyUp;
        window.KeyDown += scene.KeyDown;

        window.MouseUp += scene.MouseUp;
        window.MouseDown += scene.MouseDown;
        window.MouseMove += scene.MouseMove;

        window.MouseWheel += scene.MouseWheel;
        window.MouseEnter += scene.MouseEnter;
        window.MouseLeave += scene.MouseLeave;
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
    
    private void RenderFrame(FrameEventArgs obj)
    {
        _thisTime = Stopwatch.GetTimestamp();
        DeltaTime = (float)(_thisTime - _prevTime) / Stopwatch.Frequency;

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        scene.Update(obj, this);
        window.SwapBuffers();

        _prevTime = _thisTime;
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