using System.Diagnostics.CodeAnalysis;

namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Represents a java attribute not recognized by the jvm.
    /// </summary>
    /// <seealso cref="IJavaAttribute" />
    public class JavaAttributeUnknown : JavaAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaAttributeUnknown"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name in the constants pool.</param>
        /// <param name="length">The number of bytes of this attribute.</param>
        /// <param name="bytes">The info bytes of the attribute.</param>
        public JavaAttributeUnknown(ushort nameIndex, uint length, byte[] bytes)
            : base(nameIndex, length)
        {
            Bytes = bytes;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
        public byte[] Bytes { get; }
    }
}
