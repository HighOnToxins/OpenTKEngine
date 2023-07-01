
using OpenTK.Mathematics;

namespace OpenTKEngine.Utility;

public struct ShapeInstance
{
    public ShapeInstance(Vector3 position, Vector3 size, Quaternion quaternion, Vector4 color)
    {
        model = Matrix4.CreateScale(size) * Matrix4.CreateFromQuaternion(quaternion) * Matrix4.CreateTranslation(position);
        this.color = color;
    }

    public ShapeInstance(Vector3 position, Vector3 size, Vector3 normal, float rotation, Vector4 color) :
        this(position, size, Quaternion.FromAxisAngle(normal, rotation), color)
    { }

    public ShapeInstance(Vector3 position, Vector3 size, Vector4 color) :
        this(position, size, Quaternion.Identity, color)
    { }

    public ShapeInstance(Vector2 position, Vector2 size, float rotation, Vector4 color) : 
        this(new Vector3(position, 0), new Vector3(size, 1), Vector3.UnitZ, rotation, color)
    { }

    public ShapeInstance(Vector2 position, Vector2 size, Vector4 color) :
        this(new Vector3(position, 0), new Vector3(size, 1), color)
    { }

    public Matrix4 model;

    public Vector4 color;
}
