using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Vertices;

public struct TextureVertex {
	public Vector3 Position { get; private init; }
	public Vector2 TexturePosition { get; private init; }

	public TextureVertex(Vector3 position, Vector2 texturePosition) {
		Position = position;
		TexturePosition = texturePosition;
	}
}
