namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Attribute representing a constant value.
    /// </summary>
    /// <seealso cref="JavaAttribute" />
    public class JavaAttributeConstantValue : JavaAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaAttributeConstantValue"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        /// <param name="constantValueIndex">Index of the constant value.</param>
        public JavaAttributeConstantValue(ushort nameIndex, uint length, ushort constantValueIndex)
            : base(nameIndex, length)
        {
            ConstantValueIndex = constantValueIndex;
        }

        /// <summary>
        /// Gets the index of the constant value in the constants pool.
        /// </summary>
        /// <value>
        /// The index of the constant value in the constants pool.
        /// </value>
        public ushort ConstantValueIndex { get; }
    }
}
