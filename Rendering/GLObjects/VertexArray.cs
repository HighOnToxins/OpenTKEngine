
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

    public int MaxVertexCount { get; private set; }

    public override ObjectIdentifier Identifier => ObjectIdentifier.VertexArray;

    public override uint Handle => (uint) arrayHandle.Handle;

    public void AddBuffer(Buffer buffer, uint divisor, params ProgramAttribute[] attributes)
    {
        //TODO: Use shader program information, to determine buffer-field sizes, and if the type matches.
        // Such as having the ArraySize match an arraySize of elements in the buffer.
        // Such as making sure the correct number of attributes was given.

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

        //check if the types match
        foreach(ProgramAttribute attribute in attributes)
        {
            VertexAttribPointerType attribPointerType = attribute.Type switch
            {
                AttributeType.Int =>        VertexAttribPointerType.Int,
                AttributeType.Float =>      VertexAttribPointerType.Float,
                AttributeType.Double =>     VertexAttribPointerType.Double,
                AttributeType.FloatVec2 =>  VertexAttribPointerType.Float,
                AttributeType.FloatVec3 =>  VertexAttribPointerType.Float,
                AttributeType.FloatVec4 =>  VertexAttribPointerType.Float,
                AttributeType.DoubleVec2 => VertexAttribPointerType.Double,
                AttributeType.DoubleVec3 => VertexAttribPointerType.Double,
                AttributeType.DoubleVec4 => VertexAttribPointerType.Double,
                AttributeType.FloatMat4 =>  VertexAttribPointerType.Float,
                AttributeType.DoubleMat4 => VertexAttribPointerType.Double,
                _ => throw new NotSupportedException("The given attribute type was not supported!"),
            };

            if(pointerType != attribPointerType)
            {
                throw new ArgumentException("The type of the given buffer did not match the type of the given attribute!");
            }
        }

        int totalSize = 0;
        for(int i = 0; i < attributes.Length; i++)
        {
            totalSize += attributes[i].ArraySize;
        }

        Bind();
        buffer.Bind();

        int offset = 0;
        for(int i = 0; i < attributes.Length; i++)
        {
            GL.VertexAttribPointer(attributes[i].Index, attributes[i].ArraySize, pointerType, false, totalSize * typeSize, offset);
            GL.VertexAttribDivisor(attributes[i].Index, divisor);
            GL.EnableVertexAttribArray(attributes[i].Index);
            offset += attributes[i].ArraySize * typeSize;
        }

        if(divisor == 0)
        {
            int vertexCount = buffer.Count / totalSize;
            if(vertexCount > MaxVertexCount) MaxVertexCount = vertexCount;
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
