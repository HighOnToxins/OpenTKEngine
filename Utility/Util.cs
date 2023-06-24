
using OpenTK.Graphics.OpenGL;
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

    public static VertexAttribPointerType TypeToPointerType(Type type)
        => type.Name switch
    {
        nameof(Byte) =>   VertexAttribPointerType.UnsignedByte,
        nameof(SByte) =>  VertexAttribPointerType.Byte,
        nameof(Int16) =>  VertexAttribPointerType.Short,
        nameof(UInt16) => VertexAttribPointerType.UnsignedShort,
        nameof(Int32) =>  VertexAttribPointerType.Int,
        nameof(UInt32) => VertexAttribPointerType.UnsignedInt,
        nameof(Single) => VertexAttribPointerType.Float,
        nameof(Double) => VertexAttribPointerType.Double,
        _ => throw new NotImplementedException(),
    };

    public static VertexAttribPointerType AttributeToPointerType(AttributeType type) => type switch
    {
        AttributeType.Int => VertexAttribPointerType.Int,
        AttributeType.Float => VertexAttribPointerType.Float,
        AttributeType.Double => VertexAttribPointerType.Double,
        AttributeType.FloatVec2 => VertexAttribPointerType.Float,
        AttributeType.FloatVec3 => VertexAttribPointerType.Float,
        AttributeType.FloatVec4 => VertexAttribPointerType.Float,
        AttributeType.DoubleVec2 => VertexAttribPointerType.Double,
        AttributeType.DoubleVec3 => VertexAttribPointerType.Double,
        AttributeType.DoubleVec4 => VertexAttribPointerType.Double,
        AttributeType.FloatMat4 => VertexAttribPointerType.Float,
        AttributeType.DoubleMat4 => VertexAttribPointerType.Double,
        _ => throw new NotSupportedException("The given attribute type was not supported!"),
    };

    public static int ValueCount(AttributeType type) => type switch
    {
        AttributeType.Bool => 1,
        AttributeType.Int => 1,
        AttributeType.Float => 1,
        AttributeType.Double => 1,
        AttributeType.FloatVec2 => 2,
        AttributeType.FloatVec3 => 3,
        AttributeType.FloatVec4 => 4,
        AttributeType.DoubleVec2 => 2,
        AttributeType.DoubleVec3 => 3,
        AttributeType.DoubleVec4 => 4,
        AttributeType.FloatMat4 => 4 * 4,
        AttributeType.DoubleMat4 => 4 * 4,
        _ => throw new NotSupportedException("The given attribute type was not supported!"),
    };

    public static UniformType UniformTypeOf(this object obj) => obj switch
    {
        int =>      UniformType.Int,
        float =>    UniformType.Float,
        double =>   UniformType.Double,
        int[] =>    UniformType.IntSampler1dArray,
        Vector2 =>  UniformType.FloatVec2,
        Vector3 =>  UniformType.FloatVec3,
        Vector4 =>  UniformType.FloatVec4,
        Vector2d => UniformType.DoubleVec2,
        Vector3d => UniformType.DoubleVec3,
        Vector4d => UniformType.DoubleVec4,
        Matrix4 =>  UniformType.FloatMat4,
        Matrix4d => UniformType.DoubleMat4,
        _ => throw new NotSupportedException("The give type was not supported as a uniform."),
    };

    public static int Size(this Type type) => type.Name switch
    {
        nameof(Byte) => sizeof(byte),
        nameof(SByte) => sizeof(sbyte),
        nameof(Int16) => sizeof(short),
        nameof(UInt16) => sizeof(ushort),
        nameof(Int32) => sizeof(int),
        nameof(UInt32) => sizeof(uint),
        nameof(Single) => sizeof(float),
        nameof(Double) => sizeof(double),
        _ => throw new NotSupportedException("The given type was not supported!"),
    };
}
