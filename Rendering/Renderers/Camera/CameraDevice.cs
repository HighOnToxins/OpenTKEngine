using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering;
using OpenTKEngine.Rendering.Renderers.Camera;
using OpenTKEngine.Scenes.Components;

namespace OpenTKMiniEngine.Rendering.Renderers.Camera;

public class CameraDevice : IRenderingComponent {

	//fields
	public const string CameraUniformLabel = "cameraUniform";

	private readonly IReadOnlyCamera _camera;
	private readonly ShaderProgram[] _shaderPrograms;

	private readonly string[] _uniformLocations;

	//constructor
	public CameraDevice(IReadOnlyCamera camera, params ShaderProgram[] shaderPrograms) {
		_camera = camera;
		_shaderPrograms = shaderPrograms;

		_uniformLocations = new string[shaderPrograms.Length];

		for(int i = 0; i < _uniformLocations.Length; i++) {
			string? name = shaderPrograms[i].GetNameFromLabel(CameraUniformLabel);
			if(name == null) {
				throw new ArgumentException($"ShaderProgram did not contain any uniform with the label {CameraUniformLabel}.");
			}

			_uniformLocations[i] = name;
		}
	}
	
	//update before render
	public void RenderUpdate(FrameEventArgs obj, GameWindow window) {
		for(int i = 0; i < _uniformLocations.Length; i++) {
			_shaderPrograms[i].SetUniform(_uniformLocations[i], _camera.World2Screen, true);
		}
	}

	//rendering
	public void Render() {}

	//unload
	public void Unload() {}
}
