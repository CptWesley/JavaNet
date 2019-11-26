using System;

namespace JavaNet.Jvm.Parser
{
    /// <summary>
    /// Enum flags representing access flags of classes.
    /// </summary>
    [Flags]
    public enum JavaClassAccessFlags
    {
        /// <summary>
        /// The public keyword.
        /// </summary>
        Public = 0x0001,

        /// <summary>
        /// The final keyword.
        /// </summary>
        Final = 0x0010,

        /// <summary>
        /// Indicates there is an (implicit) super class.
        /// </summary>
        Super = 0x0020,

        /// <summary>
        /// Indicates the class is an interface.
        /// </summary>
        Interface = 0x0200,

        /// <summary>
        /// The abstract keyword.
        /// </summary>
        Abstract = 0x0400,

        /// <summary>
        /// Synthetic.
        /// </summary>
        Synthetic = 0x1000,

        /// <summary>
        /// Indicates the class is an annotation.
        /// </summary>
        Annotation = 0x2000,

        /// <summary>
        /// Indicates the class is an enum.
        /// </summary>
        Enum = 0x4000,

        /// <summary>
        /// Indicates the class is a module.
        /// </summary>
        Module = 0x8000,
    }
}
