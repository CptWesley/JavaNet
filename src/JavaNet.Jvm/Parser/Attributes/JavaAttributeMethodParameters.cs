using System.Diagnostics.CodeAnalysis;
using System.IO;
using JavaNet.Jvm.Util;

namespace JavaNet.Jvm.Parser.Attributes
{
    /// <summary>
    /// Class representing method parameters.
    /// </summary>
    /// <seealso cref="JavaAttribute" />
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Easier to work with.")]
    public class JavaAttributeMethodParameters : JavaAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaAttributeMethodParameters"/> class.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        /// <param name="parameterCount">The parameter count.</param>
        /// <param name="parameters">The parameters.</param>
        public JavaAttributeMethodParameters(ushort nameIndex, uint length, byte parameterCount, JavaParameter[] parameters)
            : base(nameIndex, length)
        {
            ParameterCount = parameterCount;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the parameter count.
        /// </summary>
        /// <value>
        /// The parameter count.
        /// </value>
        public byte ParameterCount { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public JavaParameter[] Parameters { get; }

        /// <summary>
        /// Reads this attribute from the stream.
        /// </summary>
        /// <param name="nameIndex">Index of the name.</param>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>The attribute from the stream.</returns>
        public static JavaAttributeMethodParameters ReadFromStream(ushort nameIndex, uint length, Stream stream)
        {
            Guard.NotNull(ref stream, nameof(stream));

            byte count = (byte)stream.ReadByte();
            JavaParameter[] parameters = new JavaParameter[count];
            for (int i = 0; i < count; i++)
            {
                parameters[i] = new JavaParameter(stream.ReadShort(), stream.ReadShort());
            }

            return new JavaAttributeMethodParameters(nameIndex, length, count, parameters);
        }
    }
}
