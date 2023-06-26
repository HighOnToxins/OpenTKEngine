
using OpenTKEngine.Rendering.GLObjects;
using System.Diagnostics.CodeAnalysis;

namespace OpenTKEngine.Rendering;

public sealed class Graphics
{
    private readonly Dictionary<IMaterial, IMaterialGroup> groups;

    public IReadOnlyCamera? Camera { get; set; }

    public Graphics(IReadOnlyCamera? camera = null) 
    {
        Camera = camera;
        groups = new Dictionary<IMaterial, IMaterialGroup>();
    }

    public void Add<T1, T2>(Material<T1, T2> material, Mesh<T1> mesh, T2 instance) where T1 : unmanaged where T2 : unmanaged
    {
        if(!groups.TryGetValue(material, out IMaterialGroup? materGroup) || materGroup is not MaterialGroup<T1, T2> materialGroup)
        {
            materialGroup = new MaterialGroup<T1, T2>(material);
            groups.Add(material, materialGroup);
        }

        if(!materialGroup.TryGetValue(mesh, out InstanceGroup<T1, T2>? instanceGroup))
        {
            instanceGroup = new InstanceGroup<T1, T2>(material, mesh);
            materialGroup.Add(mesh, instanceGroup);
        }

        instanceGroup.Add(instance);

    }

    //TODO: Add the ability to change an existing shape.

    //TODO: Add the ability to add constant shapes, that aren't updated each time.

    public void Clear()
    {
        foreach(IMaterialGroup materialGroup in groups.Values) 
        {
            materialGroup.Clear();
        }
    }

    public void Draw()
    {
        foreach(IMaterialGroup materialGroup in groups.Values)
        {
            materialGroup.Draw(Camera);
        }
    }
    
    private sealed class InstanceGroup<T1, T2> where T1 : unmanaged where T2 : unmanaged
    {
        private readonly VertexArray array;
        private readonly VertexBuffer<T2> instanceBuffer;
        private readonly List<T2> instances;

        public InstanceGroup(Material<T1, T2> material, Mesh<T1> mesh)
        {
            array = new();
            instanceBuffer = new();
            mesh.SetBuffers(array, material.MeshAttribute);
            array.SetBuffer(instanceBuffer, 1, material.InstanceAttributes);
            instances = new List<T2>();
        }

        //TODO: Keep track of what was updated.

        public void Add(T2 instance)
        {
            instances.Add(instance);
        }

        public void Clear()
        {
            instances.Clear();
        }

        public void Draw(ShaderProgram shader)
        {
            instanceBuffer.SetData(instances.ToArray());
            shader.Draw(array);
        }
    }

    private interface IMaterialGroup
    {
        public void Draw(IReadOnlyCamera? camera);
        public void Clear();
    }

    private sealed class MaterialGroup<T1, T2>: IMaterialGroup where T1 : unmanaged where T2 : unmanaged
    {
        private readonly Material<T1, T2> material;
        private readonly Dictionary<Mesh<T1>, InstanceGroup<T1, T2>> groups;

        public MaterialGroup(Material<T1, T2> material)
        {
            this.material = material;
            groups = new();
        }

        public bool TryGetValue(Mesh<T1> mesh, [NotNullWhen(true)] out InstanceGroup<T1, T2>? result)
        {
            return groups.TryGetValue(mesh, out result);
        }

        public void Add(Mesh<T1> mesh, InstanceGroup<T1, T2> instanceGroup)
        {
            groups.Add(mesh, instanceGroup);
        }

        public void Clear()
        {
            foreach(InstanceGroup<T1, T2> group in groups.Values)
            {
                group.Clear();
            }
        }

        public void Draw(IReadOnlyCamera? camera)
        {
            if(material.UsesCamera && camera is not null)
            {
                camera.AssignMatrices(material.Shader, material.ViewUniformName, material.ProjectionUniformName);
            }

            foreach(InstanceGroup<T1, T2> group in groups.Values)
            {
                group.Draw(material.Shader);
            }
        }
    }
}
