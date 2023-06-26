
using OpenTK.Mathematics;

namespace OpenTKEngine.Utility;

public struct Shape
{
    public Shape(Vector3 position, Vector3 size, Quaternion quaternion, Vector4 color)
    {
        model = Matrix4.CreateScale(size) * Matrix4.CreateFromQuaternion(quaternion) * Matrix4.CreateTranslation(position);
        this.color = color;
    }

    public Shape(Vector3 position, Vector3 size, Vector3 normal, float angle, Vector4 color) :
        this(position, size, Quaternion.FromAxisAngle(normal, angle), color)
    { }

    public Matrix4 model;

    public Vector4 color;
}
