using System.Diagnostics.CodeAnalysis;

namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Represents entries in the bootstrap methods field of <see cref="JavaAttributeBootstrapMethods"/>.
    /// </summary>
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
    public class JavaBootstrapMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaBootstrapMethod"/> class.
        /// </summary>
        /// <param name="methodReference">The method reference.</param>
        /// <param name="argumentCount">The argument count.</param>
        /// <param name="arguments">The arguments.</param>
        public JavaBootstrapMethod(ushort methodReference, ushort argumentCount, ushort[] arguments)
        {
            MethodReference = methodReference;
            ArgumentCount = argumentCount;
            Arguments = arguments;
        }

        /// <summary>
        /// Gets the method reference.
        /// </summary>
        /// <value>
        /// The method reference.
        /// </value>
        public ushort MethodReference { get; }

        /// <summary>
        /// Gets the argument count.
        /// </summary>
        /// <value>
        /// The argument count.
        /// </value>
        public ushort ArgumentCount { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public ushort[] Arguments { get; }
    }
}
