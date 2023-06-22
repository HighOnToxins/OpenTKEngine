
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public abstract class Buffer: GLObject
{
    private readonly BufferHandle bufferHandle;

    public Buffer(BufferTargetARB target = BufferTargetARB.ArrayBuffer)
    {
        this.Target = target;
        bufferHandle = GL.GenBuffer();
    }

    protected BufferTargetARB Target { get; private init; }

    public override ObjectIdentifier Identifier => ObjectIdentifier.Buffer;

    public override uint Handle => (uint)bufferHandle.Handle;

    public abstract int Count { get; }

    public abstract Type Type {get; }

    public override void Bind()
    {
        GL.BindBuffer(Target, bufferHandle);
    }

    public override void Dispose()
    {
        GL.DeleteBuffer(bufferHandle);
    }
}
