using System;
using System.Collections.Generic;
using System.Linq;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Util;
using Mono.Cecil;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Provides extension methods for <see cref="ModuleDefinition"/> instances.
    /// </summary>
    public static class ModuleDefinitionExtensions
    {
        /// <summary>
        /// Gets the type reference from a module from a java type names.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="javaTypeName">Name of the java type.</param>
        /// <returns>The type reference.</returns>
        public static TypeDefinition GetJavaType(this ModuleDefinition module, string javaTypeName)
        {
            Guard.NotNull(ref module, nameof(module));
            Guard.NotNull(ref javaTypeName, nameof(javaTypeName));

            string dotnetNamespace = IdentifierHelper.GetDotNetNamespace(javaTypeName);
            string dotnetClass = IdentifierHelper.GetDotNetClassName(javaTypeName);
            return module.GetType(dotnetNamespace, dotnetClass);
        }

        /// <summary>
        /// Gets the type reference from a module from a java type names.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="jc">Java class.</param>
        /// <returns>The type reference.</returns>
        public static TypeReference ResolveBaseType(this ModuleDefinition module, JavaClass jc)
        {
            Guard.NotNull(ref module, nameof(module));
            Guard.NotNull(ref jc, nameof(jc));
            string superName = jc.GetSuperName();

            if (superName == jc.GetName())
            {
                return module.TypeSystem.Object;
            }

            if (superName != "java/lang/Object" || !jc.AccessFlags.HasFlag(JavaClassAccessFlags.Interface))
            {
                return module.GetJavaType(superName);
            }

            return null;
        }

        /// <summary>
        /// Gets the type reference from a module from a java descriptor.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="descriptor">Java type descriptor.</param>
        /// <returns>The type reference.</returns>
        public static TypeReference GetDescriptorType(this ModuleDefinition module, string descriptor)
        {
            Guard.NotNull(ref module, nameof(module));
            Guard.NotNull(ref descriptor, nameof(descriptor));
            TypeSystem types = module.TypeSystem;

            switch (descriptor[0])
            {
                case 'V':
                    return types.Void;
                case 'B':
                    return types.Byte;
                case 'C':
                    return types.Char;
                case 'D':
                    return types.Double;
                case 'F':
                    return types.Single;
                case 'I':
                    return types.Int32;
                case 'J':
                    return types.Int64;
                case 'L':
                    return module.GetJavaType(descriptor.Substring(1, descriptor.Length - 2));
                case 'S':
                    return types.Int16;
                case 'Z':
                    return types.Boolean;
                case '[':
                    return new ArrayType(GetDescriptorType(module, descriptor.Substring(1)));
                default:
                    throw new ArgumentException($"Illegal first character found '{descriptor[0]}'.");
            }
        }

        /// <summary>
        /// Gets the return type of a method.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="descriptor">The java type descriptor.</param>
        /// <returns>The return type of the java descriptor.</returns>
        public static TypeReference GetReturnType(this ModuleDefinition module, string descriptor)
        {
            Guard.NotNull(ref module, nameof(module));
            Guard.NotNull(ref descriptor, nameof(descriptor));

            return module.GetDescriptorType(DescriptorHelper.GetReturn(descriptor));
        }

        /// <summary>
        /// Gets the parameter types of a method.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="descriptor">The java type descriptor.</param>
        /// <returns>The parameter types of the java descriptor.</returns>
        public static TypeReference[] GetParameterTypes(this ModuleDefinition module, string descriptor)
        {
            Guard.NotNull(ref module, nameof(module));
            Guard.NotNull(ref descriptor, nameof(descriptor));

            string[] parameters = DescriptorHelper.GetParameters(descriptor);
            return parameters.Select(x => module.GetDescriptorType(x)).ToArray();
        }
    }
}
