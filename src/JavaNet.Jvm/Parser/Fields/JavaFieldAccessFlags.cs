using System;

namespace JavaNet.Jvm.Parser.Fields
{
    /// <summary>
    /// Enum flags representing access flags of fields.
    /// </summary>
    [Flags]
    public enum JavaFieldAccessFlags
    {
        /// <summary>
        /// The public keyword.
        /// </summary>
        Public = 0x0001,

        /// <summary>
        /// The private keyword.
        /// </summary>
        Private = 0x0002,

        /// <summary>
        /// The protected keyword.
        /// </summary>
        Protected = 0x0004,

        /// <summary>
        /// The static keyword.
        /// </summary>
        Static = 0x0008,

        /// <summary>
        /// The final keyword.
        /// </summary>
        Final = 0x0010,

        /// <summary>
        /// The volatile keyword.
        /// </summary>
        Volatile = 0x0040,

        /// <summary>
        /// The transient keyword.
        /// </summary>
        Transient = 0x0080,

        /// <summary>
        /// Indicates the class is synthetic.
        /// </summary>
        Synthetic = 0x1000,

        /// <summary>
        /// Indicates the field is an enum.
        /// </summary>
        Enum = 0x4000
    }
}
