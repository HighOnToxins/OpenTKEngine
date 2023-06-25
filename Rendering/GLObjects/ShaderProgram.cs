
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTKEngine.Utility;
using System.Collections;
using System.Xml.Linq;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class ShaderProgram: GLObject
{
    private readonly ProgramHandle programHandle;

    public ShaderProgram(string vertexShaderSource, string fragmentShaderSource)
    {
        ShaderHandle vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);
        GL.GetShaderInfoLog(vertexShader, out string vertInfoLog);
        if(!string.IsNullOrEmpty(vertInfoLog)) throw new Exception($"Shader compile error: {vertInfoLog}");

        ShaderHandle fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);
        GL.GetShaderInfoLog(fragmentShader, out string fragInfoLog);
        if(!string.IsNullOrEmpty(fragInfoLog)) throw new Exception($"Shader compile error: {fragInfoLog}");

        programHandle = GL.CreateProgram();
        GL.AttachShader(programHandle, vertexShader);
        GL.AttachShader(programHandle, fragmentShader);
        GL.LinkProgram(programHandle);
        GL.DetachShader(programHandle, fragmentShader);
        GL.DetachShader(programHandle, vertexShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.GetProgramInfoLog(programHandle, out string vertexInfoLog);
        if(!string.IsNullOrEmpty(vertexInfoLog)) throw new Exception($"Shader compile error: {vertexInfoLog}");

        int attributeCount = 0;
        GL.GetProgrami(programHandle, ProgramPropertyARB.ActiveAttributes, ref attributeCount);
        int maxAttributeLength = 0;
        GL.GetProgrami(programHandle, ProgramPropertyARB.ActiveAttributeMaxLength, ref maxAttributeLength);
        attributes = new();
        for (uint i = 0; i < attributeCount; i++)
        {
            int length = 0;
            int size = 0;
            AttributeType type = 0;
            GL.GetActiveAttrib(programHandle, i, maxAttributeLength, ref length, ref size, ref type, out string name);
            uint index = (uint) GL.GetAttribLocation(programHandle, name);
            attributes.Add(name,new ProgramAttribute(this, name, size, type, index));
        }

        int uniformCount = 0;
        GL.GetProgrami(programHandle, ProgramPropertyARB.ActiveUniforms, ref uniformCount);
        int maxUniformLength = 0;
        GL.GetProgrami(programHandle, ProgramPropertyARB.ActiveUniformMaxLength, ref maxUniformLength);
        uniforms = new();
        for(uint i = 0; i < uniformCount; i++)
        {
            int length = 0;
            int size = 0;
            UniformType type = 0;
            GL.GetActiveUniform(programHandle, i, maxUniformLength, ref length, ref size, ref type, out string name);
            int location = GL.GetUniformLocation(programHandle, name);
            uniforms.Add(name, new ProgramUniform(this, name, size, type, location));
        }

    }

    public static ShaderProgram LoadFromFiles(string vertexShaderPath, string fragmentShaderPath)
    {
        return new ShaderProgram(File.ReadAllText(vertexShaderPath), File.ReadAllText(fragmentShaderPath));
    }

    private readonly Dictionary<string, ProgramAttribute> attributes;

    private readonly Dictionary<string, ProgramUniform> uniforms;

    public IReadOnlyList<ProgramAttribute> Attributes { get => attributes.Values.ToList(); }

    public IReadOnlyList<ProgramUniform> Uniforms { get => uniforms.Values.ToList(); }

    public ProgramAttribute GetAttribute(string name) => attributes[name];

    public ProgramUniform GetUniform(string name) => uniforms[name];

    public override ObjectIdentifier Identifier => ObjectIdentifier.Program;

    protected override uint Handle => (uint) programHandle.Handle;

    internal void BindAttributeLocation(string name, uint index)
    {
        GL.BindAttribLocation(programHandle, index, name);
    }

    public void Draw(VertexArray vertexArray, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        Bind();
        vertexArray.Bind();
        GL.DrawArrays(primitiveType, 0, vertexArray.MaxVertexCount());
    }

    public void DrawElements(VertexArray vertexArray, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        if(vertexArray.ElementBuffer is null) throw new ArgumentException("No element buffer was attached to the vertex array!");

        Bind();
        vertexArray.Bind();
        DrawElementsType elementType = (DrawElementsType)vertexArray.ElementBuffer.ValueType;
        GL.DrawElements(primitiveType, vertexArray.ElementBuffer.ValueCount, elementType, 0);
    }

    public void DrawInstanced(VertexArray vertexArray, int instanceCount, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        Bind();
        vertexArray.Bind();
        GL.DrawArraysInstanced(primitiveType, 0, vertexArray.MaxVertexCount(), instanceCount);
    }

    public void DrawElementsInstanced(VertexArray vertexArray, int instanceCount, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        if(vertexArray.ElementBuffer is null) throw new ArgumentException("No element buffer was attached to the vertex array!");

        Bind();
        vertexArray.Bind();
        DrawElementsType elementType = (DrawElementsType)vertexArray.ElementBuffer.ValueType;
        GL.DrawElementsInstanced(primitiveType, vertexArray.ElementBuffer.ValueCount, elementType, 0, instanceCount);
    }

    public override void Bind()
    {
        GL.UseProgram(programHandle);
    }

    public override void Dispose()
    {
        GL.DeleteProgram(programHandle);
    }
}

