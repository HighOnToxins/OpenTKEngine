using OpenTK.Mathematics;
using OpenTKEngine.Rendering.Items;
using OpenTKEngine.Rendering.Renderers;

public class ColorRenderer: InstanceRenderer<ColorVertex> {
    public ColorRenderer(Mesh mesh) : base(mesh) { }

    public void AddInstanceData(Vector3 position, Vector4 color, Quaternion angle, float scale = 1) {
        Matrix4 translationMatrix = Matrix4.CreateTranslation(position);
        Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(angle);
        Matrix4 scaleMatrix = Matrix4.CreateScale(scale);

        AddInstanceData(new ColorVertex(translationMatrix * rotationMatrix * scaleMatrix, color));
    }

    public void AddInstanceData(Vector3 position, Vector4 color, float scale = 1) {
        AddInstanceData(position, color, new Quaternion(0, 0, 0), scale);
    }
}