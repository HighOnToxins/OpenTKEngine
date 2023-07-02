
using OpenTK.Mathematics;
using System.Diagnostics;

namespace OpenTKEngine.Utility.Physics;

public static class CollisionManager2D
{
    public static void Collide(IReadOnlyList<IPhysicsEntity2D> entities)
    {
        for(int i = 0; i < entities.Count; i++)
        {
            for(int j = i+1; j < entities.Count; j++)
            {
                if(OverlapsRect(entities[i], entities[j], out Vector2 overlap))
                {
                    Collide(entities[i], entities[j], overlap);
                }
            }
        }
    }

    public static void Collide(IPhysicsEntity2D entityA, IPhysicsEntity2D entityB, Vector2 overlap) 
    {
        Vector2 overlapTime = (overlap / (entityA.Velocity - entityB.Velocity)).Select(f => Util.LerpEndZero(float.IsRealNumber(f), Math.Abs(f)));
        
        Vector2 projectedEntity1Velocity = entityA.Velocity.Proj(overlap);
        Vector2 projectedEntity2Velocity = entityB.Velocity.Proj(overlap);
        Vector2 factor = 2 / (entityA.Mass + entityB.Mass) * (projectedEntity2Velocity - projectedEntity1Velocity);

        Vector2 velocityADelta = entityB.Mass * factor;
        Vector2 velocityBDelta = -entityA.Mass * factor;
        entityA.Velocity += velocityADelta;
        entityB.Velocity += velocityBDelta;
        entityA.Position += overlapTime * velocityADelta;
        entityB.Position += overlapTime * velocityBDelta;
    }

    public static bool OverlapsRect(IEntity2D rectA, IEntity2D rectB, out Vector2 overlap)
    {
        Vector2 ABottomLeft = rectA.Position + rectA.Size * rectA.PositionOffset;
        Vector2 ATopRight = rectA.Position + rectA.Size * (rectA.PositionOffset + Vector2.One);
        Vector2 BBottomLeft = rectB.Position + rectB.Size * rectB.PositionOffset;
        Vector2 BTopRight = rectB.Position + rectB.Size * (rectB.PositionOffset + Vector2.One);

        Vector2 bottomLeftOverlap = new(
            ABottomLeft.X <= BBottomLeft.X && BBottomLeft.X <= ATopRight.X ? ATopRight.X - BBottomLeft.X : float.PositiveInfinity,
            ABottomLeft.Y <= BBottomLeft.Y && BBottomLeft.Y <= ATopRight.Y ? ATopRight.Y - BBottomLeft.Y : float.PositiveInfinity
        );

        Vector2 topRightOverlap = new(
            ABottomLeft.X <= BTopRight.X && BTopRight.X <= ATopRight.X ? BTopRight.X - ABottomLeft.X : float.PositiveInfinity,
            ABottomLeft.Y <= BTopRight.Y && BTopRight.Y <= ATopRight.Y ? BTopRight.Y - ABottomLeft.Y : float.PositiveInfinity
        );

        Vector2 totalOverlap = new(
            Math.Abs(bottomLeftOverlap.X) <= Math.Abs(topRightOverlap.X) ? bottomLeftOverlap.X : topRightOverlap.X,
            Math.Abs(bottomLeftOverlap.Y) <= Math.Abs(topRightOverlap.Y) ? bottomLeftOverlap.Y : topRightOverlap.Y
        );

        overlap = new(
            Math.Abs(totalOverlap.X) <= Math.Abs(totalOverlap.Y) ? totalOverlap.X : 0,
            Math.Abs(totalOverlap.Y) <= Math.Abs(totalOverlap.X) ? totalOverlap.Y : 0
        );

        return totalOverlap.X != 0 && totalOverlap.Y != 0 && totalOverlap.X != float.PositiveInfinity && totalOverlap.Y != float.PositiveInfinity;
    }
}
