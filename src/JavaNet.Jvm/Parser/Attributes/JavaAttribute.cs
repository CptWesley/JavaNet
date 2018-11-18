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
    }
}
