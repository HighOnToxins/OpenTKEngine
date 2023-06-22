
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace OpenTKEngine.Utility;

public static class Util
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Rlerp(float inter, float inStart, float inEnd, float outStart, float outEnd)
        => (outEnd - outStart) / (inEnd - inStart) * (inter - inEnd) + outEnd;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Proj(this Vector2 vec1, Vector2 vec2)
        => Vector2.Dot(vec1, vec2) / vec2.LengthSquared * vec2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ProjLength(this Vector2 vec1, Vector2 vec2)
        => Vector2.Dot(vec1, vec2) / vec2.LengthSquared * vec2.Length;
}
