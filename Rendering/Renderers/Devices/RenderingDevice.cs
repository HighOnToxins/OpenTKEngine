
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Scenes.Components;
using static OpenTKEngine.Rendering.ShaderProgram;

namespace OpenTKEngine.Rendering.Renderers.Devices;

public abstract class RenderingDevice : IRenderingComponent {

	//propterties
	protected ShaderProgram ShaderProgram { get; init; }
	protected VertexArray VertexArray { get; init; }

	//constructor
	public RenderingDevice(ShaderProgram shaderProgram) {
		ShaderProgram = shaderProgram;
		VertexArray = new VertexArray();
	}

	/// <summary> An update happening before any draw method is called. </summary>
	public abstract void RenderUpdate(FrameEventArgs obj, GameWindow win);

	/// <summary> The draw method is for drawing pixels on screen. </summary>
	protected abstract void Draw();

	public virtual void Render() {
		ShaderProgram.Bind();
		VertexArray.Bind();

		Draw();

		VertexArray.Unbind();
		ShaderProgram.Unbind();
	}

	public virtual void Unload() {
		ShaderProgram.Dispose();
		VertexArray.Dispose();
	}
}
