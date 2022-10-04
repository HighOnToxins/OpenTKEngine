using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering.Meshes;

namespace OpenTKEngine.Rendering.Renderers.Devices;

public class MeshDevice<V> : RenderingDevice where V : unmanaged {

	//fields
	private readonly ArrayBuffer<V> _buffer;

	//properties
	protected int VerticiesPrInstance { get; init; }

	//constructor
	public MeshDevice(Mesh<V> mesh, ShaderProgram shaderProgram, params string[] vertexFieldNames) : base(shaderProgram) {
		_buffer = new(BufferTargetARB.ArrayBuffer, mesh.Vertices, BufferUsageARB.DynamicDraw);
		VertexArray.SetBuffer(_buffer, 0, ShaderProgram.GetVariableLocations(vertexFieldNames));

		VerticiesPrInstance = mesh.Vertices.Length;
	}

	//render update
	public override void RenderUpdate(FrameEventArgs obj, GameWindow win) {}

	//draw
	protected override void Draw() {
		GL.DrawArrays(PrimitiveType.Triangles, 0, VerticiesPrInstance);
	}

	public override void Unload() {
		base.Unload();
		_buffer.Dispose();
	}
}

