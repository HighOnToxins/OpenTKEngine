
using OpenTK.Mathematics;

namespace OpenTKEngine.Rendering.Renderers.Camera;

public class Camera : IReadOnlyCamera {

	//properties
	public Vector3 Position { get; set; }
	public Vector3 Direction { get; set; }
	public float Scale { get; set; }
	public float Angle { get; set; }

	public float Left { get; set; }
	public float Right { get; set; }
	public float Bottom { get; set; }
	public float Top { get; set; }
	public float Near { get; set; }
	public float Far { get; set; }

	public Vector3 Up { get; set; }
	public Vector3 CameraUp {
		get => (new Vector4(Up, 1) * Matrix4.CreateFromAxisAngle(Direction, Angle)).Xyz;
		set{ 
			Up = value;
			Angle = 0;
		}
	}
	//fields
	private readonly bool _isOrthographic;
	
	//constructor
	public Camera(Vector3 position, Vector3 direction, float angle, float scale, Vector3 up, bool isOrthographic) {
		Position = position;
		Direction = direction;
		Scale = scale;
		Angle = angle;

		Up = up;

		Left = -1;
		Right = 1;
		Bottom = -1;
		Top = 1;
		Near = 1f;
		Far = 100f;

		_isOrthographic = isOrthographic;
	}

	//additional properties
	public Matrix4 ViewMatrix {
		get => Matrix4.LookAt(Position, Position + Direction, CameraUp);
	}

	public Matrix4 ProjMatrix {
		get => _isOrthographic
			? Matrix4.CreateOrthographicOffCenter(Left * Scale, Right * Scale, Bottom * Scale, Top * Scale, Near, Far)
			: Matrix4.CreatePerspectiveOffCenter(Left * Scale, Right * Scale, Bottom * Scale, Top * Scale, Near, Far);
	}

	public Matrix4 World2Screen {
		get => ViewMatrix * ProjMatrix;
	}
}
