
using OpenTKEngine.Rendering.GLObjects;
using System.Diagnostics.CodeAnalysis;

namespace OpenTKEngine.Rendering;

public sealed class Graphics
{
    private readonly Dictionary<IMaterial, IGraphicsBatchList> batchLists;

    public IReadOnlyCamera? Camera { get; set; }

    public Graphics(IReadOnlyCamera? camera = null) 
    {
        Camera = camera;
        batchLists = new Dictionary<IMaterial, IGraphicsBatchList>();
    }

    public void Add<T1, T2>(Material<T1, T2> material, Mesh<T1> mesh, T2 instance) where T1 : unmanaged where T2 : unmanaged
    {
        if(!batchLists.TryGetValue(material, out IGraphicsBatchList? materGroup) || materGroup is not GraphicsBatchList<T1, T2> batchList)
        {
            batchList = new GraphicsBatchList<T1, T2>(material);
            batchLists.Add(material, batchList);
        }

        if(!batchList.TryGetValue(mesh, out GraphicsBatch<T1, T2>? batch))
        {
            batch = new GraphicsBatch<T1, T2>(material, mesh);
            batchList.Add(mesh, batch);
        }

        batch.Add(instance);
    }

    //TODO: Add the ability to change an existing shape.

    //TODO: Add the ability to add constant shapes, that aren't updated each time.

    public void Clear()
    {
        foreach(IGraphicsBatchList batchList in batchLists.Values) 
        {
            batchList.Clear();
        }
    }

    public void Draw()
    {
        foreach(IGraphicsBatchList materialGroup in batchLists.Values)
        {
            materialGroup.Draw(Camera);
        }
    }
    
    private sealed class GraphicsBatch<T1, T2> where T1 : unmanaged where T2 : unmanaged
    {
        private readonly VertexArray array;
        private readonly VertexBuffer<T2> instanceBuffer;
        private readonly List<T2> instances;

        public GraphicsBatch(Material<T1, T2> material, Mesh<T1> mesh)
        {
            array = new();
            instanceBuffer = new();
            mesh.SetBuffers(array, material.MeshAttribute);
            array.SetBuffer(instanceBuffer, 1, material.InstanceAttributes);
            instances = new List<T2>();
        }

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

    private interface IGraphicsBatchList
    {
        public void Draw(IReadOnlyCamera? camera);
        public void Clear();
    }

    private sealed class GraphicsBatchList<T1, T2>: IGraphicsBatchList where T1 : unmanaged where T2 : unmanaged
    {
        private readonly Material<T1, T2> material;
        private readonly Dictionary<Mesh<T1>, GraphicsBatch<T1, T2>> groups;

        public GraphicsBatchList(Material<T1, T2> material)
        {
            this.material = material;
            groups = new();
        }

        public bool TryGetValue(Mesh<T1> mesh, [NotNullWhen(true)] out GraphicsBatch<T1, T2>? result)
        {
            return groups.TryGetValue(mesh, out result);
        }

        public void Add(Mesh<T1> mesh, GraphicsBatch<T1, T2> instanceGroup)
        {
            groups.Add(mesh, instanceGroup);
        }

        public void Clear()
        {
            foreach(GraphicsBatch<T1, T2> group in groups.Values)
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

            foreach(GraphicsBatch<T1, T2> group in groups.Values)
            {
                group.Draw(material.Shader);
            }
        }
    }
}
