
namespace OpenTKEngine.Rendering.GLObjects;

public record BufferFields(
    params (uint Index, int Size)[] Field
);
