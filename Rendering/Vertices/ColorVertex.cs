
namespace OpenTKMiniEngine.Rendering.Vertices;

public readonly struct ColorVertex : IVertex {
	public readonly Position position;
	public readonly Color color;

	public ColorVertex(Position p, Color c){
		position = p;
		color = c;
	}

	public VertexAttribs GetAttribs() =>
		new(typeof(float), Position.FieldSize, Color.FieldSize);

}
