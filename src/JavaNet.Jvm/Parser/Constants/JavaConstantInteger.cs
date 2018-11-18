namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents integer constants.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantInteger : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantInteger"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public JavaConstantInteger(uint value)
        {
            Value = (int)value;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.Integer;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value { get; }
    }
}
