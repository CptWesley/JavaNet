namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents long constants.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantLong : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantLong"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public JavaConstantLong(ulong value)
        {
            Value = (long)value;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.Long;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public long Value { get; }
    }
}
