
using OpenTK.Graphics.OpenGL;
using OpenTKEngine.Rendering.Meshes;

namespace OpenTKEngine.Rendering.Renderers.Devices;

public class InstanceDevice<I> : MeshDevice where I : unmanaged {

	//constants
	public const string InstanceFieldLabel = "instanceField";

	//fields
	private VertexBuffer<I> _instanceBuffer;
	private readonly List<I> _instanceData;

	private readonly uint[] _instanceFieldLocations;
	protected int _instanceCount;

	//constructor
	public InstanceDevice(Mesh mesh, ShaderProgram shaderProgram) :
		base(mesh, shaderProgram) {

		_instanceFieldLocations = ShaderProgram.GetLocationsFromLabel(InstanceFieldLabel);
		_instanceCount = 0;

		_instanceData = new List<I>();

		_instanceBuffer = new(BufferTargetARB.ArrayBuffer, _instanceData.ToArray(), BufferUsageARB.DynamicDraw);
		VertexArray.SetBuffer(_instanceBuffer, 1, _instanceFieldLocations);
	}

	//adding data about instance
	public void AddInstanceData(I instance) =>
		_instanceData.Add(instance);

	//render
	public override void Render() {
		PreDraw();
		base.Render();
	}

	private void PreDraw() {
		if(_instanceData.Count != _instanceCount) {
			_instanceCount = _instanceData.Count;

            _instanceBuffer.Dispose();
            _instanceBuffer = new(BufferTargetARB.ArrayBuffer, _instanceData.ToArray(), BufferUsageARB.DynamicDraw);
			VertexArray.SetBuffer(_instanceBuffer, 1, _instanceFieldLocations);
		} else {
			_instanceBuffer.BufferData(_instanceData.ToArray(), BufferUsageARB.DynamicDraw);
		}
		_instanceData.Clear();
	}

	//draw
	protected override void Draw() {
		if(Mesh.HasNoElements) {
			GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, Mesh.VertexCount, _instanceCount); 
		} else {
			GL.DrawElementsInstanced(PrimitiveType.Triangles, Mesh.ElementCount, DrawElementsType.UnsignedShort, 0, _instanceCount);
		}
	}

	public override void Unload() {
		base.Unload();
		_instanceBuffer.Dispose();
	}

}