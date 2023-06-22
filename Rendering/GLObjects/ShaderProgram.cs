
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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
    }

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
