
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class VertexArray: GLObject
{
    private readonly VertexArrayHandle arrayHandle;

    public Buffer? ElementBuffer { get; private set; } 

    public VertexArray() 
    {
        arrayHandle = GL.GenVertexArray();
    }    

    public override ObjectIdentifier Identifier => ObjectIdentifier.VertexArray;

    public override uint Handle => (uint) arrayHandle.Handle;

    public void SetBuffer<T>(VertexBuffer<T> buffer, BufferFields attribute, uint divisor = 0) where T : unmanaged
    {
        (VertexAttribPointerType pointerType, int typeSize) = typeof(T).Name switch
        {
            nameof(Byte) => (VertexAttribPointerType.UnsignedByte, sizeof(byte)),
            nameof(SByte) => (VertexAttribPointerType.Byte, sizeof(sbyte)),
            nameof(Int16) => (VertexAttribPointerType.Short, sizeof(short)),
            nameof(UInt16) => (VertexAttribPointerType.UnsignedShort, sizeof(ushort)),
            nameof(Int32) => (VertexAttribPointerType.Int, sizeof(int)),
            nameof(UInt32) => (VertexAttribPointerType.UnsignedInt, sizeof(uint)),
            nameof(Single) => (VertexAttribPointerType.Float, sizeof(float)),
            nameof(Double) => (VertexAttribPointerType.Double, sizeof(double)),
            _ => throw new NotImplementedException(),
        };

        int stride = 0;
        for(int i = 0; i < attribute.Field.Length; i++)
        {
            stride += attribute.Field[i].Size * typeSize;
        }

        Bind();
        buffer.Bind();

        int offset = 0;
        for(int i = 0; i < attribute.Field.Length; i++)
        {
            GL.VertexAttribPointer(attribute.Field[i].Index, attribute.Field[i].Size, pointerType, false, stride, offset);
            GL.VertexAttribDivisor(attribute.Field[i].Index, divisor);
            GL.EnableVertexAttribArray(attribute.Field[i].Index);
            offset += attribute.Field[i].Size * typeSize;
        }
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
