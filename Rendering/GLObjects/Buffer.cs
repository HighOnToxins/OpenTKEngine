
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

    public int VertexValueCount { get; }

    public int VertexCount { get; }

    public AttributeType[] AttributeTypes { get; }

    public VertexAttribPointerType ValueType { get; }

    public bool IsElementBuffer { get; }

    public void Bind();

    public void Dispose();
}

public sealed class Buffer<T>: GLObject, IBuffer where T : unmanaged
{
    private readonly BufferHandle bufferHandle;
    private readonly BufferTargetARB target;

    public Buffer(BufferTargetARB target = BufferTargetARB.ArrayBuffer)
    {
        this.target = target;
        bufferHandle = GL.GenBuffer();

        try
        {
            AttributeTypes = new AttributeType[] { Util.TypeToAttributeType(typeof(T)) };
            ValueType = Util.TypeToPointerType(typeof(T));
            VertexValueCount = Util.Size(AttributeTypes[0]);
        }
        catch
        {
            //if type is not a struct
            if(!typeof(T).IsValueType || typeof(T).IsEnum) throw;

            ReadTypesFromStruct(out AttributeType[] attributeTypes, out VertexAttribPointerType valueType, out int vertexValueCount);
            AttributeTypes = attributeTypes;
            ValueType = valueType;
            VertexValueCount = vertexValueCount;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReadTypesFromStruct(out AttributeType[] attributeTypes, out VertexAttribPointerType valueType, out int vertexValueCount)
    {
        Type[] fieldTypes = typeof(T)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Select(f => f.FieldType)
            .ToArray();

        if(fieldTypes.Length == 0)
        {
            throw new ArgumentException("The struct given as a generic type did not contain any data.");
        }

        vertexValueCount = 0;
        attributeTypes = new AttributeType[fieldTypes.Length];
        valueType = Util.TypeToPointerType(fieldTypes[0]);
        for(int i = 0; i < fieldTypes.Length; i++)
        {
            attributeTypes[i] = Util.TypeToAttributeType(fieldTypes[i]);
            VertexAttribPointerType pointerType = Util.TypeToPointerType(fieldTypes[i]);
            vertexValueCount += Util.Size(attributeTypes[i]);

            if(valueType != pointerType)
            {
                throw new ArgumentException($"The value type {pointerType} deviated from {valueType}. All value types of a given struct must be the same.");
            }
        }
    }

    public Buffer(BufferTargetARB target, params T[] data) : this(target)
    {
        SetData(data);
    }

    public Buffer(params T[] data) : this(BufferTargetARB.ArrayBuffer, data)
    {
    }

    public int ValueCount { get => VertexValueCount * VertexCount; }

    public int VertexValueCount { get; private init; }

    public int VertexCount { get; private set; }

    public AttributeType[] AttributeTypes { get; private init; }

    public VertexAttribPointerType ValueType { get; private init; }

    public bool IsElementBuffer { get => target == BufferTargetARB.ElementArrayBuffer; }

    public override ObjectIdentifier Identifier => ObjectIdentifier.Buffer;

    protected override uint Handle => (uint)bufferHandle.Handle;

    public void SetData(params T[] data) 
    {
        Bind();

        if(data.Length > VertexCount)
        {
            GL.BufferData(target, data, BufferUsageARB.StaticDraw); //TODO: Figure out the usage of usage.
        }
        else
        {
            GL.BufferSubData(target, 0, data);
        }

        VertexCount = data.Length;
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
