
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKEngine.Rendering.GLObjects;

namespace OpenTKEngine.Rendering;

//TODO: Consider making mesh into interface, and moving element buffer to mesh<T>.
public abstract class Mesh: IDisposable
{
    //TODO: Add the information of how to draw the buffers into the mesh. (such as triangle or fan or such)
    public static readonly Mesh<Vector3> Box = new(
        new Vector3[]
        {
            new(1, 1, 0),
            new(1, 0, 0),
            new(0, 1, 0),
            new(0, 0, 0),
        },
        new uint[]
        {
            0, 1, 2, 1, 2, 3
        }
    );

    private readonly Buffer<uint>? elementBuffer;

    public bool HasElements { get => elementBuffer is not null; }

    public Mesh(params uint[] elements) 
    { 
        if(elements.Length != 0)
        {
            elementBuffer = new Buffer<uint>(BufferTargetARB.ElementArrayBuffer, elements);
        }
    }

    public virtual int SetBuffers(VertexArray array, params ProgramAttribute[] attributes)
    {
        if(elementBuffer is not null)
        {
            array.SetElementBuffer(elementBuffer);
        }

        return 0;
    }

    public virtual void Dispose()
    {
        elementBuffer?.Dispose();
    }

}


public class Mesh<T> : Mesh where T : unmanaged
{

    private readonly Buffer<T> buffer;

    public Mesh(T[] vertices, params uint[] elements) : base(elements)
    {
        buffer = new(vertices);
    }

    public Mesh(params T[] vertices) : this(vertices, Array.Empty<uint>())
    {
    }

    public override int SetBuffers(VertexArray array, params ProgramAttribute[] attributes)
    {
        base.SetBuffers(array, attributes);
        array.SetBuffer(buffer, 0, attributes[..buffer.AttributeTypes.Length]);
        return buffer.AttributeTypes.Length;
    }

    public override void Dispose()
    {
        base.Dispose();
        buffer.Dispose();
    }
}

public class Mesh<T1, T2>: Mesh<T1> where T1: unmanaged where T2: unmanaged
{
    private readonly Buffer<T2> buffer;

    public Mesh((T1, T2)[] vertices, params uint[] elements) : 
        base(vertices.Select(v => v.Item1).ToArray(), elements)
    {
        buffer = new(vertices.Select(v => v.Item2).ToArray());
    }

    public Mesh((T1, T2)[] vertices) : this(vertices, Array.Empty<uint>())
    {
    }

    public override int SetBuffers(VertexArray array, params ProgramAttribute[] attributes)
    {
        int offset = base.SetBuffers(array, attributes);
        array.SetBuffer(buffer, 0, 
            attributes[(offset+1)..(offset+1+buffer.AttributeTypes.Length)]);
        return offset + buffer.AttributeTypes.Length;
    }

    public override void Dispose()
    {
        base.Dispose();
        buffer.Dispose();
    }
}