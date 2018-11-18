namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Parameter for <see cref="JavaAttributeMethodParameters"/> class.
    /// </summary>
    public class JavaParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaParameter"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="accessFlags">The access flags.</param>
        public JavaParameter(ushort nameIndex, ushort accessFlags)
        {
            NameIndex = nameIndex;
            AccessFlags = accessFlags;
        }

        /// <summary>
        /// Gets the index of the name.
        /// </summary>
        /// <value>
        /// The index of the name.
        /// </value>
        public ushort NameIndex { get; }

        /// <summary>
        /// Gets the access flags.
        /// </summary>
        /// <value>
        /// The access flags.
        /// </value>
        public ushort AccessFlags { get; }
    }
}
