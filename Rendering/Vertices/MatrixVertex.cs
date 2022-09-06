
using OpenTK.Mathematics;

namespace OpenTKMiniEngine.Rendering.Vertices;

public readonly struct MatrixVertex : IVertex {

	public readonly Matrix4 Matrix;

	public MatrixVertex(Matrix4 matrix) {
		Matrix = matrix;
	}

	public VertexAttribs GetAttribs() =>
		new(typeof(float), 4, 4, 4, 4);
}
