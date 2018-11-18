using System.Diagnostics.CodeAnalysis;

namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Attribute representing possible exceptions.
    /// </summary>
    /// <seealso cref="JavaAttribute" />
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
    public class JavaAttributeExceptions : JavaAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaAttributeExceptions"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        /// <param name="numberOfExceptions">The number of exceptions.</param>
        /// <param name="exceptionIndices">The exception indices.</param>
        public JavaAttributeExceptions(ushort nameIndex, uint length, ushort numberOfExceptions, ushort[] exceptionIndices)
            : base(nameIndex, length)
        {
            NumberOfExceptions = numberOfExceptions;
            ExceptionIndices = exceptionIndices;
        }

        /// <summary>
        /// Gets the number of exceptions.
        /// </summary>
        /// <value>
        /// The number of exceptions.
        /// </value>
        public ushort NumberOfExceptions { get; }

        /// <summary>
        /// Gets the exception indices.
        /// </summary>
        /// <value>
        /// The exception indices.
        /// </value>
        public ushort[] ExceptionIndices { get; }
    }
}
