
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Vertices;

public struct ColorVertex {

	public Vector3 Position { get; init; }
	public Vector4 Color { get; init; }

	public ColorVertex(Vector3 position, Vector4 color){
		Position = position;
		Color = color;
	}

}
