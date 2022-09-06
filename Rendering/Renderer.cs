
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKMiniEngine.Rendering.Handles;
using OpenTKMiniEngine.Scenes.Components;

namespace OpenTKMiniEngine.Rendering;

public abstract class Renderer : IRenderingComponent {

	protected List<VertexBuffer> VertexBuffers { get; }
	protected ShaderProgram Shader { get; }
	protected VertexArray VertexArray { get; }

	public Renderer(ShaderProgram.ShaderInfo shaderInfo) {
		Shader = new ShaderProgram(shaderInfo);

		VertexBuffers = LoadBuffers();

		VertexArray = new VertexArray();
		VertexArray.SetVertexBufferList(VertexBuffers);
	}

	protected abstract List<VertexBuffer> LoadBuffers();

	public abstract void RenderUpdate(FrameEventArgs obj, GameWindow win);

	public abstract void Draw();

	public virtual void Render() {
		Shader.Bind();
		VertexArray.Bind();

		Draw();

		VertexArray.Unbind();
		ShaderProgram.Unbind();
	}

	public void Unload() {
		Shader.Dispose();
		VertexArray.Dispose();
		foreach(VertexBuffer buffer in VertexBuffers) buffer.Dispose();
	}

	public static void DrawTriangles(int verticesPrInstance) =>
		GL.DrawArrays(PrimitiveType.Triangles, 0, verticesPrInstance);

	public static void DrawTrianglesInstanced(int verticesPrInstance, int instanceCount) =>
		GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, verticesPrInstance, instanceCount);



	public static void DrawTriangleElemenets(int elementsPrInstance) =>
		GL.DrawElements(PrimitiveType.Triangles, elementsPrInstance, DrawElementsType.UnsignedInt, 0);

	public static void DrawTriangleElementsInstanced(int elementsPrInstance, int instanceCount) =>
		GL.DrawElementsInstanced(PrimitiveType.Triangles, elementsPrInstance, DrawElementsType.UnsignedInt, 0, instanceCount);



	public static void DrawTriangleStripElements(int elementsPrInstance) =>
		GL.DrawElements(PrimitiveType.TriangleStrip, elementsPrInstance, DrawElementsType.UnsignedInt, 0);

	public static void DrawTriangleStripElementsInstanced(int elementsPrInstance, int instanceCount) =>
		GL.DrawElementsInstanced(PrimitiveType.TriangleStrip, elementsPrInstance, DrawElementsType.UnsignedInt, 0, instanceCount);

}
