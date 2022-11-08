
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Items;

public class Camera: IReadOnlyCamera {

    //properties
    public Vector3 Position { get; set; }
    public Vector3 Direction { get; set; }
    public float Scale { get; set; }
    public float Angle { get; set; }

    private Vector2 _position;
    public Vector2 ScreenPosition {
        get {
            if(ScreenPositionIsCenterized) {
                return -ScreenSize / 2f;
            } else {
                return _position;
            }
        }
        set {
            ScreenPositionIsCenterized = false;
            _position = value;
        }
    }

    public Vector2 ScreenSize { get; set; }
    public Vector2 DepthRange { get; set; }


    public Vector3 Up { get; set; }
    public Vector3 CameraUp {
        get => (new Vector4(Up, 1) * Matrix4.CreateFromAxisAngle(Direction, Angle)).Xyz;
        set {
            Up = value;
            Angle = 0;
        }
    }
    public bool ScreenPositionIsCenterized { get; set; }

    //fields
    private readonly bool _isOrthographic;

    //constructor

    public Camera() {
        Position = new(0, 0, -1);
        Direction = new(0, 0, 1);
        Scale = 1;
        Angle = 0;

        Up = Vector3.UnitY;

        ScreenPosition = new(-1, -1);
        ScreenSize = new(2, 2);
        DepthRange = new(1, 100);

        _isOrthographic = true;
        ScreenPositionIsCenterized = false;
    }

    public Camera(Vector3 position, Vector3 direction, bool isOrthographic) {
        Position = position;
        Direction = direction;
        Scale = 1;
        Angle = 0;

        Up = Vector3.UnitY;

        ScreenPosition = new(-1, -1);
        ScreenSize = new(2, 2);
        DepthRange = new(1, 100);

        _isOrthographic = isOrthographic;
        ScreenPositionIsCenterized = false;
    }

    public Camera(Vector3 position, Vector3 direction, float angle, float scale, Vector3 up, bool isOrthographic) {
        Position = position;
        Direction = direction;
        Scale = scale;
        Angle = angle;

        Up = up;

        ScreenPosition = new(-1, -1);
        ScreenSize = new(2, 2);
        DepthRange = new(1, 100);

        _isOrthographic = isOrthographic;
        ScreenPositionIsCenterized = false;
    }

    //additional properties
    public Matrix4 ViewMatrix {
        get => Matrix4.LookAt(Position, Position + Direction, CameraUp);
    }

    public Matrix4 ProjMatrix {
        get => _isOrthographic
            ? Matrix4.CreateOrthographicOffCenter(
                ScreenPosition.X * Scale, (ScreenPosition.X + ScreenSize.X) * Scale,
                ScreenPosition.Y * Scale, (ScreenPosition.Y + ScreenSize.Y) * Scale, 
                DepthRange.X * Scale, DepthRange.Y * Scale)
            : Matrix4.CreatePerspectiveOffCenter(
                ScreenPosition.X * Scale, (ScreenPosition.X + ScreenSize.X) * Scale,
                ScreenPosition.Y * Scale, (ScreenPosition.Y + ScreenSize.Y) * Scale, 
                DepthRange.X * Scale, DepthRange.Y * Scale);
    }

    public Matrix4 World2Screen {
        get => ViewMatrix * ProjMatrix;
    }

    public Vector2 GetPositionForCenterization()
        => -ScreenSize / 2;

}
