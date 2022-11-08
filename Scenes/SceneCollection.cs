
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKEngine.Scenes;

public class SceneCollection: IScene {

    private IScene[] _scenes;

    public SceneCollection(params IScene[] scenes) => _scenes = scenes;
    protected void Init(params IScene[] scenes) => _scenes = scenes;

    public void Update(FrameEventArgs obj, GameWindow window) { foreach(IScene scene in _scenes) scene.Update(obj, window); }

    public void RenderUpdate(FrameEventArgs obj, GameWindow window) { foreach(IScene scene in _scenes) scene.RenderUpdate(obj, window); }

    public void Render() { foreach(IScene scene in _scenes) scene.Render(); }

    public void Unload() { foreach(IScene scene in _scenes) scene.Unload(); }

    public void KeyUp(KeyboardKeyEventArgs obj) { foreach(IScene scene in _scenes) scene.KeyUp(obj); }

    public void KeyDown(KeyboardKeyEventArgs obj) { foreach(IScene scene in _scenes) scene.KeyDown(obj); }

    public void MouseUp(MouseButtonEventArgs obj) { foreach(IScene scene in _scenes) scene.MouseUp(obj); }

    public void MouseDown(MouseButtonEventArgs obj) { foreach(IScene scene in _scenes) scene.MouseDown(obj); }

    public void MouseMove(MouseMoveEventArgs obj) { foreach(IScene scene in _scenes) scene.MouseMove(obj); }

    public void MouseWheel(MouseWheelEventArgs obj) { foreach(IScene scene in _scenes) scene.MouseWheel(obj); }

    public void MouseEnter() { foreach(IScene scene in _scenes) scene.MouseEnter(); }

    public void MouseLeave() { foreach(IScene scene in _scenes) scene.MouseLeave(); }
}

