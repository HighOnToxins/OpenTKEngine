
using OpenTKMiniEngine.Rendering.Vertices;

namespace OpenTKMiniEngine.Rendering.Meshes;

public class Mesh<V> where V : IVertex {

	public readonly V[] Vertices;

	public Mesh(params V[] vertices) {
		Vertices = vertices;
	}
}
