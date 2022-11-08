
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering;
using OpenTKEngine.Scenes.Components;

namespace OpenTKEngine.Scenes;

public class SceneSystem: IScene {

    private readonly IInputComponent? _input;
    private readonly ILogicComponent? _logic;
    private readonly IEnumerable<IRenderingComponent> _renderers;

    public SceneSystem(IInputComponent? input, ILogicComponent? logic, ShaderProgram shader, params IRenderingComponent[] renderer) {
        _input = input;
        _logic = logic;
        _renderers = renderer;

        foreach(IRenderingComponent renderingComponent in _renderers) {
            renderingComponent.AssignShader(shader);
        }
    }

    public void Update(FrameEventArgs obj, GameWindow window) {
        _input?.Update(obj, window);
        _logic?.Update(obj, window);
    }

    public void RenderUpdate(FrameEventArgs obj, GameWindow window) {
        foreach(IRenderingComponent renderer in _renderers) {
            renderer.RenderUpdate(obj, window);
        }
    }

    public void Render() {
        foreach(IRenderingComponent renderer in _renderers) {
            renderer.Render();
        }
    }

    public void Unload() {
        _input?.Unload();
        _logic?.Unload();

        foreach(IRenderingComponent renderer in _renderers) {
            renderer.Unload();
        }
    }

    public void KeyDown(KeyboardKeyEventArgs obj) { _input?.KeyDown(obj); }
    public void KeyUp(KeyboardKeyEventArgs obj) { _input?.KeyUp(obj); }

    public void MouseUp(MouseButtonEventArgs obj) { _input?.MouseUp(obj); }
    public void MouseDown(MouseButtonEventArgs obj) { _input?.MouseDown(obj); }
    public void MouseMove(MouseMoveEventArgs obj) { _input?.MouseMove(obj); }
    public void MouseWheel(MouseWheelEventArgs obj) { _input?.MouseWheel(obj); }
    public void MouseEnter() { _input?.MouseEnter(); }
    public void MouseLeave() { _input?.MouseLeave(); }

}
