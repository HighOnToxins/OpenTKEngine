
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering; 

public abstract class GLObject : IDisposable {

	public abstract ObjectIdentifier Identifier { get; }
	public abstract uint Handle { get; }

	private string _label;

	public string Label {
		set {
			_label = value;
			GL.ObjectLabel(Identifier, Handle, value.Length, value);
		}
		get => _label;
	}

	public GLObject() {
		_label = "";
	}

	public abstract void Bind();
	public abstract void Unbind();
	public abstract void Dispose();
}
