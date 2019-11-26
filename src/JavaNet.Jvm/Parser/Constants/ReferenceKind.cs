namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Enum representing the reference kind.
    /// </summary>
    public enum ReferenceKind
    {
        /// <summary>
        /// Get the value of a field of an instance.
        /// </summary>
        GetField = 1,

        /// <summary>
        /// Get the value of a static field.
        /// </summary>
        GetStatic = 2,

        /// <summary>
        /// Set the value of a field of an instance.
        /// </summary>
        PutField = 3,

        /// <summary>
        /// Set the value of a static field.
        /// </summary>
        PutStatic = 4,

        /// <summary>
        /// Invoke a method on an instance.
        /// </summary>
        InvokeVirtual = 5,

        /// <summary>
        /// Invoke a static method.
        /// </summary>
        InvokeStatic = 6,

        /// <summary>
        /// Special invoke a method on an instance.
        /// </summary>
        InvokeSpecial = 7,

        /// <summary>
        /// The new invoke special.
        /// </summary>
        NewInvokeSpecial = 8,

        /// <summary>
        /// Invoke a method on an interface.
        /// </summary>
        InvokeInterface = 9,
    }
}
