
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Meshes;

public abstract class Mesh : IDisposable {

	//fields
	private readonly VertexBuffer<uint>? _elementBuffer;

	//properties
	public bool HasNoElements { get => _elementBuffer == null; }

	public int VertexCount { get; protected init; }
	public int ElementCount { get; protected init; }

	//constructor
	public Mesh(params uint[] elements) {
		ElementCount = elements.Length;

		if(ElementCount > 0) {
			_elementBuffer = new(BufferTargetARB.ElementArrayBuffer, elements, BufferUsageARB.StaticDraw);
		}
	}

	//sets properties for the vertex array
	public VertexArray CreateVertexArray(params uint[] variableLocations) {
		VertexArray vertexArray = new();
		
		SetBuffers(vertexArray, variableLocations);

		if(_elementBuffer != null) {
			vertexArray.SetElementBuffer(_elementBuffer);
		}

		return vertexArray;
	}

	protected abstract void SetBuffers(VertexArray vertexArray, params uint[] variableLocations);

	public virtual void Dispose() {
		_elementBuffer?.Dispose();
	}
}

public class Mesh<V> : Mesh where V : unmanaged {

	private readonly VertexBuffer<V> _buffer;

	public Mesh(V[] vertices, params uint[] elements) : base(elements){
		_buffer = new(BufferTargetARB.ArrayBuffer, vertices, BufferUsageARB.StaticDraw);
		VertexCount = vertices.Length;
	}

	protected override void SetBuffers(VertexArray vertexArray, params uint[] variableLocations) {
		vertexArray.SetBuffer(_buffer, 0, variableLocations);
	}

	public override void Dispose() {
		base.Dispose();
		_buffer.Dispose();
	}
}

public class Mesh<V1, V2> : Mesh where V1 : unmanaged where V2 : unmanaged {

	private readonly VertexBuffer<V1> _buffer1;
	private readonly VertexBuffer<V2> _buffer2;

	public Mesh(V1[] vertices1, V2[] vertices2, params uint[] elements) : base(elements) {
		_buffer1 = new(BufferTargetARB.ArrayBuffer, vertices1, BufferUsageARB.StaticDraw);
		_buffer2 = new(BufferTargetARB.ArrayBuffer, vertices2, BufferUsageARB.StaticDraw);

		if(vertices1.Length != vertices2.Length) {
			throw new ArgumentException("The given vertex arrays did not have the same length.");
		}

		VertexCount = vertices1.Length;
	}

	protected override void SetBuffers(VertexArray vertexArray, params uint[] variableLocations) {

		int vertex1FieldCount = 1;

		uint[] variableLocations1 = new uint[vertex1FieldCount];
		Array.Copy(variableLocations, 0, variableLocations1, 0, vertex1FieldCount);

		uint[] variableLocations2 = new uint[variableLocations.Length - vertex1FieldCount];
		Array.Copy(variableLocations, vertex1FieldCount, variableLocations2, 0, variableLocations2.Length);

		vertexArray.SetBuffer(_buffer1, 0, variableLocations1);
		vertexArray.SetBuffer(_buffer2, 0, variableLocations2);
	}

	public override void Dispose() {
		base.Dispose();
		_buffer1.Dispose();
		_buffer2.Dispose();
	}
}