namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Interface for java attributes.
    /// </summary>
    public interface IJavaAttribute
    {
        /// <summary>
        /// Gets the index of the name in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the name in the constant pool.
        /// </value>
        ushort NameIndex { get; }

        /// <summary>
        /// Gets the length of the attribute in number of bytes.
        /// </summary>
        /// <value>
        /// The length of the attribute in number of bytes.
        /// </value>
        uint Length { get; }
    }
}
