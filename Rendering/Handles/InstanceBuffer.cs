
using OpenTK.Graphics.OpenGL;
using OpenTKMiniEngine.Rendering.Vertices;

namespace OpenTKMiniEngine.Rendering.Handles;

public class InstanceBuffer<V> : VertexBuffer where V : unmanaged, IVertex {
	public InstanceBuffer(string name, V[] data, uint divisor, ShaderProgram shader, params string[] variableNames) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, data);
		Init<V>(data.Length, shader, variableNames, divisor);
	}

	public InstanceBuffer(string name, V[] data, uint divisor, params uint[] variableLocations) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, data);
		Init<V>(data.Length, variableLocations, divisor);
	}

	public InstanceBuffer(string name, uint divisor, ShaderProgram shader, params string[] variableNames) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, Array.Empty<V>());
		Init<V>(0, shader, variableNames, divisor);
	}

	public InstanceBuffer(string name, uint divisor, params uint[] variableLocations) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, Array.Empty<V>());
		Init<V>(0, variableLocations, divisor);
	}
}
