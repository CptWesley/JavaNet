using System.Collections.Generic;
using System.Linq;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Fields;
using JavaNet.Jvm.Util;
using Mono.Cecil;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Extension methods for the <see cref="JavaField"/> class.
    /// </summary>
    public static class JavaFieldExtensions
    {
        /// <summary>
        /// Gets the field attributes.
        /// </summary>
        /// <param name="jf">The java field.</param>
        /// <returns>The attributes of the field.</returns>
        public static FieldAttributes GetAttributes(this JavaField jf)
        {
            Guard.NotNull(ref jf, nameof(jf));
            JavaFieldAccessFlags accessFlags = jf.AccessFlags;

            FieldAttributes result = 0;

            if (accessFlags.HasFlag(JavaFieldAccessFlags.Static))
            {
                result |= FieldAttributes.Static;
            }

            if (accessFlags.HasFlag(JavaFieldAccessFlags.Public))
            {
                result |= FieldAttributes.Public;
            }

            if (accessFlags.HasFlag(JavaFieldAccessFlags.Private))
            {
                result |= FieldAttributes.Private;
            }

            if (accessFlags.HasFlag(JavaFieldAccessFlags.Protected))
            {
                result |= FieldAttributes.Family;
            }

            return result;
        }

        /// <summary>
        /// Gets the name of a method.
        /// </summary>
        /// <param name="jf">The java field.</param>
        /// <param name="jc">The java class.</param>
        /// <returns>The name of the java method.</returns>
        public static string GetName(this JavaField jf, JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref jf, nameof(jf));

            return jc.GetConstant<JavaConstantUtf8>(jf.NameIndex).Value;
        }

        /// <summary>
        /// Gets the descriptor of a method.
        /// </summary>
        /// <param name="jf">The java field.</param>
        /// <param name="jc">The java class.</param>
        /// <returns>The descriptor of the java method.</returns>
        public static string GetDescriptor(this JavaField jf, JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref jf, nameof(jf));

            return jc.GetConstant<JavaConstantUtf8>(jf.DescriptorIndex).Value;
        }

        /// <summary>
        /// Gets the attributes of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the attributes.</typeparam>
        /// <param name="jf">The java field.</param>
        /// <returns>The attributes of the given type.</returns>
        public static T[] GetAttributes<T>(this JavaField jf)
            where T : IJavaAttribute
        {
            Guard.NotNull(ref jf, nameof(jf));

            List<T> result = new List<T>();

            foreach (IJavaAttribute attribute in jf.Attributes)
            {
                if (attribute is T matched)
                {
                    result.Add(matched);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets the attribute of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="jf">The java field.</param>
        /// <returns>The attribute of the given type.</returns>
        public static T GetAttribute<T>(this JavaField jf)
            where T : IJavaAttribute
        {
            Guard.NotNull(ref jf, nameof(jf));
            return jf.GetAttributes<T>().FirstOrDefault();
        }
    }
}
