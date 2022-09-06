
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKMiniEngine.Rendering.Handles;
using OpenTKMiniEngine.Rendering.Instances;
using OpenTKMiniEngine.Rendering.Meshes;
using OpenTKMiniEngine.Rendering.Vertices;

namespace OpenTKMiniEngine.Rendering;

public class MeshRenderer<V, I> : Renderer where V : unmanaged, IVertex where I : IInstance {

	public record RenderingInfo(
		string VertPath, string FragPath,
		MatrixVariableNames MatrixVariableNames,
		params string[] VertexVariableNames);

	public record MatrixVariableNames(string MatrixVector0, string MatrixVector1, string MatrixVector2, string MatrixVector3);

	private readonly Mesh<V> _mesh;
	private readonly List<I> _instances;

	private readonly string[] _vertexVariableNames;
	private readonly MatrixVariableNames _matrixVariableNames;

	public MeshRenderer(Mesh<V> mesh, List<I> instances, RenderingInfo renderingInfo) : 
			base(new ShaderProgram.ShaderInfo(renderingInfo.VertPath, renderingInfo.FragPath)) {
		_mesh = mesh;
		_instances = instances;

		_matrixVariableNames = renderingInfo.MatrixVariableNames;
		_vertexVariableNames = renderingInfo.VertexVariableNames;
	}

	private MatrixVertex[] CollectMatrixVertices() {
		MatrixVertex[] matrixVertices = new MatrixVertex[_instances.Count];

		for(int i = 0; i < _instances.Count; i++) {
			matrixVertices[i] = new MatrixVertex(_instances[i].GetInstanceMatrix());
		}

		return matrixVertices;
	}

	private InstanceBuffer<MatrixVertex> CreateNewMatrixBuffer() =>
		new("instanceMatrixBuffer", CollectMatrixVertices(), 1, Shader,
			_matrixVariableNames.MatrixVector0,
			_matrixVariableNames.MatrixVector1,
			_matrixVariableNames.MatrixVector2,
			_matrixVariableNames.MatrixVector3);

	protected override List<VertexBuffer> LoadBuffers() => new(){
		new ArrayBuffer<V>("meshBuffer", _mesh.Vertices, Shader, _vertexVariableNames),
		 CreateNewMatrixBuffer()
	};


	public override void RenderUpdate(FrameEventArgs obj, GameWindow win) {

		int matrixBufferIndex = VertexBuffers.FindElementIndex(e => e.Name == "instanceMatrixBuffer");
		VertexBuffers[matrixBufferIndex].Dispose();
		VertexBuffers[matrixBufferIndex] = CreateNewMatrixBuffer();
		VertexArray.SetVertexBuffer(VertexBuffers[matrixBufferIndex]);

	}

	public override void Draw() {
		Renderer.DrawTrianglesInstanced(_mesh.Vertices.Length, _instances.Count);
	}


}
