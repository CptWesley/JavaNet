namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a name and type combination.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantNameAndType : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantNameAndType"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name in the constant pool.</param>
        /// <param name="descriptorIndex">Index of the descriptor in the constant pool.</param>
        public JavaConstantNameAndType(ushort nameIndex, ushort descriptorIndex)
        {
            NameIndex = nameIndex;
            DescriptorIndex = descriptorIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.NameAndType;

        /// <summary>
        /// Gets the index of the name in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the name in the constant pool.
        /// </value>
        public ushort NameIndex { get; }

        /// <summary>
        /// Gets the index of the descriptor in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the descriptor in the constant pool.
        /// </value>
        public ushort DescriptorIndex { get; }
    }
}
