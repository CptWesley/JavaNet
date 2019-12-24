using System;
using System.Collections.Generic;
using System.IO;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Constants;
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
                foreach (JavaClass jc in classes)
                {
                    assembly.MainModule.Types.Add(ConvertClass(assembly, jc));
                }

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

        private static string ToDotNetName(string javaTypeName)
        {
            return javaTypeName.Replace('/', '.');
        }

        private static TypeDefinition ConvertClass(AssemblyDefinition assembly, JavaClass jc)
        {
            ModuleDefinition module = assembly.MainModule;
            string superName = jc.GetSuperName();
            TypeReference baseReference;
            if (superName == "java/lang/Object")
            {
                baseReference = module.TypeSystem.Object;
            }
            else
            {
                baseReference = module.ImportReference(module.GetType(ToDotNetName(superName)));
            }

            TypeDefinition result = new TypeDefinition(jc.GetPackageName(), jc.GetName(), jc.GetTypeAttributes(), baseReference);
            return result;
        }
    }
}
