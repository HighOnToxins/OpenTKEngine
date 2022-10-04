
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Vertices;

public readonly struct ColorVertex {

	public Vector3 Position { get; private init; }
	public Vector4 Color { get; private init; }

	public ColorVertex(Vector3 position, Vector4 color){
		Position = position;
		Color = color;
	}

}
