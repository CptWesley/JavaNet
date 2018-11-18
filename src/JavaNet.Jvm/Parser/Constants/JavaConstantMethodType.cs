namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a method type.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantMethodType : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantMethodType"/> class.
        /// </summary>
        /// <param name="descriptorIndex">Index of the descriptor in the constant pool.</param>
        public JavaConstantMethodType(ushort descriptorIndex)
        {
            DescriptorIndex = descriptorIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.MethodType;

        /// <summary>
        /// Gets the index of the descriptor in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the descriptor in the constant pool.
        /// </value>
        public ushort DescriptorIndex { get; }
    }
}
