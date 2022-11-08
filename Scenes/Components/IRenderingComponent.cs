using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering;

namespace OpenTKEngine.Scenes.Components;

public interface IRenderingComponent {


    public void AssignShader(ShaderProgram shader);

    public void RenderUpdate(FrameEventArgs obj, GameWindow window);

    public void Render();

    public void Unload();

}
