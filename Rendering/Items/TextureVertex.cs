using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Items;

public struct TextureVertex {
    public Vector3 Position { get; init; }
    public Vector2 TexturePosition { get; init; }

    public TextureVertex(Vector3 position, Vector2 texturePosition) {
        Position = position;
        TexturePosition = texturePosition;
    }
}
