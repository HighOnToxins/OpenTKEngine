
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Rendering.Vertices;
using System.Runtime.InteropServices;
using static OpenTKEngine.Util;

namespace OpenTKEngine.Rendering;

public sealed class VertexArray : GLObject{

	//fields
	private readonly VertexArrayHandle _vertexArrayObject;

	//properties
	public override ObjectIdentifier Identifier { get => ObjectIdentifier.VertexArray; }
	public override uint Handle { get => (uint)_vertexArrayObject.Handle; }

	//constructors
	public VertexArray() {
		_vertexArrayObject = GL.GenVertexArray();
	}

	private void SetAttribute<V>(ArrayBuffer<V> buffer, 
			uint index, int count, 
			VertexAttribPointerType type, 
			int stride, int offset, uint divisor = 0)
			where V : unmanaged{

		Bind();
		buffer.Bind();

		GL.EnableVertexAttribArray(index);
		GL.VertexAttribPointer(index, count, type, false, stride, offset);
		GL.VertexAttribDivisor(index, divisor);

		buffer.Unbind();
		Unbind();
	}

	public void SetElementBuffer<V>(ArrayBuffer<V> elementBuffer) where V : unmanaged {
		Bind();
		elementBuffer.Bind();
		Unbind();
		elementBuffer.Unbind();
	}

	public void SetBuffer<V>(ArrayBuffer<V> buffer, uint divisor, params uint[] variableLocations) where V : unmanaged{

		if(buffer.Target == BufferTargetARB.ElementArrayBuffer) {
			SetElementBuffer(buffer);
			return;
		}

		VertexAttribute attrib = Util.GetVertexAttribute<V>();

		if(variableLocations.Length != attrib.FieldSizes.Length) {
			throw new ArgumentException("The number of vertex properties did not match the given amount of variable names.\n Guessing location is not suported.");
		}

		int offset = 0;
		for(int i = 0; i < variableLocations.Length; i++) {
			SetAttribute( buffer, variableLocations[i], attrib.FieldSizes[i], attrib.PointerType, attrib.Stride, offset, divisor);
			offset += attrib.FieldSizes[i] * attrib.TypeSize;
		}
	}

	public override void Bind() => 
		GL.BindVertexArray(_vertexArrayObject);

	public static void StaticUnbind() =>
		GL.BindVertexArray(VertexArrayHandle.Zero);

	public override void Unbind() =>
		StaticUnbind();

	public override void Dispose() =>
		GL.DeleteVertexArray(_vertexArrayObject); 

}

