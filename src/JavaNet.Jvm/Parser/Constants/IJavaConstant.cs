namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Interface for java constants.
    /// </summary>
    public interface IJavaConstant
    {
        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        ConstantPoolTag Tag { get; }
    }
}
