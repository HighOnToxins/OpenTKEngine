
using OpenTK.Mathematics;

namespace OpenTKEngine.Utility;

public interface IEntity2D
{
    public Vector2 PositionOffset { get; }

    public Vector2 Position { get; }

    public Vector2 Size { get; }

    public float Rotation { get; }
}

//TODO: Add shape-type.
