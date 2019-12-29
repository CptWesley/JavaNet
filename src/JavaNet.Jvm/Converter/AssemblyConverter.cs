using System;
using System.Collections.Generic;
using System.IO;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Fields;
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
                Dictionary<JavaMethod, MethodDefinition> methods = new Dictionary<JavaMethod, MethodDefinition>();
                foreach (JavaClass jc in classes)
                {
                    TypeDefinition definition = ConvertClass(jc);
                    definitions.Add(jc, definition);
                    module.Types.Add(definition);
                }

                foreach (JavaClass jc in classes)
                {
                    TypeDefinition definition = definitions[jc];
                    definition.BaseType = module.ResolveBaseType(jc);
                    foreach (string interfac in jc.GetInterfaces())
                    {
                        definition.Interfaces.Add(new InterfaceImplementation(module.GetJavaType(interfac)));
                    }

                    foreach (JavaField jf in jc.Fields)
                    {
                        definition.Fields.Add(ConvertField(assembly, jc, jf));
                    }

                    foreach (JavaMethod jm in jc.Methods)
                    {
                        MethodDefinition method = DeclareMethod(assembly, jc, jm);
                        methods.Add(jm, method);
                        definition.Methods.Add(method);
                    }
                }

                foreach (JavaClass jc in classes)
                {
                    foreach (JavaMethod jm in jc.Methods)
                    {
                        EmitMethod(methods[jm], jc, jm);
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
            string dotnetNamespace = IdentifierHelper.GetDotNetNamespace(className);
            string dotnetClass = IdentifierHelper.GetDotNetClassName(className);
            TypeDefinition result = new TypeDefinition(dotnetNamespace, dotnetClass, jc.GetAttributes());
            return result;
        }

        private static FieldDefinition ConvertField(AssemblyDefinition assembly, JavaClass jc, JavaField jf)
        {
            string name = jf.GetName(jc);
            FieldAttributes attributes = jf.GetAttributes();
            FieldDefinition result = new FieldDefinition(name, attributes, assembly.MainModule.GetDescriptorType(jf.GetDescriptor(jc)));
            return result;
        }

        private static MethodDefinition DeclareMethod(AssemblyDefinition assembly, JavaClass jc, JavaMethod jm)
        {
            string name = IdentifierHelper.GetDotNetFullName(jm.GetName(jc));
            MethodAttributes attributes = jm.GetAttributes();
            ModuleDefinition module = assembly.MainModule;
            string returnTypeName = jm.GetReturnType(jc);
            TypeReference returnType = module.GetDescriptorType(returnTypeName);

            if (name == ".ctor" || name == ".cctor")
            {
                attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
                attributes &= ~MethodAttributes.Virtual;
            }

            MethodDefinition result = new MethodDefinition(name, attributes, returnType);

            string[] parameterTypes = jm.GetParameterTypes(jc);
            string[] parameterNames = jm.GetParameterNames(jc);

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                result.Parameters.Add(new ParameterDefinition(parameterNames[i], ParameterAttributes.None, module.GetDescriptorType(parameterTypes[i])));
            }

            return result;
        }

        private static void EmitMethod(MethodDefinition method, JavaClass jc, JavaMethod jm)
        {
            ModuleDefinition module = method.Module;
            string[] parameterTypes = jm.GetParameterTypes(jc);
            byte[] code = jm.GetCode();

            ILProcessor il = method.Body?.GetILProcessor();
            if (il != null && code != null)
            {
                new IlEmitter(il, module).EmitMethod(jc, code, method.IsStatic, parameterTypes);
            }
        }
    }
}
