using System.Diagnostics.CodeAnalysis;
using JavaNet.Jvm.Parser.Attributes;

namespace JavaNet.Jvm.Parser.Fields
{
    /// <summary>
    /// Representing parsed java fields.
    /// </summary>
    public class JavaField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaField"/> class.
        /// </summary>
        /// <param name="accessFlags">The access flags.</param>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="descriptorIndex">Index of the descriptor.</param>
        /// <param name="attributesCount">The attributes count.</param>
        /// <param name="attributes">The attributes.</param>
        public JavaField(JavaFieldAccessFlags accessFlags, ushort nameIndex, ushort descriptorIndex, ushort attributesCount, IJavaAttribute[] attributes)
        {
            AccessFlags = accessFlags;
            NameIndex = nameIndex;
            DescriptorIndex = descriptorIndex;
            AttributesCount = attributesCount;
            Attributes = attributes;
        }

        /// <summary>
        /// Gets the access flags.
        /// </summary>
        /// <value>
        /// The access flags.
        /// </value>
        public JavaFieldAccessFlags AccessFlags { get; }

        /// <summary>
        /// Gets the index of the name in the constants pool.
        /// </summary>
        /// <value>
        /// The index of the name in the constants pool.
        /// </value>
        public ushort NameIndex { get; }

        /// <summary>
        /// Gets the index of the descriptor in the constants pool.
        /// </summary>
        /// <value>
        /// The index of the descriptor in the constants pool.
        /// </value>
        public ushort DescriptorIndex { get; }

        /// <summary>
        /// Gets the attributes count.
        /// </summary>
        /// <value>
        /// The attributes count.
        /// </value>
        public ushort AttributesCount { get; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
        public IJavaAttribute[] Attributes { get; }
    }
}
