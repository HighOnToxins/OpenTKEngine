
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Items;

public struct ColorVertex {

    public Matrix4 Matrix { get; private init; }

    public Vector4 Color { get; private init; }

    public ColorVertex(Matrix4 matrix, Vector4 color) {
        Matrix = matrix;
        Color = color;
    }

}
