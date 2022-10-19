
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering;

public class VertexBuffer <V> : GLObject where V : unmanaged{

	//fields
	private readonly int _dataLength;
	protected readonly BufferHandle _vertexBufferObject;

	//properties
	public override ObjectIdentifier Identifier { get => ObjectIdentifier.Buffer; }
	public override uint Handle { get => (uint)_vertexBufferObject.Handle; }
	public BufferTargetARB Target { get; init; }

	//constructor
	public VertexBuffer(BufferTargetARB target, V[] data, BufferUsageARB usage) {
		_dataLength = data.Length;

		Target = target;
		_vertexBufferObject = GL.GenBuffer();

		BufferData(data, usage);
	}

	public void BufferData(V[] data, BufferUsageARB usage) {
		if(data.Length != _dataLength) {
			throw new ArgumentException(
			"The the size of the given " +
			"data array does not match " +
			"with the size of the buffer.");
		}

		Bind();
		GL.BufferData(Target, data, usage);
		Unbind();
	}

	public void BufferSubData(int offset, V[] data) {
		if(data.Length != _dataLength) {
			throw new ArgumentException(
			"The the size of the given " +
			"data array does not match " +
			"with the size of the buffer.");
		}

		Bind();
		GL.BufferSubData(Target, (IntPtr)offset, data);
		Unbind();
	}

	public override void Bind() =>
		GL.BindBuffer(Target, _vertexBufferObject);
	public override void Unbind() =>
		GL.BindBuffer(Target, BufferHandle.Zero);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
	public override void Dispose() =>
		GL.DeleteBuffer(_vertexBufferObject);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize

}
