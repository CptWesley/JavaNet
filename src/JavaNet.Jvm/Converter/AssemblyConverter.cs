using System;
using System.Collections.Generic;
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
                Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();
                foreach (JavaClass jc in classes)
                {
                    assembly.MainModule.Types.Add(ConvertClass(jc, supers, types));
                }

                ResolveBaseTypes(assembly.MainModule, supers, types);

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

        private static TypeDefinition ConvertClass(JavaClass jc, Dictionary<TypeDefinition, string> supers, Dictionary<string, TypeDefinition> types)
        {
            string superName = jc.GetSuperName();
            TypeDefinition result = new TypeDefinition(jc.GetPackageName(), jc.GetName(), jc.GetTypeAttributes());
            supers.Add(result, superName);
            types.Add($"{jc.GetPackageName()}{jc.GetName()}", result);
            return result;
        }

        private static void ResolveBaseTypes(ModuleDefinition module, Dictionary<TypeDefinition, string> supers, Dictionary<string, TypeDefinition> types)
        {
            foreach (KeyValuePair<TypeDefinition, string> pair in supers)
            {
                if (pair.Value == "java/lang/Object")
                {
                    pair.Key.BaseType = module.TypeSystem.Object;
                }
                else
                {
                    pair.Key.BaseType = module.Types.First(x => x.FullName == pair.Value);
                    //pair.Key.BaseType = types[pair.Value];
                }
            }
        }
    }
}
