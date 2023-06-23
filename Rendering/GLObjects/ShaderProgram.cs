
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;

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
            attributes.Add(name,new ProgramAttribute(programHandle, name, size, type));
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
            uniforms.Add(name, new ProgramUniform(programHandle, name, size, type));
        }

    }

    private readonly Dictionary<string, ProgramAttribute> attributes;
    private readonly Dictionary<string, ProgramUniform> uniforms;

    public IReadOnlyList<ProgramAttribute> Attributes { get => attributes.Values.ToList(); }
    public IReadOnlyList<ProgramUniform> Uniforms { get => uniforms.Values.ToList(); }

    public ProgramAttribute GetAttribute(string name) => attributes[name];
    public ProgramUniform GetUniform(string name) => uniforms[name];

    public override ObjectIdentifier Identifier => ObjectIdentifier.Program;

    public override uint Handle => (uint) programHandle.Handle;

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

    private readonly ProgramHandle programHandle;

    internal ProgramAttribute(ProgramHandle programHandle, string name, int size, AttributeType type)
    {
        this.programHandle = programHandle;
        Name = name;
        Size = size;
        Type = type;
        index = (uint) GL.GetAttribLocation(programHandle, Name);
    }

    public string Name { get; private set; }

    public int Size { get; private init; }

    public AttributeType Type { get; private init; }


    private uint index;
    public uint Index
    {
        get
        {
            return index;
        }
        set
        {
            GL.BindAttribLocation(programHandle, value, Name);
            index = value;
        }
    }

}

public class ProgramUniform
{
    private readonly ProgramHandle programHandle;

    public ProgramUniform(ProgramHandle programHandle, string name, int size, UniformType type)
    {
        this.programHandle = programHandle;
        Name = name;
        Size = size;
        Type = type;
        Index = GL.GetUniformLocation(programHandle, Name);
    }
    
    public string Name;
    public int Size { get; private init; }

    public UniformType Type { get; private init; }

    public int Index { get;  }

    public void SetUniform(object value)
    {
        GL.UseProgram(programHandle);

        UniformType objectType = value switch
        {
            int =>      UniformType.Int,
            float =>    UniformType.Float,
            double =>   UniformType.Double,
            int[] =>    UniformType.IntSampler1dArray,
            Vector2 =>  UniformType.FloatVec2,
            Vector3 =>  UniformType.FloatVec3,
            Vector4 =>  UniformType.FloatVec4,
            Vector2d => UniformType.DoubleVec2,
            Vector3d => UniformType.DoubleVec3,
            Vector4d => UniformType.DoubleVec4,
            Matrix4 =>  UniformType.FloatMat4,
            Matrix4d => UniformType.DoubleMat4,
            _ => throw new NotSupportedException("The give type was not supported as a uniform."),
        };

        if(Type != objectType) throw new ArgumentException("The given type did not match the type of the uniform.");

        switch(value)
        {
            case int i:         GL.Uniform1i(Index, i); break;
            case float f:       GL.Uniform1f(Index, f); break;
            case double d:      GL.Uniform1d(Index, d); break;
            case int[] ii:      GL.Uniform1i(Index, ii.Length, ii); break;
            case Vector2 v2:    GL.Uniform2f(Index, v2); break;
            case Vector3 v3:    GL.Uniform3f(Index, v3); break;
            case Vector4 v4:    GL.Uniform4f(Index, v4); break;
            case Vector2d d2:   GL.Uniform2d(Index, d2); break;
            case Vector3d d3:   GL.Uniform3d(Index, d3); break;
            case Vector4d d4:   GL.Uniform4d(Index, d4); break;
            case Matrix4 m44:   GL.UniformMatrix4f(Index, false, m44); break;
            case Matrix4d d44:  GL.UniformMatrix4d(Index, false, d44); break;
        }
    }
}
