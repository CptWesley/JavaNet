namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents an dynmamically computed constant.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantDynamic : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantDynamic"/> class.
        /// </summary>
        /// <param name="bootstrapMethodAttributeIndex">Index of the bootstrap method attribute in the constant pool.</param>
        /// <param name="nameAndTypeIndex">Index of the name and type in the constant pool.</param>
        public JavaConstantDynamic(ushort bootstrapMethodAttributeIndex, ushort nameAndTypeIndex)
        {
            BootstrapMethodAttributeIndex = bootstrapMethodAttributeIndex;
            NameAndTypeIndex = nameAndTypeIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.Dynamic;

        /// <summary>
        /// Gets the index of the bootstrap method attribute in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the bootstrap method attribute in the constant pool.
        /// </value>
        public ushort BootstrapMethodAttributeIndex { get; }

        /// <summary>
        /// Gets the index of the name and type in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the name and type in the constant pool.
        /// </value>
        public ushort NameAndTypeIndex { get; }
    }
}
