
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Utility;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class VertexArray: GLObject
{
    private readonly VertexArrayHandle arrayHandle;

    private readonly Dictionary<uint, (IBuffer buffer, uint divisor)> vertexBuffers;
    private ShaderProgram? program;

    public IBuffer? ElementBuffer { get; private set; }

    public VertexArray() 
    {
        arrayHandle = GL.GenVertexArray();
        vertexBuffers = new();
    }    

    public int MaxVertexCount()
    {
        int maxVertexCount = 0;
        foreach((IBuffer buffer, uint divisor) in vertexBuffers.Values)
        {
            if(divisor == 0 && buffer.VertexCount > maxVertexCount)
            {
                maxVertexCount = buffer.VertexCount;
            }
        }
        return maxVertexCount;
    }

    public int MaxInstanceCount()
    {
        int maxInstanceCount = 0;
        foreach((IBuffer buffer, uint divisor) in vertexBuffers.Values)
        {
            if(divisor != 0 && buffer.VertexCount > maxInstanceCount)
            {
                maxInstanceCount = buffer.VertexCount * (int)divisor;
            }
        }
        return maxInstanceCount;
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
        int stride = attributes.Select(a => a.Size * a.AttribCount).Sum() * typeSize;
        for(int i = 0; i < attributes.Length; i++)
        {
            int columnOffset = 0;
            for(uint column = 0; column < attributes[i].AttribCount; column++)
            {
                int totalOffset = offset + columnOffset;
                if(VertexAttribPointerType.Byte <= buffer.ValueType && buffer.ValueType <= VertexAttribPointerType.UnsignedInt)
                {
                    GL.VertexAttribIPointer(attributes[i].Index + column, attributes[i].Size, (VertexAttribIType)buffer.ValueType, stride, totalOffset);
                }
                else if(buffer.ValueType == VertexAttribPointerType.Double)
                {
                    GL.VertexAttribLPointer(attributes[i].Index + column, attributes[i].Size, (VertexAttribLType)buffer.ValueType, stride, totalOffset);
                }
                else{
                    GL.VertexAttribPointer(attributes[i].Index + column, attributes[i].Size, buffer.ValueType, false, stride, totalOffset);
                }

                GL.VertexAttribDivisor(attributes[i].Index + column, divisor);
                GL.EnableVertexAttribArray(attributes[i].Index + column);

                columnOffset += attributes[i].Size * typeSize;
            }

            offset += attributes[i].AttribCount * attributes[i].Size * typeSize;

            vertexBuffers.Add(attributes[i].Index, (buffer, divisor));
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
