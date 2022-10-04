
namespace OpenTKEngine.Rendering.Meshes;

public class Mesh<V> where V : unmanaged {

	public V[] Vertices { get; protected init; }

	public Mesh(params V[] vertices) {
		Vertices = vertices;
	}
}
