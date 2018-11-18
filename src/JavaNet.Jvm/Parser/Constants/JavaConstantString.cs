namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents string constants.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantString : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantString"/> class.
        /// </summary>
        /// <param name="stringIndex">Index of the string in the constant pool.</param>
        public JavaConstantString(ushort stringIndex)
        {
            StringIndex = stringIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.String;

        /// <summary>
        /// Gets the index of the string in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the string in the constant pool.
        /// </value>
        public ushort StringIndex { get; }
    }
}
