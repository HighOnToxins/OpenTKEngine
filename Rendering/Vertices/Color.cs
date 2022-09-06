using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKMiniEngine.Rendering.Handles;

namespace OpenTKMiniEngine.Rendering.Vertices;

public struct Color : IVertex {
	public float r;
	public float g;
	public float b;

	public Color(float r, float g, float b) {
		this.r = r;
		this.g = g;
		this.b = b;
	}

	public Color(float value) {
		r = g = b = value;
	}

	public const int FieldSize = 3;


	public static implicit operator Color(Vector3 v) { return new Color(v.X, v.Y, v.Z); }
	public static implicit operator Vector3(Color c) { return new Vector3(c.r, c.g, c.b); }

	public VertexAttribs GetAttribs() =>
		new(typeof(float), FieldSize);

}


