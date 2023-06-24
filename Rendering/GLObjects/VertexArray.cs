
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Utility;

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

        VertexAttribPointerType pointerType = Util.TypeToPointerType(buffer.Type);
        int typeSize = buffer.Type.Size();

        //check if the types match
        foreach(ProgramAttribute attribute in attributes)
        {
            if(pointerType != Util.AttributeToPointerType(attribute.AttribType))
            {
                throw new ArgumentException("The type of the given buffer did not match the type of the given attribute!");
            }
        }

        int totalSize = 0;
        for(int i = 0; i < attributes.Length; i++)
        {
            totalSize += attributes[i].ValueCount;
        }

        Bind();
        buffer.Bind();

        int offset = 0;
        for(int i = 0; i < attributes.Length; i++)
        {
            GL.VertexAttribPointer(attributes[i].Index, attributes[i].ValueCount, pointerType, false, totalSize * typeSize, offset);
            GL.VertexAttribDivisor(attributes[i].Index, divisor);
            GL.EnableVertexAttribArray(attributes[i].Index);
            offset += attributes[i].ValueCount * typeSize;
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
