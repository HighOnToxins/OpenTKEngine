
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace OpenTKEngine.Utility;

public static class Util
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Rlerp(float inter, float inStart, float inEnd, float outStart, float outEnd)
        => (inter - inEnd) * (outEnd - outStart) / (inEnd - inStart) + outEnd;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Proj(this Vector2 vec1, Vector2 vec2)
        => Vector2.Dot(vec1, vec2) / vec2.LengthSquared * vec2;
    


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ProjLength(this Vector2 vec1, Vector2 vec2)
        => Vector2.Dot(vec1, vec2) / vec2.LengthSquared * vec2.Length;

    public static AttributeType TypeToAttributeType(Type type) => type.Name switch
    {
        nameof(Int32) => AttributeType.Int,
        nameof(UInt32) => AttributeType.UnsignedInt, 
        nameof(Single) => AttributeType.Float, 
        nameof(Double) => AttributeType.Double, 
        nameof(Vector2) => AttributeType.FloatVec2, 
        nameof(Vector3) => AttributeType.FloatVec3, 
        nameof(Vector4) => AttributeType.FloatVec4, 
        nameof(Vector2d) => AttributeType.DoubleVec2, 
        nameof(Vector3d) => AttributeType.DoubleVec3, 
        nameof(Vector4d) => AttributeType.DoubleVec4, 
        nameof(Matrix4) => AttributeType.FloatMat4, 
        nameof(Matrix4d) => AttributeType.DoubleMat4, 
        nameof(TextureUnit) => AttributeType.Sampler2d,
        _ => type.IsAssignableTo(typeof(Array)) && type.GetElementType() is Type elementType 
            ? TypeToAttributeType(elementType) 
            : throw new NotSupportedException("The given attribute type was not supported!"),
    };

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
        nameof(Vector2) => VertexAttribPointerType.Float,
        nameof(Vector3) => VertexAttribPointerType.Float,
        nameof(Vector4) => VertexAttribPointerType.Float,
        nameof(Vector2d) => VertexAttribPointerType.Double,
        nameof(Vector3d) => VertexAttribPointerType.Double,
        nameof(Vector4d) => VertexAttribPointerType.Double,
        nameof(Matrix4) => VertexAttribPointerType.Float,
        nameof(Matrix4d) => VertexAttribPointerType.Double,
        _ => type.IsAssignableTo(typeof(Array)) && type.GetElementType() is Type elementType
            ? TypeToPointerType(elementType)
            : throw new NotImplementedException(),
    };

    //TODO: Add other matrices.
    public static int Size(AttributeType type) => type switch
    {
        AttributeType.Bool => 1,
        AttributeType.Int => 1,
        AttributeType.UnsignedInt => 1,
        AttributeType.Float => 1,
        AttributeType.Double => 1,
        AttributeType.FloatVec2 => 2,
        AttributeType.FloatVec3 => 3,
        AttributeType.FloatVec4 => 4,
        AttributeType.DoubleVec2 => 2,
        AttributeType.DoubleVec3 => 3,
        AttributeType.DoubleVec4 => 4,
        AttributeType.FloatMat4 => 4,
        AttributeType.DoubleMat4 => 4,
        _ => throw new NotSupportedException("The given attribute type was not supported!"),
    };

    public static int AttribCount(AttributeType type) => type switch
    {
        AttributeType.Bool => 1,
        AttributeType.Int => 1,
        AttributeType.UnsignedInt => 1,
        AttributeType.Float => 1,
        AttributeType.Double => 1,
        AttributeType.FloatVec2 => 1,
        AttributeType.FloatVec3 => 1,
        AttributeType.FloatVec4 => 1,
        AttributeType.DoubleVec2 => 1,
        AttributeType.DoubleVec3 => 1,
        AttributeType.DoubleVec4 => 1,
        AttributeType.FloatMat4 => 4,
        AttributeType.DoubleMat4 => 4,
        _ => throw new NotSupportedException("The given attribute type was not supported!"),
    };

    public static int TypeSize(VertexAttribPointerType type) => type switch
    {
        VertexAttribPointerType.Byte => sizeof(sbyte),
        VertexAttribPointerType.UnsignedByte => sizeof(byte),
        VertexAttribPointerType.Short => sizeof(short),
        VertexAttribPointerType.UnsignedShort => sizeof(ushort),
        VertexAttribPointerType.Int => sizeof(int),
        VertexAttribPointerType.UnsignedInt => sizeof(uint),
        VertexAttribPointerType.Float => sizeof(float),
        VertexAttribPointerType.Double => sizeof(double),
        _ => throw new NotSupportedException("The given type was not supported!"),
    };

    public static Vector2 Scale(float ratio)
    {
        Vector2 screenSize;
        if(ratio <= 0)
        {
            screenSize = Vector2.One;
        }
        else if(ratio <= 1)
        {
            screenSize = new(1, ratio);
        }
        else
        {
            screenSize = new(1f / ratio, 1);
        }

        return screenSize;
    }

    public static Vector2 Rescale(Vector2 size, float ratio)
    {
        if(ratio <= 0)
        {
            return size;
        }
        else if(size.X > ratio * size.Y)
        {
            return new(ratio * size.Y, size.Y);
        }
        else
        {
            return new(size.X, size.X / ratio);
        }
    }
}
