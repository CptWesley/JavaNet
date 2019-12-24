using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Methods;
using Mono.Cecil;
using Mono.Cecil.Cil;

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
                ModuleDefinition module = assembly.MainModule;
                Dictionary<JavaClass, TypeDefinition> definitions = new Dictionary<JavaClass, TypeDefinition>();
                foreach (JavaClass jc in classes)
                {
                    TypeDefinition definition = ConvertClass(jc);
                    definitions.Add(jc, definition);
                    module.Types.Add(definition);
                }

                foreach (JavaClass jc in classes)
                {
                    TypeDefinition definition = definitions[jc];
                    definition.BaseType = ResolveBaseType(module, jc);
                    foreach (string interfac in jc.GetInterfaces())
                    {
                        definition.Interfaces.Add(new InterfaceImplementation(GetType(module, interfac)));
                    }

                    foreach (JavaMethod jm in jc.Methods)
                    {
                        definition.Methods.Add(ConvertMethod(assembly, jc, jm));
                    }
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

        private static TypeDefinition ConvertClass(JavaClass jc)
        {
            string className = $"{jc.GetPackageName()}{jc.GetName()}";
            TypeDefinition result = new TypeDefinition(GetDotNetNamespace(className), GetDotNetClassName(className), jc.GetAttributes());
            return result;
        }

        private static MethodDefinition ConvertMethod(AssemblyDefinition assembly, JavaClass jc, JavaMethod jm)
        {
            string name = GetDotNetFullName(jm.GetMethodName(jc));
            MethodAttributes attributes = jm.GetAttributes();

            if (name == "<init>" || name == "<clinit>")
            {
                if (name == "<init>")
                {
                    name = ".ctor";
                }
                else if (name == "<clinit>")
                {
                    name = ".cctor";
                }

                attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
            }

            TypeReference returnType = GetDescriptorType(assembly.MainModule, jm.GetReturnType(jc));
            MethodDefinition result = new MethodDefinition(name, attributes, returnType);

            string[] parameterTypes = jm.GetParameterTypes(jc);
            string[] parameterNames = jm.GetParameterNames(jc);

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                result.Parameters.Add(new ParameterDefinition(parameterNames[i], ParameterAttributes.None, GetDescriptorType(assembly.MainModule, parameterTypes[i])));
            }

            ILProcessor il = result.Body?.GetILProcessor();
            if (il != null)
            {
                il.Append(il.Create(OpCodes.Ret));
            }

            return result;
        }

        private static string GetDotNetNamespace(string typeName)
        {
            typeName = GetDotNetFullName(typeName);
            int last = typeName.LastIndexOf('.');
            return last > 0 ? typeName.Substring(0, last) : typeName;
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

        private static TypeReference ResolveBaseType(ModuleDefinition module, JavaClass jc)
        {
            string superName = jc.GetSuperName();

            if (superName == jc.GetName())
            {
                return module.TypeSystem.Object;
            }

            if (superName != "java/lang/Object" || !jc.AccessFlags.HasFlag(JavaClassAccessFlags.Interface))
            {
                return GetType(module, superName);
            }

            return null;
        }

        private static TypeDefinition GetType(ModuleDefinition module, string javaTypeName)
            => module.GetType(GetDotNetNamespace(javaTypeName), GetDotNetClassName(javaTypeName));

        private static TypeReference GetDescriptorType(ModuleDefinition module, string javaTypeName)
        {
            TypeSystem types = module.TypeSystem;

            switch (javaTypeName[0])
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
                    return types.Double;
                case 'I':
                    return types.Int32;
                case 'J':
                    return types.Int64;
                case 'L':
                    return GetType(module, javaTypeName.Substring(1, javaTypeName.Length - 2));
                case 'S':
                    return types.Int16;
                case 'Z':
                    return types.Boolean;
                case '[':
                    // TODO: handle arrays correctly.
                    return GetDescriptorType(module, javaTypeName.Substring(1));
                default:
                    throw new Exception();
            }
        }
    }
}
