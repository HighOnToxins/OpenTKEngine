using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKMiniEngine.Scenes;

namespace OpenTKMiniEngine;

public class OpenTKEngine {

	public struct EngineOptions {
		public int Width;
		public int Height;
		public string Title;

		public float Ratio;

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

	public float Ratio; //Width / Height

	public OpenTKEngine(Func<IScene> initScene, EngineOptions options) { 

		_window = new GameWindow(GameWindowSettings.Default,
			new NativeWindowSettings {
				Title = options.Title,
				Size = new Vector2i(options.Width, options.Height),
			});

		_window.Load += Load;
		_window.RenderFrame += RenderFrame;
		_window.Resize += Resize;

		Ratio = options.Ratio;
		_initScene = initScene;
	}

	public void Run() {
		_window.Run();
	}

	private void Load() {
		_scene = _initScene();

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

		GL.Clear(ClearBufferMask.ColorBufferBit);
		_scene.Render();
		_window.SwapBuffers();
	}

	public void ToggleFullscreen() {
		if(_window.IsFullscreen) _window.WindowState = WindowState.Normal;
		else _window.WindowState = WindowState.Fullscreen;
	}
}
