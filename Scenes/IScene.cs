using OpenTK.Windowing.Common;
using System.ComponentModel;

namespace OpenTKEngine.Scenes;

public interface IScene
{
    void Update();

    void KeyDown(KeyboardKeyEventArgs args);
    void KeyUp(KeyboardKeyEventArgs args);

    void MouseDown(MouseButtonEventArgs args);
    void MouseUp(MouseButtonEventArgs args);
    void MouseMove(MouseMoveEventArgs args);
    void MouseWheel(MouseWheelEventArgs args);

    void MouseEnter();
    void MouseLeave();

    void Closing(CancelEventArgs args);
}
