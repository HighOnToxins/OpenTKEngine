
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering.Meshes;
using OpenTKEngine.Scenes.Components;

namespace OpenTKEngine.Rendering.Renderers.Devices;

public class RenderingDevice : IRenderingComponent {

	//constants
	public const string MeshFieldsLabel = "meshField";

	//propterties
	protected Mesh Mesh { get; private init; }
	protected ShaderProgram ShaderProgram { get; private init; }
	protected VertexArray VertexArray { get; private init; }

	//constructor
	public RenderingDevice(Mesh mesh, ShaderProgram shaderProgram) {
		ShaderProgram = shaderProgram;
		VertexArray = mesh.CreateVertexArray(shaderProgram.GetLocationsFromLabel(MeshFieldsLabel));

		Mesh = mesh;
	}

	/// <summary> An update happening before any draw method is called. </summary>
	public void RenderUpdate(FrameEventArgs obj, GameWindow win) { }

	protected virtual void Draw() {
		if(Mesh.HasNoElements) {
			GL.DrawArrays(PrimitiveType.Triangles, 0, Mesh.VertexCount);
		} else {
			GL.DrawElements(PrimitiveType.Triangles, Mesh.ElementCount, DrawElementsType.UnsignedInt, 0); 
		}
	}

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
