
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Utility;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class VertexArray: GLObject
{
    private readonly VertexArrayHandle arrayHandle;

    public IBuffer? ElementBuffer { get; private set; } 

    public VertexArray() 
    {
        arrayHandle = GL.GenVertexArray();
    }    

    public int MaxVertexCount { get; private set; }

    public override ObjectIdentifier Identifier => ObjectIdentifier.VertexArray;

    protected override uint Handle => (uint) arrayHandle.Handle;

    public void AddBuffer(IBuffer buffer, uint divisor, params ProgramAttribute[] attributes)
    {
        if(buffer.AttributeTypes.Length != attributes.Length)
        {
            throw new ArgumentException($"An incorrect number of attributes was given when adding buffer to vertex array, expected {buffer.AttributeTypes.Length} attributes but got {attributes.Length}");
        }

        for(int i = 0; i < buffer.AttributeTypes.Length; i++)
        {
            if(buffer.AttributeTypes[i] != attributes[i].AttribType)
            {
                throw new ArgumentException($"Attribute {attributes[i].Name} of type {attributes[i].AttribType} did not match the the buffer which expected {buffer.AttributeTypes[i]}.");
            }
        }

        Bind();
        buffer.Bind();

        int typeSize = Util.TypeSize(buffer.ValueType);
        int offset = 0;
        for(int i = 0; i < attributes.Length; i++)
        {
            GL.VertexAttribPointer(attributes[i].Index, attributes[i].ValueCount, buffer.ValueType, false, buffer.ElementValueCount * typeSize, offset);
            GL.VertexAttribDivisor(attributes[i].Index, divisor);
            GL.EnableVertexAttribArray(attributes[i].Index);
            offset += attributes[i].ValueCount * typeSize;
        }

        if(divisor == 0)
        {
            int vertexCount = buffer.ValueCount / buffer.ElementValueCount;
            if(vertexCount > MaxVertexCount) MaxVertexCount = vertexCount;
        }
    }

    public void SetElementBuffer(IBuffer elementBuffer)
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
