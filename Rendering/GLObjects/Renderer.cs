﻿
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
        GL.DrawArrays(primitiveType, 0, vertexArray.VertexCount);
    }

    public void DrawElements(PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        if (vertexArray.ElementBuffer is null) throw new ArgumentException("No element buffer was attached to the vertex array!");

        DrawElementsType elementType = GetElementType(vertexArray.ElementBuffer.Type);

        shaderProgram.Bind();
        vertexArray.Bind();
        GL.DrawElements(primitiveType, vertexArray.ElementBuffer.Count, elementType, 0);
    }

    public void DrawInstanced(int instanceCount, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        shaderProgram.Bind();
        vertexArray.Bind();
        GL.DrawArraysInstanced(primitiveType, 0, vertexArray.VertexCount, instanceCount);
    }

    public void DrawElementsInstanced(int instanceCount, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        if(vertexArray.ElementBuffer is null) throw new ArgumentException("No element buffer was attached to the vertex array!");

        DrawElementsType elementType = GetElementType(vertexArray.ElementBuffer.Type);

        shaderProgram.Bind();
        vertexArray.Bind();
        GL.DrawElementsInstanced(primitiveType, vertexArray.ElementBuffer.Count, elementType, 0, instanceCount);
    }

    private static DrawElementsType GetElementType(Type type)
    {
        return type.Name switch
        {
            nameof(UInt32) => DrawElementsType.UnsignedInt,
            nameof(UInt16) => DrawElementsType.UnsignedShort,
            nameof(Byte) => DrawElementsType.UnsignedByte,
            _ => throw new ArgumentException("The type of the element buffer was not of a possible type!"),
        };
    }
}