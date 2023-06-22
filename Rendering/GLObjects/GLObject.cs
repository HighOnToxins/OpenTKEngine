
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public abstract class GLObject : IDisposable
{
    public abstract ObjectIdentifier Identifier { get; }
    public abstract uint Handle { get; }

    public string Label
    {
        set
        {
            GL.ObjectLabel(Identifier, Handle, value.Length, value);
        }
        get
        {
            int length = 0;
            return GL.GetObjectLabel(Identifier, Handle, 10, ref length);
        }
    }

    public abstract void Bind();
    public abstract void Dispose();

}
