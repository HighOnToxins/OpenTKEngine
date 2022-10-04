
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Scenes.Components;

namespace OpenTKEngine.Scenes;

public class SceneSystem : IScene {

	private readonly IInputComponent? _input;
	private readonly ILogicComponent? _logic;
	private readonly IRenderingComponent? _renderer;

	public SceneSystem(IInputComponent? input, ILogicComponent? logic, IRenderingComponent? renderer) {
		_input = input;
		_logic = logic;
		_renderer = renderer;
	}

	public void Update(FrameEventArgs obj, GameWindow window) {
		_input?.Update(obj, window);
		_logic?.Update(obj, window);
	}

	public void RenderUpdate(FrameEventArgs obj, GameWindow window) {
		_renderer?.RenderUpdate(obj, window);
	}

	public void Render() { _renderer?.Render();}

	public void Unload() {
		_input?.Unload();
		_logic?.Unload();
		_renderer?.Unload();
	}

	public void KeyDown(KeyboardKeyEventArgs obj) { _input?.KeyDown(obj);}
	public void KeyUp(KeyboardKeyEventArgs obj) { _input?.KeyUp(obj); }

	public void MouseUp(MouseButtonEventArgs obj) { _input?.MouseUp(obj); }
	public void MouseDown(MouseButtonEventArgs obj) { _input?.MouseDown(obj); }
	public void MouseMove(MouseMoveEventArgs obj) { _input?.MouseMove(obj); }
	public void MouseWheel(MouseWheelEventArgs obj) { _input?.MouseWheel(obj); }
	public void MouseEnter() {_input?.MouseEnter(); }
	public void MouseLeave() {  _input?.MouseLeave(); }

}
