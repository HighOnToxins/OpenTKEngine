
using OpenTKMiniEngine.Rendering.Vertices;

namespace OpenTKMiniEngine.Rendering.Meshes;

public class ElementMesh<V> : Mesh<V> where V : IVertex {

	public readonly uint[] Elements;

	public ElementMesh(V[] vertices, params uint[] elements) : base(vertices){
		Elements = elements;
	}
}
