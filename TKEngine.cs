﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Scenes;

namespace OpenTKEngine;

/*
 Manage NuGet packages -> OpenTK (include prerelease [V] )

OpenTKEngine.csproj:
	<ItemGroup>
		<None Update="Shaders\**\*.*">
			<CopyToOutputDirectory>Always</CopyToOutputDirectory>
		</None>
	</ItemGroup>
 */

public class TKEngine {

    /*
		TODO: Add stuff to the engine.
		- Mesh creators/handling.
		- Shader info gathering.
		- TextureRenderer (TextureVertex, Matrix, TextureNum)
		- EntityRenderer (List<IReadonlyEntity>)
		- Scene gathering/handling.
	*/

    public readonly struct EngineOptions {
        public int Width { internal get; init; }
        public int Height { internal get; init; }
        public string Title { internal get; init; }

        public float Ratio { internal get; init; }

        public EngineOptions() {
            Width = 1200;
            Height = 900;

            Ratio = -1;

            Title = "Window Title";
        }
    }

    private readonly GameWindow _window;
    private readonly Func<IScene> _initScene;
    private IScene? _scene;

    public float Ratio; //width divided by height

    public TKEngine(Func<IScene> initScene, EngineOptions options) {

        _window = new GameWindow(GameWindowSettings.Default,
            new NativeWindowSettings {
                Title = options.Title,
                Size = new Vector2i(options.Width, options.Height),
            }
        );

        _window.Load += Load;

        Ratio = options.Ratio;
        _initScene = initScene;
    }

    public void Run() {
        _window.Run();
    }

    private void Load() {

        GL.Enable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha);
        GL.Enable(EnableCap.Normalize);

        _scene = _initScene();

        _window.Resize += Resize;
        _window.RenderFrame += RenderFrame;

        _window.Unload += _scene.Unload;

        _window.KeyUp += _scene.KeyUp;
        _window.KeyDown += _scene.KeyDown;

        _window.MouseUp += _scene.MouseUp;
        _window.MouseDown += _scene.MouseDown;
        _window.MouseMove += _scene.MouseMove;
        _window.MouseWheel += _scene.MouseWheel;
        _window.MouseEnter += _scene.MouseEnter;
        _window.MouseLeave += _scene.MouseLeave;
    }

    private void Resize(ResizeEventArgs obj) {
        Vector2i screenSize = Rescale(_window.ClientSize, Ratio);

        GL.Viewport(
            (_window.ClientSize.X - screenSize.X) / 2,
            (_window.ClientSize.Y - screenSize.Y) / 2,
            screenSize.X, screenSize.Y);
    }

    private static Vector2i Rescale(Vector2i size, float ratio) {
        if(ratio <= 0) return size;
        Vector2i resize = new((int)(ratio * size.Y), 0);

        if(size.X > resize.X) {
            resize.Y = size.Y;
        } else {
            resize = new(size.X, (int)(size.X / ratio));
        }

        return resize;
    }

    private void RenderFrame(FrameEventArgs obj) {
        if(_scene == null) return;

        _scene.Update(obj, _window);
        _scene.RenderUpdate(obj, _window);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        _scene.Render();
        _window.SwapBuffers();
    }

    public void ToggleFullscreen() {
        if(_window.IsFullscreen) _window.WindowState = WindowState.Normal;
        else _window.WindowState = WindowState.Fullscreen;
    }

    public static Vector2 PixelToScreen(Vector2 pixel, GameWindow window) {
        Vector2 screenPositionOff = (new Vector4(pixel, 0, 1) * Matrix4.CreateOrthographicOffCenter(0, window.ClientSize.X, 0, window.ClientSize.Y, -1, 1)).Xy;
        Vector2 screenPosition = new Vector2(screenPositionOff.X, -screenPositionOff.Y);
        return screenPosition;
    }
}
