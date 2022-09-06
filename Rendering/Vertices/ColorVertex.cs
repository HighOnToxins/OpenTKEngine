
using OpenTK.Mathematics;

namespace OpenTKMiniEngine.Rendering.Vertices;

public readonly struct ColorVertex : IVertex {
	public readonly Vector4 Position;
	public readonly Vector4 Color;

	public ColorVertex(Vector4 position, Vector4 color){
		Position = position;
		Color = color;
	}

	public VertexAttribs GetAttribs() =>
		new(typeof(float), 4, 4);

}
