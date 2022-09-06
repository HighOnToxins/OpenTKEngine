
namespace OpenTKMiniEngine.Rendering.Vertices;

public record VertexAttribs(Type Type, params int[] FieldSizes);

//TODO: Use attributes instead of an interface method for vertex fields.
public interface IVertex {
	public VertexAttribs GetAttribs();
}
