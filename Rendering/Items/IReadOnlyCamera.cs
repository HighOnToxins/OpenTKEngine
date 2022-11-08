
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Items;

public interface IReadOnlyCamera {
    public Matrix4 ViewMatrix { get; }
    public Matrix4 ProjMatrix { get; }

    public Matrix4 World2Screen {
        get => ViewMatrix * ProjMatrix;
    }
}
