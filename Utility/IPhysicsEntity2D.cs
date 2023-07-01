
using OpenTK.Mathematics;

namespace OpenTKEngine.Utility;

public interface IPhysicsEntity2D : IEntity2D
{
    public Vector2 Velocity { get; }

    public float Mass { get; set; }
}
