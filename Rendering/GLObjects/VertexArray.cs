
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Utility;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class VertexArray: GLObject
{
    private readonly VertexArrayHandle arrayHandle;

    private readonly Dictionary<uint, IBuffer> attachedBuffers;
    private ShaderProgram? program;

    public IBuffer? ElementBuffer { get; private set; } 

    public VertexArray() 
    {
        arrayHandle = GL.GenVertexArray();
        attachedBuffers = new();
    }    

    public int MaxVertexCount()
    {
        int maxVertexCount = 0;
        foreach(IBuffer buffer in attachedBuffers.Values)
        {
            if(buffer.VertexCount > maxVertexCount)
            {
                maxVertexCount = buffer.VertexCount;
            }
        }
        return maxVertexCount;
    }

    public int MinVertexCount()
    {
        if(attachedBuffers.Values.Count == 0)
        {
            return 0;
        }

        int maxVertexCount = int.MaxValue;
        foreach(IBuffer buffer in attachedBuffers.Values)
        {
            if(buffer.VertexCount < maxVertexCount)
            {
                maxVertexCount = buffer.VertexCount;
            }
        }
        return maxVertexCount;
    }

    public override ObjectIdentifier Identifier => ObjectIdentifier.VertexArray;

    protected override uint Handle => (uint) arrayHandle.Handle;

    public void SetBuffer(IBuffer buffer, uint divisor, params ProgramAttribute[] attributes)
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

        foreach(ProgramAttribute attribute in attributes)
        {
            if(program is null)
            {
                program = attribute.Program;
            }
            else if(attribute.Program != program)
            {
                throw new ArgumentException($"The given attribute {attribute.Name} originated from program shader with label \"{attribute.Program.Label}\", but expected shader with label \"{program.Label}\".");
            }
        }

        Bind();
        buffer.Bind();

        int typeSize = Util.TypeSize(buffer.ValueType);
        int offset = 0;
        for(int i = 0; i < attributes.Length; i++)
        {
            if(VertexAttribPointerType.Byte <= buffer.ValueType && buffer.ValueType <= VertexAttribPointerType.UnsignedInt)
            {
                GL.VertexAttribIPointer(attributes[i].Index, attributes[i].ValueCount, (VertexAttribIType)buffer.ValueType, buffer.VertexValueCount * typeSize, offset);
            }
            else if(buffer.ValueType == VertexAttribPointerType.Double)
            {
                GL.VertexAttribLPointer(attributes[i].Index, attributes[i].ValueCount, (VertexAttribLType)buffer.ValueType, buffer.VertexValueCount * typeSize, offset);
            }
            else{
                GL.VertexAttribPointer(attributes[i].Index, attributes[i].ValueCount, buffer.ValueType, false, buffer.VertexValueCount * typeSize, offset);
            }

            GL.VertexAttribDivisor(attributes[i].Index, divisor);
            GL.EnableVertexAttribArray(attributes[i].Index);
            offset += attributes[i].ValueCount * typeSize;

            if(divisor == 0) attachedBuffers.Add(attributes[i].Index, buffer);
        }
    }

    public void SetElementBuffer(IBuffer elementBuffer)
    {
        if(!elementBuffer.IsElementBuffer)
        {
            throw new ArgumentException("The given buffer was not an element buffer!");
        }

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
