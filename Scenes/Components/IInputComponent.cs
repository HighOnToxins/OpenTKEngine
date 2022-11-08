
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKEngine.Scenes.Components;

public interface IInputComponent {

    public void Update(FrameEventArgs obj, GameWindow window);

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
