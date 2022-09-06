using OpenTK.Graphics.OpenGL;

namespace OpenTKMiniEngine.Rendering.Handles;

public class ElementBuffer : VertexBuffer {
	public ElementBuffer(string name, uint[] data) : base(name) {
		ActivateBuffer(BufferTargetARB.ElementArrayBuffer, data);
		ElementInit(data.Length);
	}

	public ElementBuffer(string name) : base(name) {
		ActivateBuffer(BufferTargetARB.ElementArrayBuffer, Array.Empty<uint>());
		ElementInit(0);
	}
}
