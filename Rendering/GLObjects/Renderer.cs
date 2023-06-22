
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class Renderer
{
    private readonly ShaderProgram shaderProgram;
    private readonly VertexArray vertexArray;

    public Renderer(ShaderProgram shaderProgram, VertexArray vertexArray)
    {
        this.shaderProgram = shaderProgram;
        this.vertexArray = vertexArray;
    }

    public void Draw(int first, int count)
    {
        shaderProgram.Bind();
        vertexArray.Bind();
        GL.DrawArrays(PrimitiveType.Triangles, first, count);
    }

    public void DrawElements()
    {
        if(vertexArray.ElementBuffer is null) throw new ArgumentException("No element buffer was attached to the vertex array!");
        if(vertexArray.ElementBuffer is null) throw new ArgumentException("The element buffer did not contain any elements!");

        DrawElementsType ElementType = vertexArray.ElementBuffer.Type.Name switch
        {
            nameof(UInt32) => DrawElementsType.UnsignedInt,
            nameof(UInt16) => DrawElementsType.UnsignedShort,
            nameof(Byte) => DrawElementsType.UnsignedByte,
            _ => throw new ArgumentException("The type of the element buffer was not of a possible type!"),
        };

        shaderProgram.Bind();
        vertexArray.Bind();
        GL.DrawElements(PrimitiveType.Triangles, vertexArray.ElementBuffer.Count, ElementType, 0);
    }

}
