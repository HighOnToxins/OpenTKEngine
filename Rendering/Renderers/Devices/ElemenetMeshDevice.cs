
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Rendering.Meshes;

namespace OpenTKEngine.Rendering.Renderers.Devices;

public class ElementMeshDevice <V> : MeshDevice<V> where V : unmanaged {

	private readonly ArrayBuffer<uint> _elementBuffer;

	//constructor
	public ElementMeshDevice(ElementMesh<V> elementMesh, ShaderProgram shaderProgram, params string[] fieldNames) : 
		base(elementMesh, shaderProgram, fieldNames) {

		_elementBuffer = new(BufferTargetARB.ElementArrayBuffer, elementMesh.Elements, BufferUsageARB.StaticDraw);
		VertexArray.SetElementBuffer(_elementBuffer);

		VerticiesPrInstance = elementMesh.Elements.Length;
	}

	//draw
	protected override void Draw() {
		GL.DrawElements(PrimitiveType.Triangles, VerticiesPrInstance, DrawElementsType.UnsignedInt, 0);
	}

}
