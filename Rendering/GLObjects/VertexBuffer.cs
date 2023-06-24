﻿
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Utility;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OpenTKEngine.Rendering.GLObjects;

/// <summary> An array over a number of values, with a structure. </summary>
public interface IBuffer
{
    public int ValueCount { get; }

    public int ElementValueCount { get; }

    public AttributeType[] AttributeTypes { get; }

    public VertexAttribPointerType ValueType { get; } //TODO: Consider using type class instead such that it is easier to get size, by Marshal.Sizeof().

    public void Bind();

    public void Dispose();
}

public class VertexBuffer<T>: GLObject, IBuffer where T : unmanaged
{
    private readonly BufferHandle bufferHandle;
    private readonly BufferTargetARB target;

    public VertexBuffer(BufferTargetARB target = BufferTargetARB.ArrayBuffer)
    {
        this.target = target;
        bufferHandle = GL.GenBuffer();

        try
        {
            AttributeTypes = new AttributeType[] { Util.TypeToAttributeType(typeof(T)) };
            ValueType = Util.TypeToPointerType(typeof(T));
            ElementValueCount = Util.ValueCount(AttributeTypes[0]);
        }
        catch
        {
            //if type is not a struct
            if(!typeof(T).IsValueType || typeof(T).IsEnum) throw;

            ReadTypesFromStruct(out AttributeType[] attributeTypes, out VertexAttribPointerType valueType, out int elementSize);
            AttributeTypes = attributeTypes;
            ValueType = valueType;
            ElementValueCount = elementSize;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReadTypesFromStruct(out AttributeType[] attributeTypes, out VertexAttribPointerType valueType, out int elementSize)
    {
        Type[] fieldTypes = typeof(T)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Select(f => f.FieldType)
            .ToArray();

        if(fieldTypes.Length == 0)
        {
            throw new ArgumentException("The struct given as a generic type did not contain any data.");
        }

        elementSize = 0;
        attributeTypes = new AttributeType[fieldTypes.Length];
        valueType = Util.TypeToPointerType(fieldTypes[0]);
        for(int i = 0; i < fieldTypes.Length; i++)
        {
            attributeTypes[i] = Util.TypeToAttributeType(fieldTypes[i]);
            VertexAttribPointerType pointerType = Util.TypeToPointerType(fieldTypes[i]);
            elementSize += Util.ValueCount(attributeTypes[i]);

            if(valueType != pointerType)
            {
                throw new ArgumentException($"The value type {pointerType} deviated from {valueType}. All value types of a given struct must be the same.");
            }
        }
    }

    public VertexBuffer(BufferTargetARB target, params T[] data) : this(target)
    {
        SetData(data);
    }

    public VertexBuffer(params T[] data) : this(BufferTargetARB.ArrayBuffer, data)
    {
    }

    public int ValueCount { get; private set; }

    public int ElementValueCount { get; private init; }

    public AttributeType[] AttributeTypes { get; private init; }

    public VertexAttribPointerType ValueType { get; private init; }

    public override ObjectIdentifier Identifier => ObjectIdentifier.VertexArray;

    protected override uint Handle => (uint)bufferHandle.Handle;

    public void SetData(params T[] data) 
    {
        Bind();
        GL.BufferData(target, data, BufferUsageARB.StaticDraw); //TODO: Figure out the difference of usage.
        ValueCount = data.Length * ElementValueCount;
    }

    public override void Bind()
    {
        GL.BindBuffer(target, bufferHandle);
    }

    public override void Dispose()
    {
        GL.DeleteBuffer(bufferHandle);
    }
}
