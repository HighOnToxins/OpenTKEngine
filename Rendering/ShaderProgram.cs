using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering;

public class ShaderProgram : GLObject {

	//fields
	private readonly ProgramHandle _shaderProgram;
	private readonly Dictionary<string, string> _nameByLabel;

	//properties
	public override ObjectIdentifier Identifier { get => ObjectIdentifier.Program; }
	public override uint Handle { get => (uint)_shaderProgram.Handle; }

	//constructor
	public ShaderProgram(string vertPath, string fragPath, params (string label, string name)[] nameByLabel) {
		_nameByLabel = new Dictionary<string, string>();

		for(int i = 0; i < nameByLabel.Length; i++) {
			_nameByLabel.Add(nameByLabel[i].label, nameByLabel[i].name);
		}
		
		_shaderProgram = GL.CreateProgram();

		Shader vertexShader = new(vertPath, _shaderProgram, ShaderType.VertexShader);
		Shader fragmentShader = new(fragPath, _shaderProgram, ShaderType.FragmentShader);

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
	public void SetUniform(string name, float value) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.Uniform1f(location, value);
		Unbind();
	}

	public void SetUniform(string name, int value) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.Uniform1i(location, value);
		Unbind();
	}

	public void SetUniform(string name, double value) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.Uniform1d(location, value);
		Unbind();
	}

	public void SetUniform(string name, int[] values) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.Uniform1i(location, values.Length, values);
		Unbind();
	}

	public void SetUniform(string name, Vector2 value) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.Uniform2f(location, value);
		Unbind();
	}

	public void SetUniform(string name, Vector2d value) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.Uniform2d(location, value);
		Unbind();
	}

	public void SetUniform(string name, Vector3 value) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.Uniform3f(location, value);
		Unbind();
	}

	public void SetUniform(string name, Matrix4 value, bool transpose) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.UniformMatrix4f(location, transpose, value);
		Unbind();
	}

	public void SetUniform(string name, Matrix4d value, bool transpose) {
		int location = GetUniformVariableLocation(name);
		Bind();
		GL.UniformMatrix4d(location, transpose, value);
		Unbind();
	}

	public int GetVariableLocation(string variableName) {
		return GL.GetAttribLocation(_shaderProgram, variableName);
	}

	public int GetUniformVariableLocation(string variableName) {
		return GL.GetUniformLocation(_shaderProgram, variableName);
	}

	public uint[] GetVariableLocations(params string[] variableNames) {
		uint[] variableLocations = new uint[variableNames.Length];

		for(int i = 0; i < variableLocations.Length; i++) {

			int location = GetVariableLocation(variableNames[i]);
			if(location < 0) throw new ArgumentException("Variable name was not found.");
			variableLocations[i] = (uint)location;
		}

		return variableLocations;
	}

	public string? GetNameFromLabel(string label) {
		if(_nameByLabel.TryGetValue(label, out string? location)) {
			return location;
		}

		return null;
	}

}