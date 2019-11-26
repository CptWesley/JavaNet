using System.IO;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Util;

namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Abstract class for java attributes.
    /// </summary>
    /// <seealso cref="IJavaAttribute" />
    public abstract class JavaAttribute : IJavaAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaAttribute"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        public JavaAttribute(ushort nameIndex, uint length)
        {
            NameIndex = nameIndex;
            Length = length;
        }

        /// <summary>
        /// Gets the index of the name in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the name in the constant pool.
        /// </value>
        public ushort NameIndex { get; }

        /// <summary>
        /// Gets the length of the attribute in number of bytes.
        /// </summary>
        /// <value>
        /// The length of the attribute in number of bytes.
        /// </value>
        public uint Length { get; }

        /// <summary>
        /// Reads attributes from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="count">The amount of attributes to read.</param>
        /// <param name="constantPool">The constant pool.</param>
        /// <returns>An array of attributes read from the stream.</returns>
        public static IJavaAttribute[] ReadFromStream(Stream stream, int count, JavaConstantPool constantPool)
        {
            Guard.NotNull(ref stream, nameof(stream));
            Guard.NotNull(ref constantPool, nameof(constantPool));

            IJavaAttribute[] result = new IJavaAttribute[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = ReadAttribute(stream, constantPool);
            }

            return result;
        }

        /// <summary>
        /// Reads an attribute from the given stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="constantPool">The constant pool used for finding the attribute names.</param>
        /// <returns>The attribute read from the stream.</returns>
        private static IJavaAttribute ReadAttribute(Stream stream, JavaConstantPool constantPool)
        {
            ushort nameIndex = stream.ReadShort();
            uint length = stream.ReadInteger();
            string name = ((JavaConstantUtf8)constantPool[nameIndex]).Value;

            // TODO Implement commented cases.
            switch (name)
            {
                case "ConstantValue":
                    return new JavaAttributeConstantValue(nameIndex, length, stream.ReadShort());
                case "Code":
                    return JavaAttributeCode.ReadFromStream(stream, constantPool, nameIndex, length);

                // case "StackMapTable":
                case "Exceptions":
                    ushort exceptionCount = stream.ReadShort();
                    return new JavaAttributeExceptions(nameIndex, length, exceptionCount, stream.ReadShorts(exceptionCount));
                case "BootstrapMethods":
                    return JavaAttributeBootstrapMethods.ReadFromStream(nameIndex, length, stream);

                // case "InnerClasses":
                // case "EnclosingMethods":
                // case "Synthetic":
                // case "Signature":
                // case "RuntimeVisibleAnnotations":
                // case "RuntimeInvisibleAnnotations":
                // case "RuntimeVisibleParameterAnnotations":
                // case "RuntimeInvisibleParameterAnnotations":
                // case "RuntimeVisibleTypeAnnotations":
                // case "RuntimeInvisibleTypeAnnotations":
                // case "AnnotationDefault":
                case "MethodParameters":
                    return JavaAttributeMethodParameters.ReadFromStream(nameIndex, length, stream);

                // case "Module":
                // case "ModulePackages":
                // case "ModuleMainClass":
                // case "NestHost":
                // case "NestMembers":
                default:
                    return new JavaAttributeUnknown(nameIndex, length, stream.ReadBytes(length));
            }
        }
    }
}
