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

// TODO: Add FrameBuffer class.
// TODO: Add SceneManager, Button, Entity, InputManager, PhysicsObject.
// TODO: Create GL call dependency optimizations.

public sealed class TKEngineWindow
{

    private readonly NativeWindow window;
    private readonly Func<TKEngineWindow, IScene> sceneInit;

    private bool Running;

    public TKEngineWindow(Func<TKEngineWindow, IScene> sceneInit, string title, Vector2i size)
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
    public double Time { get; private set; }

    public float Ratio { get; set; }

    public Vector2 MousePosition => window.MousePosition;

    public void Run()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        Resize(new ResizeEventArgs());

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
        window.Resize += _ => scene.Render();

        Stopwatch timer = Stopwatch.StartNew();

        long prevTime = timer.ElapsedTicks;
        long thisTime;

        Running = true;
        while(Running)
        {
            thisTime = timer.ElapsedTicks;
            DeltaTime = (double) (thisTime - prevTime) / Stopwatch.Frequency;
            prevTime = timer.ElapsedTicks;
            Time = (double)timer.ElapsedTicks / Stopwatch.Frequency;

            NativeWindow.ProcessWindowEvents(false);

            scene.Update();
            scene.Render();

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
        Vector2i screenSize = (Vector2i) Util.Rescale(window.ClientSize, Ratio);

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