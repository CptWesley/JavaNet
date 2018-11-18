namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a method reference.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantMethodReference : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantMethodReference"/> class.
        /// </summary>
        /// <param name="classIndex">Index of the class in the constant pool.</param>
        /// <param name="nameAndTypeIndex">Index of the name and type in the constant pool.</param>
        public JavaConstantMethodReference(ushort classIndex, ushort nameAndTypeIndex)
        {
            ClassIndex = classIndex;
            NameAndTypeIndex = nameAndTypeIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.MethodReference;

        /// <summary>
        /// Gets the index of the class.
        /// </summary>
        /// <value>
        /// The index of the class.
        /// </value>
        public ushort ClassIndex { get; }

        /// <summary>
        /// Gets the index of the name and type.
        /// </summary>
        /// <value>
        /// The index of the name and type.
        /// </value>
        public ushort NameAndTypeIndex { get; }
    }
}
