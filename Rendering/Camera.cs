
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Rendering.GLObjects;
using OpenTKEngine.Utility;

namespace OpenTKEngine.Rendering;

public interface IReadOnlyCamera
{
    public Vector3 Position { get; }

    public Vector3 Direction { get;}

    public Vector3 WorldUp { get; }

    public void AssignMatrices(ShaderProgram program, string viewUniformName, string projectionUniformName);

    public Vector2 WorldToScreenPosition(Vector3 worldPosition);

    public Vector3 ScreenToWorldPosition(Vector2 screenPosition);
}

public sealed class Camera: IReadOnlyCamera
{
    public Vector3 Position { get; set; }
    public Vector2 ScreenOffset { get; set; }

    public Vector3 Direction { get; set; }
    public float Zoom { get; set; }
    public float Rotation { get; set; }
    public Vector2 DepthRange { get; set; }
    public Vector3 WorldUp { get; set; }
    public Vector3 CameraUp
    {
        get => (new Vector4(WorldUp, 0) * Matrix4.CreateFromAxisAngle(Direction, Rotation)).Xyz;
        set => Rotation = Vector3.CalculateAngle(WorldUp, value);
    }

    public Vector2 ScreenSize { get; set; }
    public float Ratio { 
        get => ScreenSize.Y / ScreenSize.X;
        set => ScreenSize = Util.Scale(1f/value) * 2f; 
    }

    public bool IsOrthographic { get; set; }

    public Camera(Vector3 position, Vector3 direction, Vector2 screenOffset, float rotation, float zoom, Vector3 up, float ratio, bool isOrthographic)
    {
        Position = position;
        Direction = direction;
        Zoom = zoom;
        ScreenOffset = screenOffset;
        Rotation = rotation;
        DepthRange = new(1, 100);
        WorldUp = up;

        Ratio = ratio;
        IsOrthographic = isOrthographic;
    }

    public Camera(Vector3 position, Vector3 direction, bool isOrthographic) : this(
        position,
        direction,
        -Vector2.One,
        0f,
        1f,
        Vector3.UnitY,
        1f,
        isOrthographic
    )
    {
    }

    public Camera() : this(
        -Vector3.UnitZ,
        Vector3.UnitZ,
        true
    )
    {
    }

    private Matrix4 View
    {
        get => Matrix4.LookAt(Position, Position + Direction, CameraUp);
    }

    private Matrix4 Projection
    {
        get {
            Vector2 bottomLeft = ScreenSize * ScreenOffset / 2 * Zoom;
            Vector2 topRight = (ScreenSize * (ScreenOffset / 2 + Vector2.One)) * Zoom;
            Vector2 depth = DepthRange * Zoom;

            if(IsOrthographic)
            {
                return Matrix4.CreateOrthographicOffCenter(bottomLeft.X, topRight.X, bottomLeft.Y, topRight.Y, depth.X, depth.Y);
            }
            else
            {
                return Matrix4.CreatePerspectiveOffCenter(bottomLeft.X, topRight.X, bottomLeft.Y, topRight.Y, depth.X, depth.Y);
            }
        }
    }

    public void CenterOn(Vector2 screenPosition)
    {
        Position += ScreenToWorldPosition(ScreenOffset - screenPosition);
        ScreenOffset = screenPosition;
    }

    public void AssignMatrices(ShaderProgram program, string viewUniformName, string projectionUniformName)
    {
        program.GetUniform(viewUniformName).SetValue(View);
        program.GetUniform(projectionUniformName).SetValue(Projection);
    }

    public Vector2 WorldToScreenPosition(Vector3 worldPosition)
    {
        return (new Vector4(worldPosition, 1) * View * Projection).Xy;
    }

    public Vector3 ScreenToWorldPosition(Vector2 screenPosition)
    {
        return (new Vector4(screenPosition, 0, 1) * View.Inverted() * Projection.Inverted()).Xyz;
    }

}
