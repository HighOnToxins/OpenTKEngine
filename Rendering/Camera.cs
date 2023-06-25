
using OpenTK.Mathematics;
using OpenTKEngine.Rendering.GLObjects;

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
    public float Angle { get; set; }

    public Vector2 ScreenSize { get; set; }
    public Vector2 DepthRange { get; set; }

    public Vector3 WorldUp { get; set; }
    public Vector3 CameraUp
    {
        get => (new Vector4(WorldUp, 0) * Matrix4.CreateFromAxisAngle(Direction, Angle)).Xyz;
        set => Angle = Vector3.CalculateAngle(WorldUp, value); 
    }

    private readonly bool isOrthographic;

    public Camera(Vector3 position, Vector3 direction, float angle, float zoom, Vector3 up, bool isOrthographic)
    {
        Position = position;
        Direction = direction;
        Zoom = zoom;
        Angle = angle;

        WorldUp = up;

        ScreenSize = new(2, 2);
        DepthRange = new(1, 100);

        this.isOrthographic = isOrthographic;
        ScreenOffset = new(0, 0);
    }

    public Camera(Vector3 position, Vector3 direction, bool isOrthographic) : this(
        position,
        direction,
        0f,
        1f,
        Vector3.UnitY,
        isOrthographic
    )
    {
    }

    public Camera() : this(
        new Vector3(0, 0, -1), 
        new Vector3(0, 0, 1), 
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
        get => isOrthographic
            ? Matrix4.CreateOrthographicOffCenter(
                ScreenOffset.X + Position.X * Zoom, ScreenOffset.X + (Position.X + ScreenSize.X) * Zoom,
                ScreenOffset.Y + Position.Y * Zoom, ScreenOffset.Y + (Position.Y + ScreenSize.Y) * Zoom,
                DepthRange.X * Zoom, DepthRange.Y * Zoom)
            : Matrix4.CreatePerspectiveOffCenter(
                ScreenOffset.X + Position.X * Zoom, ScreenOffset.X + (Position.X + ScreenSize.X) * Zoom,
                ScreenOffset.Y + Position.Y * Zoom, ScreenOffset.Y + (Position.Y + ScreenSize.Y) * Zoom,
                DepthRange.X * Zoom, DepthRange.Y * Zoom);
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
