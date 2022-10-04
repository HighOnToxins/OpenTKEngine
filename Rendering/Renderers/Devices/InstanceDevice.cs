
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Rendering.Meshes;

namespace OpenTKEngine.Rendering.Renderers.Devices;

//types
public record InstanceRenderingInfo(
	bool IsStatic,
	string[] MeshFieldNames,
	params string[] InstanceFieldNames
);

public class InstanceDevice <V, I> : MeshDevice<V> where V : unmanaged where I : unmanaged{

	//fields
	private ArrayBuffer<I> _instanceBuffer;
	private readonly List<I> _instanceData;

	private readonly bool _isStatic;
	private readonly uint[] _instanceFieldLocations;
	protected int _instanceCount;

	//constructor
	public InstanceDevice(Mesh<V> mesh, ShaderProgram shaderProgram, InstanceRenderingInfo info) : 
		base(mesh, shaderProgram, info.MeshFieldNames) {

		_isStatic = info.IsStatic;
		_instanceFieldLocations = ShaderProgram.GetVariableLocations(info.InstanceFieldNames);
		_instanceCount = 0;

		_instanceData = new List<I>();

		_instanceBuffer = new(BufferTargetARB.ArrayBuffer, _instanceData.ToArray(), BufferUsageARB.DynamicDraw);
		VertexArray.SetBuffer(_instanceBuffer, 1, _instanceFieldLocations);
	}

	//adding data about instance
	public void AddInstanceData(I instance) =>
		_instanceData.Add(instance);
		
	//render update
	public override void RenderUpdate(FrameEventArgs obj, GameWindow win) {}

	//render
	public override void Render() {
		if(!_isStatic) {
			PreDraw();
		}
		base.Render();
	}

	private void PreDraw() {
		if(_instanceData.Count != _instanceCount) {
			_instanceCount = _instanceData.Count;

			_instanceBuffer = new(BufferTargetARB.ArrayBuffer, _instanceData.ToArray(), BufferUsageARB.DynamicDraw);
			VertexArray.SetBuffer(_instanceBuffer, 1, _instanceFieldLocations);
		} else {
			_instanceBuffer.BufferData(_instanceData.ToArray(), BufferUsageARB.DynamicDraw);
		}
		_instanceData.Clear();
	}

	//draw
	protected override void Draw() {
		GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, VerticiesPrInstance, _instanceCount);
	}

	public override void Unload() {
		base.Unload();
		_instanceBuffer.Dispose();
	}

}
