
using OpenTKMiniEngine.Rendering.Handles;
using OpenTKMiniEngine.Rendering.Instances;
using OpenTKMiniEngine.Rendering.Meshes;
using OpenTKMiniEngine.Rendering.Vertices;

namespace OpenTKMiniEngine.Rendering;

public class ElementMeshRenderer<V, I> : MeshRenderer<V, I> where V : unmanaged, IVertex where I : IInstance {
	
	public ElementMeshRenderer(ElementMesh<V> mesh, List<I> instances, RenderingInfo renderingInfo) : base(mesh, instances, renderingInfo) {
		ElementBuffer elementBuffer = new("", mesh.Elements);
		VertexBuffers.Add(elementBuffer);
	}

}
