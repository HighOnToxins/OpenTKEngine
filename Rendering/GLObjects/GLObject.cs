
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public abstract class GLObject : IDisposable
{
    public abstract ObjectIdentifier Identifier { get; }
    protected abstract uint Handle { get; }

    public GLObject()
    {
    }

    private string? label;
    public string Label
    {
        get
        {
            if(label is null)
            {
                int length = 0;
                int maxLabelLength = 0;
                GL.GetInteger(GetPName.MaxLabelLength, ref maxLabelLength);
                label = GL.GetObjectLabel(Identifier, Handle, maxLabelLength, ref length);
                return label;
            }

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
