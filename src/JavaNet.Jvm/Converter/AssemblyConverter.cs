using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JavaNet.Jvm.Parser;
using Mono.Cecil;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Converts java classes to cecil type definitions.
    /// </summary>
    public class AssemblyConverter
    {
        private readonly List<JavaClass> classes = new List<JavaClass>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyConverter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public AssemblyConverter(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Adds a java class to the conversion.
        /// </summary>
        /// <param name="jc">The java class to include.</param>
        public void Include(JavaClass jc)
        {
            classes.Add(jc);
        }

        /// <summary>
        /// Converts the aggregated classes to a .NET assembly.
        /// </summary>
        /// <returns>The bytes of a .NET assembly.</returns>
        public byte[] Convert()
        {
            AssemblyNameDefinition nameDefinition = new AssemblyNameDefinition(Name, new Version(1, 0, 0, 0));
            using (AssemblyDefinition assembly = AssemblyDefinition.CreateAssembly(nameDefinition, Name, ModuleKind.Dll))
            {
                Dictionary<TypeDefinition, string> supers = new Dictionary<TypeDefinition, string>();
                foreach (JavaClass jc in classes)
                {
                    assembly.MainModule.Types.Add(ConvertClass(jc, supers));
                }

                ResolveBaseTypes(assembly.MainModule, supers);

                return AssemblyDefinitionToBytes(assembly);
            }
        }

        private static byte[] AssemblyDefinitionToBytes(AssemblyDefinition assembly)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                assembly.Write(ms);
                return ms.ToArray();
            }
        }

        private static TypeDefinition ConvertClass(JavaClass jc, Dictionary<TypeDefinition, string> supers)
        {
            string className = $"{jc.GetPackageName()}{jc.GetName()}";
            string superName = jc.GetSuperName();
            TypeDefinition result = new TypeDefinition(GetDotNetNamespace(className), GetDotNetClassName(className), jc.GetTypeAttributes());
            supers.Add(result, superName);
            return result;
        }

        private static string GetDotNetNamespace(string typeName)
        {
            typeName = GetDotNetFullName(typeName);
            int last = typeName.LastIndexOf('.');
            return typeName.Substring(0, last);
        }

        private static string GetDotNetClassName(string typeName)
        {
            typeName = GetDotNetFullName(typeName);
            int last = typeName.LastIndexOf('.');
            return typeName.Substring(last + 1);
        }

        private static string GetDotNetFullName(string typeName)
        {
            typeName = char.ToUpper(typeName[0], CultureInfo.InvariantCulture) + (typeName.Length > 1 ? typeName.Substring(1) : string.Empty);

            for (int i = 0; i < typeName.Length; i++)
            {
                if (typeName[i] == '/')
                {
                    typeName = typeName.Substring(0, i) + '.' + char.ToUpper(typeName[++i], CultureInfo.InvariantCulture) + typeName.Substring(i + 1, typeName.Length - i - 1);
                }
            }

            return typeName;
        }

        private static void ResolveBaseTypes(ModuleDefinition module, Dictionary<TypeDefinition, string> supers)
        {
            foreach (KeyValuePair<TypeDefinition, string> pair in supers)
            {
                if (pair.Key.FullName == $"{GetDotNetNamespace(pair.Value)}.{GetDotNetClassName(pair.Value)}")
                {
                    pair.Key.BaseType = module.TypeSystem.Object;
                }
                else if (pair.Value != "java/lang/Object" || !pair.Key.Attributes.HasFlag(TypeAttributes.Interface))
                {
                    pair.Key.BaseType = module.GetType(GetDotNetNamespace(pair.Value), GetDotNetClassName(pair.Value));
                }
            }
        }
    }
}