public class ProgramAttribute {

    public ShaderProgram Program;

    internal ProgramAttribute(ShaderProgram program, string name, int size, AttributeType type, uint index)
    {
        Program = program;
        Name = name;
        Size = size; 
        AttribType = type;
        this.index = index;
        ValueCount = Util.ValueCount(AttribType) * size;
    }

    public string Name { get; private set; }

    public int Size { get; private init; }

    public AttributeType AttribType { get; private init; }

    private uint index;
    public int ValueCount { get; private set; }

    public uint Index
    {
        get
        {
            return index;
        }
        set
        {
            Program.BindAttributeLocation(Name, value);
            index = value;
        }
    }

}

public class ProgramUniform
{
    public ShaderProgram Program;

    public ProgramUniform(ShaderProgram program, string name, int size, UniformType type, int location)
    {
        Program = program;
        Name = name;
        Size = size; //Use the size, for value count.
        UniformType = type;
        Location = location;
    }
    
    public string Name;
    public int Size { get; private init; }

    public UniformType UniformType { get; private init; }

    public int Location { get;  }

    public void SetValue(object value)
    {
        if(UniformType != (UniformType)Util.TypeToAttributeType(value.GetType())) throw new ArgumentException("The given type did not match the type of the uniform.");

        if(value is Array array && array.Length > Size)
        {
            throw new ArgumentException($"The size of the given array was {array.Length}, but the uniform has a maximum size of {Size}");
        }

        Program.Bind();
        switch(value)
        {
            case TextureUnit u: GL.Uniform1i(Location, (int)(u - TextureUnit.Texture0)); break;
            case int i: GL.Uniform1i(Location, i); break;
            case float f: GL.Uniform1f(Location, f); break;
            case double d: GL.Uniform1d(Location, d); break;
            case Vector2 v2: GL.Uniform2f(Location, v2); break;
            case Vector3 v3: GL.Uniform3f(Location, v3); break;
            case Vector4 v4: GL.Uniform4f(Location, v4); break;
            case Vector2d d2: GL.Uniform2d(Location, d2); break;
            case Vector3d d3: GL.Uniform3d(Location, d3); break;
            case Vector4d d4: GL.Uniform4d(Location, d4); break;
            case Matrix4 m44: GL.UniformMatrix4f(Location, false, m44); break;
            case Matrix4d d44: GL.UniformMatrix4d(Location, false, d44); break;

            case TextureUnit[] u: GL.Uniform1i(Location, u.Length, u.Select(i => (int)(i - TextureUnit.Texture0)).ToArray()); break;
            case int[] i: GL.Uniform1i(Location, i.Length, i); break;
            case float[] f: GL.Uniform1f(Location, f.Length, f); break;
            case double[] d: GL.Uniform1d(Location, d.Length, d); break;
            case Vector2[] v2: GL.Uniform2f(Location, v2.Length, v2); break;
            case Vector3[] v3: GL.Uniform3f(Location, v3.Length, v3); break;
            case Vector4[] v4: GL.Uniform4f(Location, v4.Length, v4); break;
            case Vector2d[] d2: GL.Uniform2d(Location, d2.Length, d2); break;
            case Vector3d[] d3: GL.Uniform3d(Location, d3.Length, d3); break;
            case Vector4d[] d4: GL.Uniform4d(Location, d4.Length, d4); break;
            case Matrix4[] m44: GL.UniformMatrix4f(Location, m44.Length, false, m44); break;
            case Matrix4d[] d44: GL.UniformMatrix4d(Location, d44.Length, false, d44); break;

            default: throw new NotSupportedException("The given object was not supported.");
        }
    }
}
