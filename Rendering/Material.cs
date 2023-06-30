using OpenTK.Mathematics;
using OpenTKEngine.Properties;
using OpenTKEngine.Rendering.GLObjects;
using OpenTKEngine.Utility;

namespace OpenTKEngine.Rendering;

public sealed class Material<T1, T2>: IMaterial
{
    public ShaderProgram Shader { get; private init; }

    public bool UsesCamera { get; private init; }
    public string ViewUniformName { get; private init; }
    public string ProjectionUniformName { get; private init; }
    public ProgramAttribute MeshAttribute { get; internal set; }
    public ProgramAttribute[] InstanceAttributes { get; internal set; } //TODO: Make Instancing Optional!

    //TODO: Add type restrictions for T1 and T2, such that they must match with program attributes.

    private Material(ShaderProgram shader, bool usesCamera, string viewUniformName, string projectionUniformName, string meshAttributeName, params string[] instanceAttributeNames)
    {
        Shader = shader;
        UsesCamera = usesCamera;
        ViewUniformName = viewUniformName;
        ProjectionUniformName = projectionUniformName;
        MeshAttribute = shader.GetAttribute(meshAttributeName);
        InstanceAttributes = instanceAttributeNames.Select(shader.GetAttribute).ToArray();
    }

    public Material(ShaderProgram shader, string viewUniformName, string projectionUniformName, string meshAttributeName, params string[] instanceAttributeNames) :
        this(shader, true, viewUniformName, projectionUniformName, meshAttributeName, instanceAttributeNames)
    {
    }

    public Material(ShaderProgram shader, string meshAttributeName, params string[] instanceAttributeNames) :
        this(shader, false, "", "", meshAttributeName, instanceAttributeNames)
    {
    }
}

public interface IMaterial
{
    public static readonly Material<Vector3, Shape> Shape = new(
        ShaderProgram.LoadFromMemory(Resources.shapeshaderv, Resources.shapeshaderf),
        "view", "proj", "pos", "model", "vColor"
    );
}
