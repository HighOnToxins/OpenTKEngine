
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public abstract class GLObject : IDisposable
{
    public abstract ObjectIdentifier Identifier { get; }
    protected abstract uint Handle { get; }

    public GLObject()
    {
        int length = 0;
        label = GL.GetObjectLabel(Identifier, Handle, 255, ref length);
    }

    private string label;
    public string Label
    {
        get
        {
            return label;
        }
        set
        {
            GL.ObjectLabel(Identifier, Handle, value.Length, value);
            label = value;
        }
    }

    public abstract void Bind();
    public abstract void Dispose();

}
