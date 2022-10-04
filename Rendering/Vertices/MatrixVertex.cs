
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Vertices;

public readonly struct MatrixVertex {

	public Matrix4 Matrix { get; private init; }

	public MatrixVertex(Matrix4 matrix) {
		Matrix = matrix;
	}
}
