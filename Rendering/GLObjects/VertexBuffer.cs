
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Utility;
using System.Runtime.InteropServices;

namespace OpenTKEngine.Rendering.GLObjects;

/// <summary> An array over a number of values, with a structure. </summary>
public interface IBuffer 
{
    public int Count { get; }

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

        //TODO: Add ability to search structs for value sizes and value type
        AttributeTypes = new AttributeType[] { Util.TypeToAttributeType(typeof(T)) };
        ValueType = Util.TypeToPointerType(typeof(T));
    }

    public VertexBuffer(BufferTargetARB target, params T[] data) : this(target)
    {
        SetData(data);
    }

    public VertexBuffer(params T[] data) : this(BufferTargetARB.ArrayBuffer, data)
    {
    }

    public AttributeType[] AttributeTypes { get; private init; }
    public int TypeSize { get; private init; }

    public int Count { get; private set; }

    public VertexAttribPointerType ValueType { get; private init; }

    public override ObjectIdentifier Identifier => ObjectIdentifier.VertexArray;

    protected override uint Handle => (uint)bufferHandle.Handle;

    public void SetData(params T[] data) 
    {
        Bind();
        GL.BufferData(target, data, BufferUsageARB.StaticDraw); //TODO: Figure out the difference of usage.
        Count = data.Length;
    }

    public void SetData<T2>(params T2[] data) where T2 : unmanaged 
    {
        Bind();
        GL.BufferData(target, data, BufferUsageARB.StaticDraw);

        unsafe
        {
            Count = data.Length * sizeof(T2) / sizeof(T);
        }
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
