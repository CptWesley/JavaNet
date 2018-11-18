namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a double constant.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantDouble : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantDouble"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public JavaConstantDouble(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.Double;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; }
    }
}
