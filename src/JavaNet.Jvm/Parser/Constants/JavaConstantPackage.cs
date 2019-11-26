namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a package constant.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantPackage : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantPackage"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        public JavaConstantPackage(ushort nameIndex)
        {
            NameIndex = nameIndex;
        }

        /// <inheritdoc/>
        public ConstantPoolTag Tag => ConstantPoolTag.Package;

        /// <summary>
        /// Gets the index of the name.
        /// </summary>
        /// <value>
        /// The index of the name.
        /// </value>
        public ushort NameIndex { get; }
    }
}
