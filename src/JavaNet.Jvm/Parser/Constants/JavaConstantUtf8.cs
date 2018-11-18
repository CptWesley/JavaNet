namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents a Utf8 string.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantUtf8 : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantUtf8"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public JavaConstantUtf8(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.Utf8;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; }
    }
}
