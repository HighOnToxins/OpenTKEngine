using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace OpenTKMiniEngine.Rendering.Handles;

public class Buffer {

	private BufferHandle _vertexBufferObject;
	public BufferTargetARB Target { get; private set; }

	public string Name; //TODO: Add naming of buffer.

	public Buffer(string name) {
		Name = name;
	}

	public void ActivateBuffer<T>(BufferTargetARB target, T[] data) where T : unmanaged {
		Target = target;

		_vertexBufferObject = GL.GenBuffer();
		Bind();
		GL.BufferData(target, data, BufferUsageARB.StaticDraw);
		Unbind();
	}

	public virtual void UploadBufferData<T>(T[] data) where T : unmanaged {
		Bind();
		unsafe {
			fixed(T* dataPtr = data) {
				GL.BufferSubData(Target, (IntPtr)0, data.Length * Marshal.SizeOf<T>(), dataPtr); 
			}
		}
		Unbind();
	}

	public void Bind() { GL.BindBuffer(Target, _vertexBufferObject); }
	public void Unbind() { GL.BindBuffer(Target, BufferHandle.Zero); }
	public void Dispose() { GL.DeleteBuffer(_vertexBufferObject); }
}
