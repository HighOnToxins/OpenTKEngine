using OpenTK.Graphics.OpenGL;
using OpenTKMiniEngine.Rendering.Vertices;
using System.Runtime.InteropServices;

namespace OpenTKMiniEngine.Rendering.Handles;

public class ArrayBuffer<V> : VertexBuffer where V : unmanaged, IVertex {
	public ArrayBuffer(string name, V[] data, ShaderProgram shader, params string[] variableNames) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, data);
		Init<V>(data.Length, shader, variableNames);
	}

	public ArrayBuffer(string name, V[] data, params uint[] variableLocations) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, data);
		Init<V>(data.Length, variableLocations);
	}

	public ArrayBuffer(string name, ShaderProgram shader, params string[] variableNames) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, Array.Empty<V>());
		Init<V>(0, shader, variableNames);
	}

	public ArrayBuffer(string name, params uint[] variableLocations) : base(name) {
		ActivateBuffer(BufferTargetARB.ArrayBuffer, Array.Empty<V>());
		Init<V>(0, variableLocations);
	}
}
