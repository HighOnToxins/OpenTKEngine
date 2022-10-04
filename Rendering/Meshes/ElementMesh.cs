
namespace OpenTKEngine.Rendering.Meshes;

public class ElementMesh<V> : Mesh<V> where V : unmanaged { 

	public uint[] Elements { get; protected init; }

	public ElementMesh(V[] vertices, params uint[] elements) {
		Vertices = vertices;
		Elements = elements;
	}

	public ElementMesh(params V[] vertices) {
		Vertices = vertices;
		Elements = new uint[vertices.Length];
		for(int i = 0; i < Elements.Length; i++) {
			Elements[i] = (uint)i;
		}
	}

	public ElementMesh(Mesh<V> mesh) {
		Vertices = new V[mesh.Vertices.Length];
		mesh.Vertices.CopyTo(Vertices, 0);

		Elements = new uint[Vertices.Length];
		for(int i = 0; i < Elements.Length; i++) {
			Elements[i] = (uint)i;
		}
	}
}
