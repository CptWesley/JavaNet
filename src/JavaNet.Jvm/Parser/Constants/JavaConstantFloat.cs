namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents float constants.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantFloat : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantFloat"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public JavaConstantFloat(float value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.Float;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public float Value { get; }
    }
}
