
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Rendering.Meshes;

namespace OpenTKEngine.Rendering.Renderers.Devices;

public class ElementInstanceDevice <V, I> : InstanceDevice<V, I> where V : unmanaged where I : unmanaged {

	//fields
	private readonly ArrayBuffer<uint> _elementBuffer;

	//constructor
	public ElementInstanceDevice(ElementMesh<V> elementMesh, ShaderProgram shaderProgram, InstanceRenderingInfo info) : 
		base(elementMesh, shaderProgram, info) {

		_elementBuffer = new(BufferTargetARB.ElementArrayBuffer, elementMesh.Elements, BufferUsageARB.StaticDraw);
		VertexArray.SetElementBuffer(_elementBuffer);

		VerticiesPrInstance = elementMesh.Elements.Length;
	}

	//draw
	protected override void Draw() {
		GL.DrawElementsInstanced(PrimitiveType.Triangles, VerticiesPrInstance, DrawElementsType.UnsignedInt, 0, _instanceCount);
	}

}
