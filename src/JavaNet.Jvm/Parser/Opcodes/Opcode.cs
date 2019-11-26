using System.IO;

namespace JavaNet.Jvm.Parser.Opcodes
{
    /// <summary>
    /// Abstract class for opcodes.
    /// </summary>
    public abstract class Opcode
    {
        /// <summary>
        /// Reads an opcode from the stream.
        /// </summary>
        /// <param name="stream">The stream to read the opcode from.</param>
        /// <returns>The opcode read from the stream.</returns>
        public static Opcode ReadFromStream(Stream stream)
        {
            return null;
        }

        /// <summary>
        /// Reads opcodes from the stream for a number.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static Opcode[] ReadFromStream(Stream stream, uint bytes)
        {
            return null;
        }

        /// <summary>
        /// Converts the opcode to a C# function call.
        /// </summary>
        /// <returns>A C# string representing the opcode call.</returns>
        public abstract string ToDotNet();
    }
}
