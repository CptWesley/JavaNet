using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Represent bootstrap methods attribute.
    /// </summary>
    /// <seealso cref="JavaAttribute" />
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
    public class JavaAttributeBootstrapMethods : JavaAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaAttributeBootstrapMethods"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        /// <param name="numberOfBootstrapMethods">The number of bootstrap methods.</param>
        /// <param name="bootstrapMethods">The bootstrap methods.</param>
        public JavaAttributeBootstrapMethods(
            ushort nameIndex,
            uint length,
            ushort numberOfBootstrapMethods,
            JavaBootstrapMethod[] bootstrapMethods)
            : base(nameIndex, length)
        {
            NumberOfBootstrapMethods = numberOfBootstrapMethods;
            BootstrapMethods = bootstrapMethods;
        }

        /// <summary>
        /// Gets the number of bootstrap methods.
        /// </summary>
        /// <value>
        /// The number of bootstrap methods.
        /// </value>
        public ushort NumberOfBootstrapMethods { get; }

        /// <summary>
        /// Gets the bootstrap methods.
        /// </summary>
        /// <value>
        /// The bootstrap methods.
        /// </value>
        public JavaBootstrapMethod[] BootstrapMethods { get; }

        /// <summary>
        /// Reads this attributes from the stream.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>The attribute from the stream.</returns>
        public static JavaAttributeBootstrapMethods ReadFromStream(ushort nameIndex, uint length, Stream stream)
        {
            ushort count = stream.ReadShort();
            JavaBootstrapMethod[] methods = new JavaBootstrapMethod[count];
            for (int i = 0; i < count; i++)
            {
                ushort reference = stream.ReadShort();
                ushort argumentCount = stream.ReadShort();
                methods[i] = new JavaBootstrapMethod(reference, argumentCount, stream.ReadShorts(argumentCount));
            }

            return new JavaAttributeBootstrapMethods(nameIndex, length, count, methods);
        }
    }
}
