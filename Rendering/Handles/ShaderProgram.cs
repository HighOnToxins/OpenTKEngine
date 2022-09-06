using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Text;

namespace OpenTKMiniEngine.Rendering.Handles;

public sealed class ShaderProgram{

	public record ShaderInfo(string VertPath, string FragPath);

	private readonly ProgramHandle _shaderProgram;

	public ShaderProgram(ShaderInfo shaderInfo) {
		static ShaderHandle CreateShader(string shaderPath, ShaderType type) {
			string shaderCode;

			using(StreamReader reader = new(shaderPath, Encoding.UTF8)) {
				shaderCode = reader.ReadToEnd();
			}

			ShaderHandle shader = GL.CreateShader(type);
			GL.ShaderSource(shader, shaderCode);
			GL.CompileShader(shader);
			GL.GetShaderInfoLog(shader, out string infoLog);

			if(!string.IsNullOrEmpty(infoLog)) {
				throw new Exception($"Shader compile error: {infoLog}");
			}

			return shader;
		}

		ShaderHandle vertexShader = CreateShader(shaderInfo.VertPath, ShaderType.VertexShader);
		ShaderHandle fragmentShader = CreateShader(shaderInfo.FragPath, ShaderType.FragmentShader);

		_shaderProgram = GL.CreateProgram();
		GL.AttachShader(_shaderProgram, vertexShader);
		GL.AttachShader(_shaderProgram, fragmentShader);
		GL.LinkProgram(_shaderProgram);

		GL.GetProgramInfoLog(_shaderProgram, out string vertexInfoLog);

		if(!string.IsNullOrEmpty(vertexInfoLog)) {
			throw new Exception($"Shader compile error: {vertexInfoLog}");
		}

		GL.DetachShader(_shaderProgram, vertexShader);
		GL.DetachShader(_shaderProgram, fragmentShader);
		GL.DeleteShader(vertexShader);
		GL.DeleteShader(fragmentShader);
	}

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

	public void Bind() {GL.UseProgram(_shaderProgram);}
	public static void Unbind() {GL.UseProgram(ProgramHandle.Zero);}
	public void Dispose() {GL.DeleteProgram(_shaderProgram);}

}
