namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a module constant.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantModule : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantModule"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        public JavaConstantModule(ushort nameIndex)
        {
            NameIndex = nameIndex;
        }

        /// <inheritdoc/>
        public ConstantPoolTag Tag => ConstantPoolTag.Module;

        /// <summary>
        /// Gets the index of the name.
        /// </summary>
        /// <value>
        /// The index of the name.
        /// </value>
        public ushort NameIndex { get; }
    }
}
