
using System.Runtime.CompilerServices;

namespace OpenTKEngine.Utility;

public static class Util
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Rlerp(float inter, float inStart, float inEnd, float outStart, float outEnd)
        => (outEnd - outStart) / (inEnd - inStart) * (inter - inEnd) + outEnd;
}
