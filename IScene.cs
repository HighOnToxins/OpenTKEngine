using OpenTK.Windowing.Common;

namespace OpenTKEngine;

public interface IScene
{
    void KeyDown(KeyboardKeyEventArgs args);
    void KeyUp(KeyboardKeyEventArgs args);

    void MouseDown(MouseButtonEventArgs args);
    void MouseUp(MouseButtonEventArgs args);
    void MouseMove(MouseMoveEventArgs args);
    void MouseWheel(MouseWheelEventArgs args);

    void MouseEnter();
    void MouseLeave();

    void Unload();
    void Update(FrameEventArgs obj);
}
