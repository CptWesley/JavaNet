using System.Diagnostics.CodeAnalysis;

namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Enum defining constant tags.
    /// </summary>
    [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Consistency with jvm specifications.", Scope = "type")]
    public enum ConstantPoolTag
    {
        /// <summary>
        /// The UTF8 constant tag.
        /// </summary>
        Utf8 = 1,

        /// <summary>
        /// The integer constant tag.
        /// </summary>
        Integer = 3,

        /// <summary>
        /// The float constant tag.
        /// </summary>
        Float = 4,

        /// <summary>
        /// The long constant tag.
        /// </summary>
        Long = 5,

        /// <summary>
        /// The double constant tag.
        /// </summary>
        Double = 6,

        /// <summary>
        /// The class constant tag.
        /// </summary>
        Class = 7,

        /// <summary>
        /// The string constant tag.
        /// </summary>
        String = 8,

        /// <summary>
        /// The field reference constant tag.
        /// </summary>
        FieldReference = 9,

        /// <summary>
        /// The method reference constant tag.
        /// </summary>
        MethodReference = 10,

        /// <summary>
        /// The interface method reference constant tag.
        /// </summary>
        InterfaceMethodReference = 11,

        /// <summary>
        /// The name and type constant tag.
        /// </summary>
        NameAndType = 12,

        /// <summary>
        /// The method handle constant tag.
        /// </summary>
        MethodHandle = 15,

        /// <summary>
        /// The method type constant tag.
        /// </summary>
        MethodType = 16,

        /// <summary>
        /// The dynamic constant tag.
        /// </summary>
        Dynamic = 17,

        /// <summary>
        /// The invoke dynamic constant tag.
        /// </summary>
        InvokeDynamic = 18,

        /// <summary>
        /// The module constant tag.
        /// </summary>
        Module = 19,

        /// <summary>
        /// The package constant tag.
        /// </summary>
        Package = 20,
    }
}
