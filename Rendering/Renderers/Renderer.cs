using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Scenes.Components;

namespace OpenTKMiniEngine.Rendering.Devices.Renderers;

public class Renderer : IRenderingComponent {

	//fields
	private readonly IReadOnlyCollection<IRenderingComponent?> _renderingDevices;

	//constructor
	public Renderer(params IRenderingComponent?[] renderingDevices) {
		_renderingDevices = renderingDevices;
	}

	//update before rendering
	public void RenderUpdate(FrameEventArgs obj, GameWindow window) {
		for(int i = 0; i < _renderingDevices.Count; i++) {
			_renderingDevices.ElementAt(i)?.RenderUpdate(obj, window);
		}
	}

	//rendering
	public void Render() {
		for(int i = 0; i < _renderingDevices.Count; i++) {
			_renderingDevices.ElementAt(i)?.Render();
		}
	}

	//unload all
	public void Unload() {
		for(int i = 0; i < _renderingDevices.Count; i++) {
			_renderingDevices.ElementAt(i)?.Unload();
		}
	}
}
