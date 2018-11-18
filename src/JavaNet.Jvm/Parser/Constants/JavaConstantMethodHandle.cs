namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents method handles.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantMethodHandle : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantMethodHandle"/> class.
        /// </summary>
        /// <param name="referenceKind">Kind of the reference.</param>
        /// <param name="referenceIndex">Index of the reference in the constant pool.</param>
        public JavaConstantMethodHandle(ReferenceKind referenceKind, ushort referenceIndex)
        {
            ReferenceKind = referenceKind;
            ReferenceIndex = referenceIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.MethodHandle;

        /// <summary>
        /// Gets the kind of the reference.
        /// </summary>
        /// <value>
        /// The kind of the reference.
        /// </value>
        public ReferenceKind ReferenceKind { get; }

        /// <summary>
        /// Gets the index of the reference in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the reference in the constant pool.
        /// </value>
        public ushort ReferenceIndex { get; }
    }
}
