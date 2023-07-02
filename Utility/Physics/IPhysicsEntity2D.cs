
using OpenTK.Mathematics;

namespace OpenTKEngine.Utility.Physics;

public interface IPhysicsEntity2D : IEntity2D
{
    public Vector2 Velocity { get; set; }

    public float Mass { get; set; }
}
