using System.Linq;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Util;
using Mono.Cecil;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Extension methods for the <see cref="JavaClass"/> class.
    /// </summary>
    public static class JavaClassExtensions
    {
        /// <summary>
        /// Gets the class name.
        /// </summary>
        /// <param name="jc">The java class.</param>
        /// <returns>The name of the java class.</returns>
        public static string GetName(this JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));

            JavaConstantClass cc = jc.GetConstant<JavaConstantClass>(jc.ThisClassIndex);
            return jc.GetConstant<JavaConstantUtf8>(cc.NameIndex).Value;
        }

        /// <summary>
        /// Gets the package name.
        /// </summary>
        /// <param name="jc">The java class.</param>
        /// <returns>The name of the package of the class.</returns>
        public static string GetPackageName(this JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));

            foreach (IJavaConstant constant in jc.ConstantPool)
            {
                if (constant is JavaConstantPackage pkg)
                {
                    return jc.GetConstant<JavaConstantUtf8>(pkg.NameIndex).Value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the super class of the given class.
        /// </summary>
        /// <param name="jc">The java class.</param>
        /// <returns>The name of the super class.</returns>
        public static string GetSuperName(this JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));

            if (jc.SuperClassIndex == 0)
            {
                return "java/lang/Object";
            }

            JavaConstantClass super = jc.GetConstant<JavaConstantClass>(jc.SuperClassIndex);
            return jc.GetConstant<JavaConstantUtf8>(super.NameIndex).Value;
        }

        /// <summary>
        /// Gets the names of the interfaces of the given class.
        /// </summary>
        /// <param name="jc">The java class.</param>
        /// <returns>The names of the interfaces of the class.</returns>
        public static string[] GetInterfaces(this JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));
            return jc.Interfaces.Select(x => jc.GetConstant<JavaConstantUtf8>(jc.GetConstant<JavaConstantClass>(x).NameIndex).Value).ToArray();
        }

        /// <summary>
        /// Gets the constant from the constant pool.
        /// </summary>
        /// <typeparam name="T">The type of the constant.</typeparam>
        /// <param name="jc">The java class.</param>
        /// <param name="index">The index of the constant in the constant pool.</param>
        /// <returns>The java constant.</returns>
        public static T GetConstant<T>(this JavaClass jc, ushort index)
            where T : IJavaConstant
        {
            Guard.NotNull(ref jc, nameof(jc));
            return (T)jc.ConstantPool[index];
        }

        /// <summary>
        /// Gets the type attributes.
        /// </summary>
        /// <param name="jc">The java class.</param>
        /// <returns>The type attributes of the class.</returns>
        public static TypeAttributes GetAttributes(this JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));

            JavaClassAccessFlags accessFlags = jc.AccessFlags;

            TypeAttributes result = TypeAttributes.Class | TypeAttributes.AutoLayout;

            if (accessFlags.HasFlag(JavaClassAccessFlags.Abstract))
            {
                result |= TypeAttributes.Abstract;
            }

            if (accessFlags.HasFlag(JavaClassAccessFlags.Final))
            {
                result |= TypeAttributes.Sealed;
            }

            if (accessFlags.HasFlag(JavaClassAccessFlags.Public))
            {
                result |= TypeAttributes.Public;
            }
            else
            {
                result |= TypeAttributes.NotPublic;
            }

            if (accessFlags.HasFlag(JavaClassAccessFlags.Interface))
            {
                result |= TypeAttributes.Interface;
            }

            return result;
        }

        /// <summary>
        /// Gets the attributes of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the attributes.</typeparam>
        /// <param name="jc">The java class.</param>
        /// <returns>The attributes of the given type.</returns>
        public static T[] GetAttributes<T>(this JavaClass jc)
            where T : IJavaAttribute
        {
            Guard.NotNull(ref jc, nameof(jc));

            return jc.Attributes.Where(x => x is T).Select(x => (T)x).ToArray();
        }

        /// <summary>
        /// Gets the attribute of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="jc">The java class.</param>
        /// <returns>The attribute of the given type.</returns>
        public static T GetAttribute<T>(this JavaClass jc)
            where T : IJavaAttribute
        {
            Guard.NotNull(ref jc, nameof(jc));

            return jc.GetAttributes<T>().FirstOrDefault();
        }
    }
}
