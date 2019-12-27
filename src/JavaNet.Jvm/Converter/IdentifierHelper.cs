using System.Globalization;
using JavaNet.Jvm.Util;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Helper classes for converting identifiers.
    /// </summary>
    public static class IdentifierHelper
    {
        /// <summary>
        /// Gets the .NET convention namespace name of a java type name.
        /// </summary>
        /// <param name="typeName">Java type name.</param>
        /// <returns>The .NET namespace name.</returns>
        public static string GetDotNetNamespace(string typeName)
        {
            Guard.NotNull(ref typeName, nameof(typeName));
            typeName = GetDotNetFullName(typeName);
            int last = typeName.LastIndexOf('.');
            return last > 0 ? typeName.Substring(0, last) : typeName;
        }

        /// <summary>
        /// Gets the .NET convention type name of a java type name.
        /// </summary>
        /// <param name="typeName">Java type name.</param>
        /// <returns>The .NET type name.</returns>
        public static string GetDotNetClassName(string typeName)
        {
            Guard.NotNull(ref typeName, nameof(typeName));
            typeName = GetDotNetFullName(typeName);
            int last = typeName.LastIndexOf('.');
            return typeName.Substring(last + 1);
        }

        /// <summary>
        /// Gets the .NET convention full type name of a java type name.
        /// </summary>
        /// <param name="typeName">Java type name.</param>
        /// <returns>The .NET full type name.</returns>
        public static string GetDotNetFullName(string typeName)
        {
            Guard.NotNull(ref typeName, nameof(typeName));
            typeName = char.ToUpper(typeName[0], CultureInfo.InvariantCulture) + (typeName.Length > 1 ? typeName.Substring(1) : string.Empty);

            for (int i = 0; i < typeName.Length; i++)
            {
                if (typeName[i] == '/')
                {
                    typeName = typeName.Substring(0, i) + '.' + char.ToUpper(typeName[++i], CultureInfo.InvariantCulture) + typeName.Substring(i + 1, typeName.Length - i - 1);
                }
            }

            if (typeName == "<init>")
            {
                typeName = ".ctor";
            }
            else if (typeName == "<clinit>")
            {
                typeName = ".cctor";
            }

            return typeName;
        }
    }
}
