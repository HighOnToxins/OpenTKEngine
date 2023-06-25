using OpenTK.Windowing.Common;
using System.ComponentModel;

namespace OpenTKEngine.Scenes;

public interface IScene
{
    public void Update() { }
    
    public void KeyDown(KeyboardKeyEventArgs args) { }
    public void KeyUp(KeyboardKeyEventArgs args) { }
     
    public void MouseDown(MouseButtonEventArgs args) { }
    public void MouseUp(MouseButtonEventArgs args) { }
    public void MouseMove(MouseMoveEventArgs args) { }
    public void MouseWheel(MouseWheelEventArgs args) { }
     
    public void MouseEnter() { }
    public void MouseLeave() { }
     
    public void Closing(CancelEventArgs args) { }
}
