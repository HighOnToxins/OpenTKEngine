using OpenTK.Graphics.OpenGL;
using OpenTKMiniEngine.Rendering.Vertices;
using System.Runtime.InteropServices;

namespace OpenTKMiniEngine.Rendering.Handles;

public class VertexBuffer : Buffer {

	private int _dataSize;
	private Type _vertexType;

	public uint[] VariableLocations { get; protected set; }
	public int[] FieldSizes { get; protected set; }
	public VertexAttribPointerType PointerType { get; protected set; }
	public int Stride { get; protected set; }
	public int TypeSize { get; protected set; }
	public uint Divisor { get; protected set; }

	public VertexBuffer(
			string name, 
			int dataSize, 
			uint[] variableLocations, 
			int[] fieldSizes,
			VertexAttribPointerType pointerType, 
			int stride, 
			int typeSize, 
			uint divisor = 0) : base(name) {
		_vertexType = typeof(IVertex);
		_dataSize = dataSize;

		VariableLocations = variableLocations;
		FieldSizes = fieldSizes;
		PointerType = pointerType;
		Stride = stride;
		TypeSize = typeSize;
		Divisor = divisor;
	}

	public VertexBuffer(string name) : base(name){
		_vertexType = typeof(IVertex);
		VariableLocations = Array.Empty<uint>();
		FieldSizes = Array.Empty<int>();
		PointerType = 0;
		Stride = 0;
		TypeSize = 0;
		Divisor = 0;
	}

	protected void Init<V>(
		int dataSize, 
		uint[] variableLocations, 
		uint divisor = 0) where V : unmanaged, IVertex {
		VertexAttribs attribs = new V().GetAttribs();

		TestLocationAndSizeLengths(variableLocations.Length, attribs.FieldSizes.Length);

		_vertexType = typeof(V);
		_dataSize = dataSize;
		VariableLocations = variableLocations;
		FieldSizes = attribs.FieldSizes;
		PointerType = TypeToPointerType(attribs.Type);
		Stride = Marshal.SizeOf<V>();
		Divisor = divisor;
		TypeSize = Marshal.SizeOf(attribs.Type);
	}

	protected void Init<V>(int dataSize, ShaderProgram shader, string[] variableNames, uint divisor = 0) where V : unmanaged, IVertex =>
		Init<V>(dataSize, GetVariableLocations(shader, variableNames), divisor);

	protected void ElementInit(int dataSize) {
		_vertexType = typeof(uint);
		_dataSize = dataSize;
		VariableLocations = Array.Empty<uint>();
		FieldSizes = Array.Empty<int>();
		PointerType = VertexAttribPointerType.UnsignedInt;
		Stride = sizeof(uint);
		Divisor = 0;
		TypeSize = sizeof(uint);
	}

	private static uint[] GetVariableLocations(ShaderProgram shader, string[] variableNames) {
		uint[] variableLocations = new uint[variableNames.Length];

		for(int i = 0; i < variableLocations.Length; i++) {

			int a = shader.GetVariableLocation(variableNames[i]);
			if(a < 0) throw new ArgumentException("Variable name was not found.");
			variableLocations[i] = (uint)a;
		}

		return variableLocations;
	}

	private static void TestLocationAndSizeLengths(int locationLength, int sizeLength) {
		if(locationLength == sizeLength) return;

		throw new ArgumentException(
			"The number of vertex " +
			"properties did not match " +
			"the given amount of " +
			"variable names.\n " +
			"Guessing location is not " +
			"suported.");
	}

	private static VertexAttribPointerType TypeToPointerType(Type type) {

		switch(type.Name) {
			case nameof(Byte): return VertexAttribPointerType.Byte;
			case nameof(SByte): return VertexAttribPointerType.UnsignedByte;

			case nameof(UInt16): return VertexAttribPointerType.UnsignedShort;
			case nameof(Int16): return VertexAttribPointerType.Short;

			case nameof(UInt32): return VertexAttribPointerType.UnsignedInt;
			case nameof(Int32): return VertexAttribPointerType.Int;

			case nameof(Single): return VertexAttribPointerType.Float;

			case nameof(Double): return VertexAttribPointerType.Double;
		};

		throw new ArgumentException("The given type was not suported.");
	}


	public override void UploadBufferData<T>(T[] data) {
		TestDataType(_vertexType, typeof(T));
		TestDataSize(_dataSize, data.Length);

		Bind();
		unsafe {
			fixed(T* dataPtr = data) {
				GL.BufferSubData(Target, (IntPtr)0, _dataSize * Stride, dataPtr);
			}
		}
		Unbind();
	}

	private static void TestDataType(Type expectedType, Type givenType) {
		if(givenType.Equals(expectedType)) return;

		throw new ArgumentException(
			"The type of the given data" +
			"does not match with the type" +
			"of the buffer's data.");
	}

	private static void TestDataSize(int expectedSize, int givenSize) {
		if(expectedSize == givenSize) return;

		throw new ArgumentException(
			"The the size of the given " +
			"data array does not match " +
			"with the size of the buffer.");
	}
}
