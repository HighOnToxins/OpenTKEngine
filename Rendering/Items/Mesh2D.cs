using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Items;

public static class Mesh2D {

    public static Mesh<Vector3> CreateBox() {
        return new Mesh<Vector3>(
            new Vector3[] {
                new(-0.5f, -0.5f, 0),
                new(0.5f, 0.5f, 0),
                new(-0.5f, 0.5f, 0),

                new(-0.5f, -0.5f, 0),
                new(0.5f, 0.5f, 0),
                new(0.5f, -0.5f, 0),
            }
        );
    }


}
