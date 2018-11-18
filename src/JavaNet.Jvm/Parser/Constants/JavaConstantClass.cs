namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a class constant.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantClass : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantClass"/> class.
        /// </summary>
        /// <param name="nameIndex">Constant pool index of the name.</param>
        public JavaConstantClass(ushort nameIndex)
        {
            NameIndex = nameIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.Class;

        /// <summary>
        /// Gets the constant pool index of the name.
        /// </summary>
        /// <value>
        /// The constant pool index of the name.
        /// </value>
        public ushort NameIndex { get; }
    }
}
