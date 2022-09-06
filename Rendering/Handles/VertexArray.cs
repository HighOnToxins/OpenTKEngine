using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKMiniEngine.Rendering.Handles;

public sealed class VertexArray{

	private readonly VertexArrayHandle _vertexArrayObject;

	public VertexArray() {
		_vertexArrayObject = GL.GenVertexArray();
	}

	public void SetAttribute(Buffer buffer, 
			uint index, 
			int count, 
			VertexAttribPointerType type, 
			int stride, 
			int offset, 
			uint divisor = 0)  {

		Bind();
		buffer.Bind();

		GL.EnableVertexAttribArray(index);
		GL.VertexAttribPointer(index, count, type, false, stride, offset);
		GL.VertexAttribDivisor(index, divisor);

		buffer.Unbind();
		Unbind();
	}

	public void SetElementBuffer(Buffer elementBuffer) {
		Bind();
		elementBuffer.Bind();
		Unbind();
		elementBuffer.Unbind();
	}

	public void SetVertexBuffer(VertexBuffer buffer) {
		
		if(buffer.Target == BufferTargetARB.ElementArrayBuffer) {
			SetElementBuffer(buffer);
			return;
		}

		int offset = 0;
		for(int i = 0; i < buffer.VariableLocations.Length; i++) {
			SetAttribute(buffer,
				buffer.VariableLocations[i],
				buffer.FieldSizes[i],
				buffer.PointerType,
				buffer.Stride, 
				offset,
				buffer.Divisor);
			offset += buffer.FieldSizes[i] * buffer.TypeSize;
		}
	}

	public void SetVertexBufferList(IReadOnlyList<VertexBuffer> buffers) {
		for(int i = 0; i < buffers.Count; i++) {
			SetVertexBuffer(buffers[i]);
		}
	}

	public void Bind() {GL.BindVertexArray(_vertexArrayObject);}
	public static void Unbind() {GL.BindVertexArray(VertexArrayHandle.Zero);}
	public void Dispose() {GL.DeleteVertexArray(_vertexArrayObject);}

}

