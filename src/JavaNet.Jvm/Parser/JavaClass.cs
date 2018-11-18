using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Fields;
using JavaNet.Jvm.Parser.Methods;

namespace JavaNet.Jvm.Parser
{
    /// <summary>
    /// Class representing a parsed java .class file.
    /// </summary>
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
    public class JavaClass
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="JavaClass"/> class from being created.
        /// </summary>
        private JavaClass()
        {
        }

        /// <summary>
        /// Gets the magic number at the start of the file. This should always be 0xCAFEBABE
        /// </summary>
        /// <value>
        /// The magic number at the start of the file.
        /// </value>
        public uint Magic { get; private set; }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        /// <value>
        /// The minor version.
        /// </value>
        public ushort MinorVersion { get; private set; }

        /// <summary>
        /// Gets the major version.
        /// </summary>
        /// <value>
        /// The major version.
        /// </value>
        public ushort MajorVersion { get; private set; }

        /// <summary>
        /// Gets the constant pool count.
        /// </summary>
        /// <value>
        /// The constant pool count.
        /// </value>
        public ushort ConstantPoolCount { get; private set; }

        /// <summary>
        /// Gets the constant pool.
        /// </summary>
        /// <value>
        /// The constant pool.
        /// </value>
        public JavaConstantPool ConstantPool { get; private set; }

        /// <summary>
        /// Gets the access flags.
        /// </summary>
        /// <value>
        /// The access flags.
        /// </value>
        public JavaClassAccessFlags AccessFlags { get; private set; }

        /// <summary>
        /// Gets the index of this class in the constant pool.
        /// </summary>
        /// <value>
        /// The index of this class in the constant pool.
        /// </value>
        public ushort ThisClassIndex { get; private set; }

        /// <summary>
        /// Gets the index of the super class in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the super class in the constant pool.
        /// </value>
        public ushort SuperClassIndex { get; private set; }

        /// <summary>
        /// Gets the interfaces count.
        /// </summary>
        /// <value>
        /// The interfaces count.
        /// </value>
        public ushort InterfacesCount { get; private set; }

        /// <summary>
        /// Gets the interface indices.
        /// </summary>
        /// <value>
        /// The indices of interfaces in the constant pool.
        /// </value>
        public ushort[] Interfaces { get; private set; }

