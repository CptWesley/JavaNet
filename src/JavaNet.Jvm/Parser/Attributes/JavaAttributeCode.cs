using System.Diagnostics.CodeAnalysis;

namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Attribute representing a constant value.
    /// </summary>
    /// <seealso cref="JavaAttribute" />
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
    public class JavaAttributeCode : JavaAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaAttributeCode"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        /// <param name="maxStack">The maximum stack.</param>
        /// <param name="maxLocals">The maximum locals.</param>
        /// <param name="codeLength">Length of the code.</param>
        /// <param name="code">The code.</param>
        /// <param name="exceptionTableLength">Length of the exception table.</param>
        /// <param name="exceptionTable">The exception table.</param>
        /// <param name="attributesCount">The attributes count.</param>
        /// <param name="attributes">The attributes.</param>
        public JavaAttributeCode(
            ushort nameIndex,
            uint length,
            ushort maxStack,
            ushort maxLocals,
            uint codeLength,
            byte[] code,
            ushort exceptionTableLength,
            JavaExceptionTableEntry[] exceptionTable,
            ushort attributesCount,
            IJavaAttribute[] attributes)
            : base(nameIndex, length)
        {
            MaxStack = maxStack;
            MaxLocals = maxLocals;
            CodeLength = codeLength;
            Code = code;
            ExceptionTableLength = exceptionTableLength;
            ExceptionTable = exceptionTable;
            AttributesCount = attributesCount;
            Attributes = attributes;
        }

        /// <summary>
        /// Gets the maximum stack size.
        /// </summary>
        /// <value>
        /// The maximum stack size.
        /// </value>
        public ushort MaxStack { get; }

        /// <summary>
        /// Gets the number of local variables.
        /// </summary>
        /// <value>
        /// The number of local variables.
        /// </value>
        public ushort MaxLocals { get; }

        /// <summary>
        /// Gets the length of the code.
        /// </summary>
        /// <value>
        /// The length of the code.
        /// </value>
        public uint CodeLength { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public byte[] Code { get; }

        /// <summary>
        /// Gets the length of the exception table.
        /// </summary>
        /// <value>
        /// The length of the exception table.
        /// </value>
        public ushort ExceptionTableLength { get; }

        /// <summary>
        /// Gets the exception table.
        /// </summary>
        /// <value>
        /// The exception table.
        /// </value>
        public JavaExceptionTableEntry[] ExceptionTable { get; }

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
        public IJavaAttribute[] Attributes { get; }
    }
}
