
using OpenTK.Mathematics;

namespace OpenTKMiniEngine.Rendering.Vertices;

public readonly struct PositionVertex : IVertex {
	public readonly Vector4 Position;

	public PositionVertex(float value ){
		Position = new Vector4(value);
	}

	public PositionVertex(float x, float y, float z, float w) {
		Position = new Vector4(x, y, z, w);
	}

	public PositionVertex(Vector4 position){
		Position = position;
	}

	public VertexAttribs GetAttribs() =>
		new(typeof(float), 4);

}
