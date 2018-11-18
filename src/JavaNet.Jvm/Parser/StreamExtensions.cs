using System;
using System.IO;

namespace JavaNet.Jvm.Parser
{
    /// <summary>
    /// Internal extension class for adding methods to simplify reading class files.
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// Reads a number of bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="amount">The amount of bytes to read.</param>
        /// <returns>An array of bytes read from the stream.</returns>
        internal static byte[] ReadBytes(this Stream stream, int amount)
        {
            byte[] result = new byte[amount];

            for (int i = 0; i < amount; i++)
            {
                result[i] = (byte)stream.ReadByte();
            }

            return result;
        }

        /// <summary>
        /// Reads a number of bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="amount">The amount of bytes to read.</param>
        /// <returns>An array of bytes read from the stream.</returns>
        internal static byte[] ReadBytes(this Stream stream, uint amount)
        {
            byte[] result = new byte[amount];

            for (int i = 0; i < amount; i++)
            {
                result[i] = (byte)stream.ReadByte();
            }

            return result;
        }

        /// <summary>
        /// Reads an unsigned long from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>An unsigned long.</returns>
        internal static ulong ReadLong(this Stream stream)
        {
            byte[] bytes = stream.ReadBytes(8);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned integer from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>An unsigned integer.</returns>
        internal static uint ReadInteger(this Stream stream)
        {
            byte[] bytes = stream.ReadBytes(4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned short from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>An unsigned short.</returns>
        internal static ushort ReadShort(this Stream stream)
        {
            byte[] bytes = stream.ReadBytes(2);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Reads a number of unsigned shorts from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="amount">The amount of shorts to read.</param>
        /// <returns>An array of unsigned shorts read from the stream.</returns>
        internal static ushort[] ReadShorts(this Stream stream, int amount)
        {
            ushort[] result = new ushort[amount];

            for (int i = 0; i < amount; i++)
            {
                result[i] = stream.ReadShort();
            }

            return result;
        }

        /// <summary>
        /// Reads a float from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A float.</returns>
        internal static float ReadFloat(this Stream stream)
        {
            byte[] bytes = stream.ReadBytes(4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Reads a double from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A double.</returns>
        internal static double ReadDouble(this Stream stream)
        {
            byte[] bytes = stream.ReadBytes(8);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToDouble(bytes, 0);
        }
    }
}
