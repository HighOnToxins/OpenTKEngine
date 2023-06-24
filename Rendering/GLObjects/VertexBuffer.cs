
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public abstract class Buffer: GLObject
{
    private readonly BufferHandle bufferHandle;

    public Buffer(BufferTargetARB target = BufferTargetARB.ArrayBuffer)
    {
        Target = target;
        bufferHandle = GL.GenBuffer();
    }

    protected BufferTargetARB Target { get; private init; }

    public override ObjectIdentifier Identifier => ObjectIdentifier.Buffer;

    public override uint Handle => (uint)bufferHandle.Handle;

    public abstract int Count { get; }

    public abstract Type Type { get; }

    public override void Bind()
    {
        GL.BindBuffer(Target, bufferHandle);
    }

    public override void Dispose()
    {
        GL.DeleteBuffer(bufferHandle);
    }
}

//TODO: Change the buffer to automatically, keep track of data.
public class VertexBuffer<T>: Buffer where T : unmanaged
{
    private int count;

    public VertexBuffer(BufferTargetARB target = BufferTargetARB.ArrayBuffer)  : base(target)
    {
    }

    public VertexBuffer(BufferTargetARB target, params T[] data) : base(target)
    {
        SetData(data);
    }

    public VertexBuffer(params T[] data) : this(BufferTargetARB.ArrayBuffer, data)
    {
    }

    public void SetData(params T[] data) 
    {
        Bind();
        GL.BufferData(Target, data, BufferUsageARB.StaticDraw);
        count = data.Length;
    }

    public void SetData<T2>(params T2[] data) where T2 : unmanaged 
    {
        Bind();
        GL.BufferData(Target, data, BufferUsageARB.StaticDraw);

        unsafe
        {
            count = data.Length * sizeof(T2) / sizeof(T);
        }
    }

    public override int Count {get => count;}

    public override Type Type => typeof(T); //TODO: Maybe change VertexBuffer.type into pointerType or attributeType.
}
