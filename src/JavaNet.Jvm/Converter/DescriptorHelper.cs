using System.Collections.Generic;
using JavaNet.Jvm.Util;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Helper class for dealing with descriptors.
    /// </summary>
    public static class DescriptorHelper
    {
        /// <summary>
        /// Gets the return of a method descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>The type name of the return.</returns>
        public static string GetReturn(string descriptor)
        {
            Guard.NotNull(ref descriptor, nameof(descriptor));
            return descriptor.Substring(descriptor.IndexOf(')') + 1);
        }

        /// <summary>
        /// Gets the parameter descriptors of a method descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>The parameters of the return.</returns>
        public static string[] GetParameters(string descriptor)
        {
            Guard.NotNull(ref descriptor, nameof(descriptor));
            string parameters = descriptor.Substring(descriptor.IndexOf('(') + 1, descriptor.IndexOf(')') - descriptor.IndexOf('(') - 1);
            List<string> result = new List<string>();
            int i = 0;
            string memory = string.Empty;
            while (i < parameters.Length)
            {
                if (parameters[i] == 'B' ||
                    parameters[i] == 'C' ||
                    parameters[i] == 'D' ||
                    parameters[i] == 'F' ||
                    parameters[i] == 'I' ||
                    parameters[i] == 'J' ||
                    parameters[i] == 'S' ||
                    parameters[i] == 'Z')
                {
                    memory += parameters[i++];
                    result.Add(memory);
                    memory = string.Empty;
                }
                else if (parameters[i] == '[')
                {
                    memory += parameters[i++];
                }
                else if (parameters[i] == 'L')
                {
                    int length = parameters.Substring(i).IndexOf(';') + 1;
                    memory += parameters.Substring(i, length);
                    result.Add(memory);
                    memory = string.Empty;
                    i += length;
                }
            }

            return result.ToArray();
        }
    }
}
