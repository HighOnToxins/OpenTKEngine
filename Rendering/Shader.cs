
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

    //info
    public IEnumerable<string> VariableNames;
    public IEnumerable<string> UniformNames;

    //constructor
    public Shader(string shaderPath, ProgramHandle program, ShaderType type) {
		_shaderProgram = program;

		string shaderCode;

		using(StreamReader reader = new(shaderPath, Encoding.UTF8)) {
			shaderCode = reader.ReadToEnd();
		}

		VariableNames = GetNames(shaderCode, "in");
		UniformNames = GetNames(shaderCode, "uniform");

		_shaderObject = GL.CreateShader(type);
		GL.ShaderSource(_shaderObject, shaderCode);
		GL.CompileShader(_shaderObject);
		GL.GetShaderInfoLog(_shaderObject, out string infoLog);

		if(!string.IsNullOrEmpty(infoLog)) {
			throw new Exception($"Shader compile error: {infoLog}");
		}
	}

	private static IEnumerable<string> GetNames(string shaderCode, string searchType) {
		int indexOfType = shaderCode.IndexOf(searchType, 0);

        do {
			int indexOfVar = shaderCode.IndexOf(' ', indexOfType, 2);
			int endOfVar = shaderCode.IndexOf(' ', indexOfVar) - 1;

			yield return shaderCode.Substring(indexOfVar, endOfVar - indexOfVar + 1);

            indexOfType = shaderCode.IndexOf(searchType, indexOfType);
        } while(indexOfType == -1);
	}

	private static List<string> GetVariableNames(string shaderCode) {
		throw new NotImplementedException();
	}

	public override void Bind() =>
		GL.AttachShader(_shaderProgram, _shaderObject);

	public override void Unbind() =>
		GL.AttachShader(_shaderProgram, _shaderObject);

	public override void Dispose() =>
		GL.DeleteShader(_shaderObject);

}

