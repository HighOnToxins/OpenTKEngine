using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering.Items;
using OpenTKEngine.Scenes.Components;

namespace OpenTKEngine.Rendering.Renderers;

public class CameraRenderer: IRenderingComponent {

    //fields
    public string UniformProjName = "uProj";
    public string UniformViewName = "uView";

    private readonly IReadOnlyCamera _camera;
    private readonly List<ShaderProgram> _shaderPrograms;

    private readonly List<(int proj, int view)> _uniformLocations;

    //constructor
    public CameraRenderer(IReadOnlyCamera camera) {
        _camera = camera;
        _shaderPrograms = new();

        _uniformLocations = new();
    }

    public void AssignShader(ShaderProgram shader) {
        _shaderPrograms.Add(shader);
        _uniformLocations.Add(
            (shader.GetUniformLocation(shader.UniformNames.First(n => n == UniformProjName)),
            shader.GetUniformLocation(shader.UniformNames.First(n => n == UniformViewName))));
    }

    //update before render
    public void RenderUpdate(FrameEventArgs obj, GameWindow window) {
        for(int i = 0; i < _shaderPrograms.Count; i++) {
            _shaderPrograms[i].SetUniform(_uniformLocations[i].proj, _camera.ProjMatrix, false);
            _shaderPrograms[i].SetUniform(_uniformLocations[i].view, _camera.ViewMatrix, false);
        }
    }

    //rendering
    public void Render() { }

    //unload
    public void Unload() { }

}
