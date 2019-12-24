using System;
using System.Collections.Generic;
using System.Linq;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Methods;
using JavaNet.Jvm.Util;
using Mono.Cecil;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Extension methods for the <see cref="JavaMethod"/> class.
    /// </summary>
    public static class JavaMethodExtensions
    {
        /// <summary>
        /// Gets the type attributes.
        /// </summary>
        /// <param name="jm">The java method.</param>
        /// <returns>The type attributes of the method.</returns>
        public static MethodAttributes GetAttributes(this JavaMethod jm)
        {
            Guard.NotNull(ref jm, nameof(jm));
            JavaMethodAccessFlags accessFlags = jm.AccessFlags;

            MethodAttributes result = MethodAttributes.HideBySig;

            if (accessFlags.HasFlag(JavaMethodAccessFlags.Abstract))
            {
                result |= MethodAttributes.Abstract;
            }

            if (accessFlags.HasFlag(JavaMethodAccessFlags.Final))
            {
                result |= MethodAttributes.Final;
            }
            else
            {
                result |= MethodAttributes.Virtual;
            }

            if (accessFlags.HasFlag(JavaMethodAccessFlags.Public))
            {
                result |= MethodAttributes.Public;
            }

            if (accessFlags.HasFlag(JavaMethodAccessFlags.Private))
            {
                result |= MethodAttributes.Private;
            }

            if (accessFlags.HasFlag(JavaMethodAccessFlags.Static))
            {
                result |= MethodAttributes.Static;
            }

            return result;
        }

        /// <summary>
        /// Gets the name of a method.
        /// </summary>
        /// <param name="jm">The java method.</param>
        /// <param name="jc">The java class.</param>
        /// <returns>The name of the java method.</returns>
        public static string GetName(this JavaMethod jm, JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref jm, nameof(jm));

            return jc.GetConstant<JavaConstantUtf8>(jm.NameIndex).Value;
        }

        /// <summary>
        /// Gets the descriptor of a method.
        /// </summary>
        /// <param name="jm">The java method.</param>
        /// <param name="jc">The java class.</param>
        /// <returns>The descriptor of the java method.</returns>
        public static string GetDescriptor(this JavaMethod jm, JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref jm, nameof(jm));

            return jc.GetConstant<JavaConstantUtf8>(jm.DescriptorIndex).Value;
        }

        /// <summary>
        /// Gets the return type of a method.
        /// </summary>
        /// <param name="jm">The java method.</param>
        /// <param name="jc">The java class.</param>
        /// <returns>The return type of the java method.</returns>
        public static string GetReturnType(this JavaMethod jm, JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref jm, nameof(jm));

            string descriptor = jm.GetDescriptor(jc);

            return descriptor.Substring(descriptor.IndexOf(')') + 1);
        }

        /// <summary>
        /// Gets the parameter types of a method.
        /// </summary>
        /// <param name="jm">The java method.</param>
        /// <param name="jc">The java class.</param>
        /// <returns>The return type of the java method.</returns>
        public static string[] GetParameterTypes(this JavaMethod jm, JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref jm, nameof(jm));

            string descriptor = jm.GetDescriptor(jc);
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

        /// <summary>
        /// Gets the attributes of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the attributes.</typeparam>
        /// <param name="jm">The java method.</param>
        /// <returns>The attributes of the given type.</returns>
        public static T[] GetAttributes<T>(this JavaMethod jm)
            where T : IJavaAttribute
        {
            Guard.NotNull(ref jm, nameof(jm));

            List<T> result = new List<T>();

            foreach (IJavaAttribute attribute in jm.Attributes)
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
        /// <param name="jm">The java method.</param>
        /// <returns>The attribute of the given type.</returns>
        public static T GetAttribute<T>(this JavaMethod jm)
            where T : IJavaAttribute
        {
            Guard.NotNull(ref jm, nameof(jm));

            return jm.GetAttributes<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the parameter names of a method.
        /// </summary>
        /// <param name="jm">The java method.</param>
        /// <param name="jc">The java class.</param>
        /// <returns>The parameter names of the java method.</returns>
        public static string[] GetParameterNames(this JavaMethod jm, JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref jm, nameof(jm));

            JavaAttributeMethodParameters attribute = jm.GetAttribute<JavaAttributeMethodParameters>();
            if (attribute == null)
            {
                int count = jm.GetParameterTypes(jc).Length;
                return Enumerable.Range(0, count).Select(x => $"obj{x}").ToArray();
            }

            return attribute.Parameters.Select(x => jc.GetConstant<JavaConstantUtf8>(x.NameIndex).Value).ToArray();
        }
    }
}
