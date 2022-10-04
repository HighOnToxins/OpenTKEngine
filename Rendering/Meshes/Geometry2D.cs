using OpenTK.Mathematics;
using OpenTKEngine.Rendering.Vertices;

namespace OpenTKEngine.Rendering.Meshes; 

public class Geometry2D : ElementMesh<ColorVertex> {

	public Geometry2D(float size, params Vector4[] vertexColors) {

		Vertices = new ColorVertex[vertexColors.Length];

		double deltaAngle = 2 * Math.PI / Vertices.Length;
		for(int i = 0; i < Vertices.Length; i++) {
			double angle = i * deltaAngle;
			Vertices[i] = new ColorVertex(
				new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0) * size,
				vertexColors[i]
			);
		}

		Elements = new uint[(Vertices.Length - 2) * 3];
		uint a = 1;
		for(int i = 0; i < Elements.Length; i += 3) {
			Elements[i] = 0;
			Elements[i+1] = a;
			Elements[i+2] = a+1;
			a++;
		}

	}
}