        /// <summary>
        /// Gets the fields count.
        /// </summary>
        /// <value>
        /// The fields count.
        /// </value>
        public ushort FieldsCount { get; private set; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public JavaField[] Fields { get; private set; }

        /// <summary>
        /// Gets the method count.
        /// </summary>
        /// <value>
        /// The method count.
        /// </value>
        public ushort MethodsCount { get; private set; }

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <value>
        /// The methods.
        /// </value>
        public JavaMethod[] Methods { get; private set; }

        /// <summary>
        /// Gets the attributes count.
        /// </summary>
        /// <value>
        /// The attributes count.
        /// </value>
        public ushort AttributesCount { get; private set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public IJavaAttribute[] Attributes { get; private set; }

        /// <summary>
        /// Creates a <see cref="JavaClass"/> instance from bytes.
        /// </summary>
        /// <param name="bytes">The bytes to create the class file from.</param>
        /// <returns>A new <see cref="JavaClass"/> instance.</returns>
        public static JavaClass Create(byte[] bytes)
        {
            using (Stream stream = new MemoryStream(bytes))
            {
                return Create(stream);
            }
        }

        /// <summary>
        /// Creates a <see cref="JavaClass"/> instance from bytes.
        /// </summary>
        /// <param name="stream">The stream to create the class file from.</param>
        /// <returns>A new <see cref="JavaClass"/> instance.</returns>
        public static JavaClass Create(Stream stream)
        {
            JavaClass result = new JavaClass();
            result.Magic = stream.ReadInteger();
            result.MinorVersion = stream.ReadShort();
            result.MajorVersion = stream.ReadShort();
            result.ConstantPoolCount = stream.ReadShort();
            result.ConstantPool = ReadConstants(stream, result.ConstantPoolCount);
            result.AccessFlags = (JavaClassAccessFlags)stream.ReadShort();
            result.ThisClassIndex = stream.ReadShort();
            result.SuperClassIndex = stream.ReadShort();
            result.InterfacesCount = stream.ReadShort();
            result.Interfaces = stream.ReadShorts(result.InterfacesCount);
            result.FieldsCount = stream.ReadShort();
            result.Fields = ReadFields(stream, result.FieldsCount);
            result.MethodsCount = stream.ReadShort();
            result.Methods = ReadMethods(stream, result.MethodsCount);
            result.AttributesCount = stream.ReadShort();
            result.Attributes = ReadAttributes(stream, result.AttributesCount);

            return result;
        }

        /// <summary>
        /// Reads the constants.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="count">The constant count.</param>
        /// <returns>A new <see cref="JavaConstantPool"/> containing all constants.</returns>
        private static JavaConstantPool ReadConstants(Stream stream, int count)
        {
            // Count is -1 for some reason.
            IJavaConstant[] constants = new IJavaConstant[count - 1];

            for (int i = 0; i < count - 1; i++)
            {
                constants[i] = ReadConstant(stream);
            }

            return new JavaConstantPool(constants);
        }

        /// <summary>
        /// Reads a single constant from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A parsed java constant.</returns>
        /// <exception cref="JavaParserException">Thrown when something goes wrong while parsing.</exception>
        private static IJavaConstant ReadConstant(Stream stream)
        {
            int tag = stream.ReadByte();
            switch (tag)
            {
                case 1:
                    return new JavaConstantUtf8(Encoding.UTF8.GetString(stream.ReadBytes(stream.ReadShort())));
                case 3:
                    return new JavaConstantInteger(stream.ReadInteger());
                case 4:
                    return new JavaConstantFloat(stream.ReadFloat());
                case 5:
                    return new JavaConstantLong(stream.ReadLong());
                case 6:
                    return new JavaConstantDouble(stream.ReadDouble());
                case 7:
                    return new JavaConstantClass(stream.ReadShort());
                case 8:
                    return new JavaConstantString(stream.ReadShort());
                case 9:
                    return new JavaConstantFieldReference(stream.ReadShort(), stream.ReadShort());
                case 10:
                    return new JavaConstantMethodReference(stream.ReadShort(), stream.ReadShort());
                case 11:
                    return new JavaConstantInterfaceMethodReference(stream.ReadShort(), stream.ReadShort());
                case 12:
                    return new JavaConstantNameAndType(stream.ReadShort(), stream.ReadShort());
                case 15:
                    return new JavaConstantMethodHandle((ReferenceKind)stream.ReadByte(), stream.ReadShort());
                case 16:
                    return new JavaConstantMethodType(stream.ReadShort());
                case 18:
                    return new JavaConstantInvokeDynamic(stream.ReadShort(), stream.ReadShort());
                default:
                    throw new JavaParserException($"Illegal constant pool tag '{tag}' was found.");
            }
        }

        /// <summary>
        /// Reads attributes from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="count">The number of attributes to read.</param>
        /// <returns>The attributes from the stream.</returns>
        private static IJavaAttribute[] ReadAttributes(Stream stream, int count)
        {
            IJavaAttribute[] result = new IJavaAttribute[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = ReadAttribute(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads an attribute from the given stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The attribute read from the stream.</returns>
        private static IJavaAttribute ReadAttribute(Stream stream)
        {
            ushort nameIndex = stream.ReadShort();
            uint length = stream.ReadInteger();
            for (int i = 0; i < length; i++)
            {
                stream.ReadByte();
            }

            return null;
        }

        /// <summary>
        /// Reads fields from the stream.
        /// </summary>
        /// <param name="stream">The stream to read the fields from.</param>
        /// <param name="count">The number of fields to read.</param>
        /// <returns>The fields read from the stream</returns>
        private static JavaField[] ReadFields(Stream stream, int count)
        {
            JavaField[] result = new JavaField[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = ReadField(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads a field from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The field read from the stream.</returns>
        private static JavaField ReadField(Stream stream)
        {
            JavaFieldAccessFlags flags = (JavaFieldAccessFlags)stream.ReadShort();
            ushort nameIndex = stream.ReadShort();
            ushort descriptorIndex = stream.ReadShort();
            ushort attributesCount = stream.ReadShort();
            IJavaAttribute[] attributes = ReadAttributes(stream, attributesCount);
            return new JavaField(flags, nameIndex, descriptorIndex, attributesCount, attributes);
        }

        /// <summary>
        /// Reads methods from the stream.
        /// </summary>
        /// <param name="stream">The stream to read the methods from.</param>
        /// <param name="count">The number of methods to read.</param>
        /// <returns>The methods read from the stream</returns>
        private static JavaMethod[] ReadMethods(Stream stream, int count)
        {
            JavaMethod[] result = new JavaMethod[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = ReadMethod(stream);
            }

            return result;
        }

        /// <summary>
        /// Reads a method from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The method read from the stream.</returns>
        private static JavaMethod ReadMethod(Stream stream)
        {
            JavaMethodAccessFlags flags = (JavaMethodAccessFlags)stream.ReadShort();
            ushort nameIndex = stream.ReadShort();
            ushort descriptorIndex = stream.ReadShort();
            ushort attributesCount = stream.ReadShort();
            IJavaAttribute[] attributes = ReadAttributes(stream, attributesCount);
            return new JavaMethod(flags, nameIndex, descriptorIndex, attributesCount, attributes);
        }
    }
}
