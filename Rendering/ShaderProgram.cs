using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering;

public class ShaderProgram : GLObject {

	//fields
	private readonly ProgramHandle _shaderProgram;

	//properties
	public override ObjectIdentifier Identifier { get => ObjectIdentifier.Program; }
	public override uint Handle { get => (uint)_shaderProgram.Handle; }

	//shader data (variables and such)
	public IEnumerable<string> VariableNames;
    public IEnumerable<string> UniformNames;

    //constructor
    public ShaderProgram(string vertPath, string fragPath) {

		_shaderProgram = GL.CreateProgram();

		Shader vertexShader = new(vertPath, _shaderProgram, ShaderType.VertexShader);
		Shader fragmentShader = new(fragPath, _shaderProgram, ShaderType.FragmentShader);

        VariableNames = vertexShader.VariableNames.Concat(fragmentShader.VariableNames);
        UniformNames = vertexShader.UniformNames.Concat(fragmentShader.UniformNames);

        vertexShader.Bind();
		fragmentShader.Bind();

		GL.LinkProgram(_shaderProgram);
		GL.GetProgramInfoLog(_shaderProgram, out string vertexInfoLog);

		if(!string.IsNullOrEmpty(vertexInfoLog)) {
			throw new Exception($"Shader compile error: {vertexInfoLog}");
		}

		vertexShader.Unbind();
		fragmentShader.Unbind();
		vertexShader.Dispose();
		fragmentShader.Dispose();

	}

	private ShaderProgram() {
		VariableNames = new List<string>();
		UniformNames = new List<string>();
	}

	// - - - GLObject Methods - - - 
	public override void Bind() =>
		GL.UseProgram(_shaderProgram);

	public override void Unbind() =>
		StaticUnbind();

	public static void StaticUnbind() =>
		GL.UseProgram(ProgramHandle.Zero);

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
	public override void Dispose() =>
		GL.DeleteProgram(_shaderProgram);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize

	// - - - Shader Methods - - - 
	public void SetUniform(int location, float value) {
		Bind();
		GL.Uniform1f(location, value);
		Unbind();
	}

	public void SetUniform(int location, int value) {
		Bind();
		GL.Uniform1i(location, value);
		Unbind();
	}

	public void SetUniform(int location, double value) {
		Bind();
		GL.Uniform1d(location, value);
		Unbind();
	}

	public void SetUniform(int location, int[] values) {
		Bind();
		GL.Uniform1i(location, values.Length, values);
		Unbind();
	}

	public void SetUniform(int location, Vector2 value) {
		Bind();
		GL.Uniform2f(location, value);
		Unbind();
	}

	public void SetUniform(int location, Vector2d value) {
		Bind();
		GL.Uniform2d(location, value);
		Unbind();
	}

	public void SetUniform(int location, Vector3 value) {
		Bind();
		GL.Uniform3f(location, value);
		Unbind();
	}

	public void SetUniform(int location, Matrix4 value, bool transpose) {
		Bind();
		GL.UniformMatrix4f(location, transpose, value);
		Unbind();
	}

	public void SetUniform(int location, Matrix4d value, bool transpose) {
		Bind();
		GL.UniformMatrix4d(location, transpose, value);
		Unbind();
	}

	public uint GetVariableLocation(string variableName) {
		return (uint)GL.GetAttribLocation(_shaderProgram, variableName);
	}

	public int GetUniformLocation(string variableName) {
		return GL.GetUniformLocation(_shaderProgram, variableName);
	}

	public static ShaderProgram EmptyShader => new();

}