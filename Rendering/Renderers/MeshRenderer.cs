
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering.Meshes;
using OpenTKEngine.Scenes.Components;

namespace OpenTKEngine.Rendering.Renderers;

public class MeshRenderer: IRenderingComponent {

    public string MeshFieldNameInclusion = "mesh";

    //propterties
    protected Mesh Mesh { get; private init; }
    protected ShaderProgram ShaderProgram { get; private set; }
    protected VertexArray VertexArray { get; private set; }

    //constructor
    public MeshRenderer(Mesh mesh) {
        ShaderProgram = ShaderProgram.EmptyShader;
        VertexArray = VertexArray.EmptyVertex;
        Mesh = mesh;
    }

    public virtual void AssignShader(ShaderProgram shader) {
        ShaderProgram = shader;
        VertexArray = Mesh.CreateVertexArray(ShaderProgram.VariableNames
            .Where(n => n.Contains(MeshFieldNameInclusion, StringComparison.OrdinalIgnoreCase))
            .Select(n => ShaderProgram.GetVariableLocation(n))
            .ToArray()); 
    }

    /// <summary> An update happening before any draw method is called. </summary>
    public void RenderUpdate(FrameEventArgs obj, GameWindow win) { }

    protected virtual void Draw() {
        if(Mesh.HasNoElements) {
            GL.DrawArrays(PrimitiveType.Triangles, 0, Mesh.VertexCount);
        } else {
            GL.DrawElements(PrimitiveType.Triangles, Mesh.ElementCount, DrawElementsType.UnsignedInt, 0);
        }
    }

    public virtual void Render() {
        ShaderProgram.Bind();
        VertexArray.Bind();

        Draw();

        VertexArray.Unbind();
        ShaderProgram.Unbind();
    }

    public virtual void Unload() {
        ShaderProgram.Dispose();
        VertexArray.Dispose();
    }

}
