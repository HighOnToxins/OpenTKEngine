
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Text;

namespace OpenTKEngine.Rendering; 

public sealed class Shader : GLObject{

	//fields
	private readonly ShaderHandle _shaderObject;
	private readonly ProgramHandle _shaderProgram;

	//properties
	public override ObjectIdentifier Identifier { get => ObjectIdentifier.Shader; }
	public override uint Handle { get => (uint)_shaderObject.Handle; }

	//constructor
	public Shader(string shaderPath, ProgramHandle program, ShaderType type) {
		_shaderProgram = program;

		string shaderCode;

		using(StreamReader reader = new(shaderPath, Encoding.UTF8)) {
			shaderCode = reader.ReadToEnd();
		}

		_shaderObject = GL.CreateShader(type);
		GL.ShaderSource(_shaderObject, shaderCode);
		GL.CompileShader(_shaderObject);
		GL.GetShaderInfoLog(_shaderObject, out string infoLog);

		if(!string.IsNullOrEmpty(infoLog)) {
			throw new Exception($"Shader compile error: {infoLog}");
		}
	}

	public override void Bind() =>
		GL.AttachShader(_shaderProgram, _shaderObject);

	public override void Unbind() =>
		GL.AttachShader(_shaderProgram, _shaderObject);

	public override void Dispose() =>
		GL.DeleteShader(_shaderObject);

}

