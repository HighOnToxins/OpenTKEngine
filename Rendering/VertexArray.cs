
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenTKEngine.Rendering;

public sealed class VertexArray: GLObject {

    //fields
    private readonly VertexArrayHandle _vertexArrayObject;

    //properties
    public override ObjectIdentifier Identifier { get => ObjectIdentifier.VertexArray; }
    public override uint Handle { get => (uint)_vertexArrayObject.Handle; }

    public static VertexArray EmptyVertex => new(false);

    //constructors
    public VertexArray() {
        _vertexArrayObject = GL.GenVertexArray();
    }


    public VertexArray(bool b) { }

    private void SetAttribute<V>(VertexBuffer<V> buffer,
            uint index, int count,
            VertexAttribPointerType type,
            int stride, int offset, uint divisor = 0)
            where V : unmanaged {

        Bind();
        buffer.Bind();

        GL.EnableVertexAttribArray(index);
        GL.VertexAttribPointer(index, count, type, false, stride, offset);
        GL.VertexAttribDivisor(index, divisor);

        buffer.Unbind();
        Unbind();
    }

    public void SetElementBuffer<V>(VertexBuffer<V> elementBuffer) where V : unmanaged {
        Bind();
        elementBuffer.Bind();
        Unbind();
        elementBuffer.Unbind();
    }

    public void SetBuffer<V>(VertexBuffer<V> buffer, uint divisor, params uint[] variableLocations) where V : unmanaged {

        if(buffer.Target == BufferTargetARB.ElementArrayBuffer) {
            SetElementBuffer(buffer);
            return;
        }

        VertexAttribute attrib = GetVertexAttribute<V>();

        if(variableLocations.Length != attrib.FieldSizes.Length) {
            throw new ArgumentException("The number of vertex properties did not match the given amount of variable names.\n Guessing location is not suported.");
        }

        int offset = 0;
        for(int i = 0; i < variableLocations.Length; i++) {
            SetAttribute(buffer, variableLocations[i], attrib.FieldSizes[i], attrib.PointerType, attrib.Stride, offset, divisor);
            offset += attrib.FieldSizes[i] * attrib.TypeSize;
        }
    }

    public override void Bind() =>
        GL.BindVertexArray(_vertexArrayObject);

    public static void StaticUnbind() =>
        GL.BindVertexArray(VertexArrayHandle.Zero);

    public override void Unbind() =>
        StaticUnbind();

    public override void Dispose() =>
        GL.DeleteVertexArray(_vertexArrayObject);



    public record VertexAttribute(VertexAttribPointerType PointerType, int Stride, int TypeSize, int[] FieldSizes);

    private readonly static Dictionary<Type, VertexAttribute> _attributeByType = new();

    /// <summary> Determines that vertex attributes of the given type. </summary>
    public static VertexAttribute GetVertexAttribute<V>() where V : unmanaged {

        Type vertexType = typeof(V);

        //if answer exists: use that
        if(_attributeByType.TryGetValue(vertexType, out VertexAttribute? vertexAttribute)) {
            return vertexAttribute;
        }

        Type innerType = GetInnerVertexType(vertexType);

        VertexAttribute attrib = new(
            GetVertexAttribPointerType(innerType),
            Marshal.SizeOf<V>(),
            Marshal.SizeOf(innerType),
            GetVertexFieldsSizes(vertexType)
        );

        //save answer
        _attributeByType.Add(vertexType, attrib);

        return attrib;
    }

    /// <summary> Determines the simple type of the vertex. </summary>
    private static Type GetInnerVertexType(Type vertexType) {

        //base step: length zero
        if(IsVertexType(vertexType)) {
            return vertexType;
        }

        //recursive step
        FieldInfo[] fields = vertexType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        bool equalInnerType = true;
        Type innerType = GetInnerVertexType(fields.First().FieldType);
        for(int i = 1; i < fields.Length; i++) {
            Type childInnerType = GetInnerVertexType(fields[i].FieldType);

            if(childInnerType != innerType) {
                equalInnerType = false;
                break;
            }
        }

        if(equalInnerType) {
            return innerType;
        }

        throw new ArgumentException($"The inner vertex type could not be determined for the given type {vertexType.Name}.");
    }

    /// <summary> Determines the sizes of the fields. </summary>
    private static int[] GetVertexFieldsSizes(Type vertexType) {

        //base step: determine if the given type has no fields
        if(IsVertexType(vertexType)) {
            return new[] { 1 };
        }

        //base step: determine if all children are vertex type
        FieldInfo[] fields = vertexType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        bool isVertexField = true;
        for(int i = 0; i < fields.Length; i++) {
            if(!IsVertexType(fields[i].FieldType)) {
                isVertexField = false;
                break;
            }
        }

        if(isVertexField && fields.Length > 0) {
            return new[] { fields.Length };
        }

        //recursive step
        List<int> fieldSizes = new();
        for(int i = 0; i < fields.Length; i++) {
            fieldSizes.AddRange(GetVertexFieldsSizes(fields[i].FieldType));
        }

        if(fieldSizes.Count > 0) {
            return fieldSizes.ToArray();
        }

        throw new ArgumentException("The given type could not be formated as a vertex.");
    }

    /// <summary> Converts from a type to a VertexAttribPointerType. </summary>
    private static VertexAttribPointerType GetVertexAttribPointerType(Type type) {
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

    /// <summary> Determines if the given type can be converted to a VertexAttribPointerType. </summary>
    private static bool IsVertexType(Type type) {
        switch(type.Name) {
            case nameof(Byte): return true;
            case nameof(SByte): return true;

            case nameof(UInt16): return true;
            case nameof(Int16): return true;

            case nameof(UInt32): return true;
            case nameof(Int32): return true;

            case nameof(Single): return true;

            case nameof(Double): return true;
        };

        return false;
    }


}

