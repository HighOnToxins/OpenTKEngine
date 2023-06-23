
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class VertexArray: GLObject
{
    private readonly VertexArrayHandle arrayHandle;

    private readonly List<(Buffer buffer, int totalSize)> buffers;

    public Buffer? ElementBuffer { get; private set; } 

    public VertexArray() 
    {
        arrayHandle = GL.GenVertexArray();
        buffers = new();
    }    

    public int VertexCount { 
        get {
            return buffers[0].buffer.Count / buffers[0].totalSize;
        } 
    }

    public override ObjectIdentifier Identifier => ObjectIdentifier.VertexArray;

    public override uint Handle => (uint) arrayHandle.Handle;

    public void AddBuffer(Buffer buffer, BufferFields fields, uint divisor = 0)
    {
        (VertexAttribPointerType pointerType, int typeSize) = buffer.Type.Name switch
        {
            nameof(Byte) =>   (VertexAttribPointerType.UnsignedByte,    sizeof(byte)),
            nameof(SByte) =>  (VertexAttribPointerType.Byte,            sizeof(sbyte)),
            nameof(Int16) =>  (VertexAttribPointerType.Short,           sizeof(short)),
            nameof(UInt16) => (VertexAttribPointerType.UnsignedShort,   sizeof(ushort)),
            nameof(Int32) =>  (VertexAttribPointerType.Int,             sizeof(int)),
            nameof(UInt32) => (VertexAttribPointerType.UnsignedInt,     sizeof(uint)),
            nameof(Single) => (VertexAttribPointerType.Float,           sizeof(float)),
            nameof(Double) => (VertexAttribPointerType.Double,          sizeof(double)),
            _ => throw new NotImplementedException(),
        };

        int totalSize = 0;
        for(int i = 0; i < fields.Fields.Length; i++)
        {
            totalSize += fields.Fields[i].Size;
        }

        Bind();
        buffer.Bind();

        int offset = 0;
        for(int i = 0; i < fields.Fields.Length; i++)
        {
            GL.VertexAttribPointer(fields.Fields[i].Index, fields.Fields[i].Size, pointerType, false, totalSize * typeSize, offset);
            GL.VertexAttribDivisor(fields.Fields[i].Index, divisor);
            GL.EnableVertexAttribArray(fields.Fields[i].Index);
            offset += fields.Fields[i].Size * typeSize;
        }

        buffers.Add((buffer, totalSize));
    }

    public void SetElementBuffer(Buffer elementBuffer)
    {
        Bind();
        elementBuffer.Bind();
        ElementBuffer = elementBuffer;
    }

    public override void Bind()
    {
        GL.BindVertexArray(arrayHandle);
    }

    public override void Dispose()
    {
        GL.DeleteVertexArray(arrayHandle);
    }
}
