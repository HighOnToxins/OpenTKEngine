
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKMiniEngine.Rendering.Handles;

namespace OpenTKMiniEngine.Rendering.Vertices;

public struct Position : IVertex {
	public float x;
	public float y;

	public Position(float x, float y) {
		this.x = x;
		this.y = y;
	}

	public Position(float value) {
		x = y = value;
	}

	public const int FieldSize = 2;

	public VertexAttribs GetAttribs() =>
		new(typeof(float), FieldSize);

	public static implicit operator Position(Vector2 v) { return new Position(v.X, v.Y); }
	public static implicit operator Vector2(Position p) { return new Vector2(p.x, p.y); }
}

