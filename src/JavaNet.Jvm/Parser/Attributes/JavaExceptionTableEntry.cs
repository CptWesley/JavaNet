namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Entries of the exception table of the <see cref="JavaAttributeCode"/> attribute.
    /// </summary>
    public class JavaExceptionTableEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaExceptionTableEntry"/> class.
        /// </summary>
        /// <param name="startPc">The start pc.</param>
        /// <param name="endPc">The end pc.</param>
        /// <param name="handlerPc">The handler pc.</param>
        /// <param name="catchType">Type of the catch.</param>
        public JavaExceptionTableEntry(ushort startPc, ushort endPc, ushort handlerPc, ushort catchType)
        {
            StartPc = startPc;
            EndPc = endPc;
            HandlerPc = handlerPc;
            CatchType = catchType;
        }

        /// <summary>
        /// Gets the start pc.
        /// </summary>
        /// <value>
        /// The start pc.
        /// </value>
        public ushort StartPc { get; }

        /// <summary>
        /// Gets the end pc.
        /// </summary>
        /// <value>
        /// The end pc.
        /// </value>
        public ushort EndPc { get; }

        /// <summary>
        /// Gets the handler pc.
        /// </summary>
        /// <value>
        /// The handler pc.
        /// </value>
        public ushort HandlerPc { get; }

        /// <summary>
        /// Gets the type of the catch.
        /// </summary>
        /// <value>
        /// The type of the catch.
        /// </value>
        public ushort CatchType { get; }
    }
}
