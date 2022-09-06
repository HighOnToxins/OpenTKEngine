using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKMiniEngine.Scenes;

public interface IScene {

	public void Update(FrameEventArgs obj, GameWindow window);

	public void RenderUpdate(FrameEventArgs obj, GameWindow window);

	public void Render();

	public void Unload();

	public void KeyUp(KeyboardKeyEventArgs obj);
	public void KeyDown(KeyboardKeyEventArgs obj);

	public void MouseUp(MouseButtonEventArgs obj);
	public void MouseDown(MouseButtonEventArgs obj);
	public void MouseMove(MouseMoveEventArgs obj);
	public void MouseWheel(MouseWheelEventArgs obj);
	public void MouseEnter();
	public void MouseLeave();

}
