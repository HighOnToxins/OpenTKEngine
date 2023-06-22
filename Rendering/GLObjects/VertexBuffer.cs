
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class VertexBuffer<T>: Buffer where T : unmanaged
{
    private int count;

    public VertexBuffer(BufferTargetARB target = BufferTargetARB.ArrayBuffer)  : base(target)
    {
    }

    public VertexBuffer(params T[] data) : base()
    {
        SetData(data);
    }

    public void SetData(params T[] data)
    {
        Bind();
        GL.BufferData(Target, data, BufferUsageARB.StaticDraw);
        count = data.Length;
    }

    public override int Count {get => count;}

    public override Type Type => typeof(T);

}
