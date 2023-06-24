
using OpenTK.Graphics.OpenGL;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class Renderer: IDisposable
{
    private readonly ShaderProgram shaderProgram;
    private readonly VertexArray vertexArray;

    public Renderer(ShaderProgram shaderProgram, VertexArray vertexArray)
    {
        this.shaderProgram = shaderProgram;
        this.vertexArray = vertexArray;
    }

    public void Dispose()
    {
        shaderProgram.Dispose();
        vertexArray.Dispose();
    }

    public void Draw(PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        shaderProgram.Bind();
        vertexArray.Bind();
        GL.DrawArrays(primitiveType, 0, vertexArray.MaxVertexCount);
    }

    public void DrawElements(PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        if (vertexArray.ElementBuffer is null) throw new ArgumentException("No element buffer was attached to the vertex array!");

        shaderProgram.Bind();
        vertexArray.Bind();
        DrawElementsType elementType = (DrawElementsType)vertexArray.ElementBuffer.ValueType;
        GL.DrawElements(primitiveType, vertexArray.ElementBuffer.ValueCount, elementType, 0);
    }

    public void DrawInstanced(int instanceCount, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        shaderProgram.Bind();
        vertexArray.Bind();
        GL.DrawArraysInstanced(primitiveType, 0, vertexArray.MaxVertexCount, instanceCount);
    }

    public void DrawElementsInstanced(int instanceCount, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        if(vertexArray.ElementBuffer is null) throw new ArgumentException("No element buffer was attached to the vertex array!");

        shaderProgram.Bind();
        vertexArray.Bind();
        DrawElementsType elementType = (DrawElementsType)vertexArray.ElementBuffer.ValueType;
        GL.DrawElementsInstanced(primitiveType, vertexArray.ElementBuffer.ValueCount, elementType, 0, instanceCount);
    }
}
