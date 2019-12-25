using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Fields;
using JavaNet.Jvm.Parser.Instructions;
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

                    foreach (JavaField jf in jc.Fields)
                    {
                        definition.Fields.Add(ConvertField(assembly, jc, jf));
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

        private static FieldDefinition ConvertField(AssemblyDefinition assembly, JavaClass jc, JavaField jf)
        {
            string name = jf.GetName(jc);
            FieldAttributes attributes = jf.GetAttributes();
            FieldDefinition result = new FieldDefinition(name, attributes, GetDescriptorType(assembly.MainModule, jf.GetDescriptor(jc)));
            return result;
        }

        private static MethodDefinition ConvertMethod(AssemblyDefinition assembly, JavaClass jc, JavaMethod jm)
        {
            string name = GetDotNetFullName(jm.GetName(jc));
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
                List<int> wides = GetWides(parameterTypes, result.IsStatic);

                try
                {
                    EmitInstructions(il, jc, jm.GetCode(), result.IsStatic, parameterTypes.Length, wides);
                }
                catch { }
            }

            return result;
        }

        private static List<int> GetWides(string[] types, bool isStatic)
        {
            List<int> wides = new List<int>();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == "J" || types[i] == "D")
                {
                    wides.Add(isStatic ? i : i + 1);
                }
            }

            return wides;
        }

        private static void EmitInstructions(ILProcessor il, JavaClass jc, byte[] code, bool isStatic, int parameters, List<int> wides)
        {
            for (int i = 0; i < code.Length; i++)
            {
                JavaOpCode op = (JavaOpCode)code[i];
                switch (op)
                {
                    case JavaOpCode.Nop:
                        il.Emit(OpCodes.Nop);
                        break;
                    case JavaOpCode.IReturn:
                    case JavaOpCode.LReturn:
                    case JavaOpCode.FReturn:
                    case JavaOpCode.DReturn:
                    case JavaOpCode.AReturn:
                    case JavaOpCode.Return:
                        il.Emit(OpCodes.Ret);
                        break;
                    case JavaOpCode.ILoad0:
                    case JavaOpCode.LLoad0:
                    case JavaOpCode.FLoad0:
                    case JavaOpCode.DLoad0:
                    case JavaOpCode.ALoad0:
                        LoadToIl(il, 0, isStatic, parameters, wides);
                        break;
                    case JavaOpCode.ILoad1:
                    case JavaOpCode.LLoad1:
                    case JavaOpCode.FLoad1:
                    case JavaOpCode.DLoad1:
                    case JavaOpCode.ALoad1:
                        LoadToIl(il, 1, isStatic, parameters, wides);
                        break;
                    case JavaOpCode.ILoad2:
                    case JavaOpCode.LLoad2:
                    case JavaOpCode.FLoad2:
                    case JavaOpCode.DLoad2:
                    case JavaOpCode.ALoad2:
                        LoadToIl(il, 2, isStatic, parameters, wides);
                        break;
                    case JavaOpCode.ILoad3:
                    case JavaOpCode.LLoad3:
                    case JavaOpCode.FLoad3:
                    case JavaOpCode.DLoad3:
                    case JavaOpCode.ALoad3:
                        LoadToIl(il, 3, isStatic, parameters, wides);
                        break;
                    case JavaOpCode.ILoad:
                    case JavaOpCode.LLoad:
                    case JavaOpCode.FLoad:
                    case JavaOpCode.DLoad:
                    case JavaOpCode.ALoad:
                        LoadToIl(il, code[++i], isStatic, parameters, wides);
                        break;
                    case JavaOpCode.AConstNull:
                        il.Emit(OpCodes.Ldnull);
                        break;
                    case JavaOpCode.BiPush:
                        il.Emit(OpCodes.Ldc_I4, (int)code[++i]);
                        break;
                    case JavaOpCode.Ldc:
                        Ldc(il, jc, code[++i]);
                        break;
                    case JavaOpCode.LdcW:
                    case JavaOpCode.Ldc2W:
                        Ldc(il, jc, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IAdd:
                    case JavaOpCode.LAdd:
                    case JavaOpCode.FAdd:
                    case JavaOpCode.DAdd:
                        il.Emit(OpCodes.Add);
                        break;
                    case JavaOpCode.ISub:
                    case JavaOpCode.LSub:
                    case JavaOpCode.FSub:
                    case JavaOpCode.DSub:
                        il.Emit(OpCodes.Sub);
                        break;
                    case JavaOpCode.IMul:
                    case JavaOpCode.LMul:
                    case JavaOpCode.FMul:
                    case JavaOpCode.DMul:
                        il.Emit(OpCodes.Mul);
                        break;
                    case JavaOpCode.IDiv:
                    case JavaOpCode.LDiv:
                    case JavaOpCode.FDiv:
                    case JavaOpCode.DDiv:
                        il.Emit(OpCodes.Div);
                        break;
                    case JavaOpCode.IRem:
                    case JavaOpCode.LRem:
                    case JavaOpCode.FRem:
                    case JavaOpCode.DRem:
                        il.Emit(OpCodes.Rem);
                        break;
                    case JavaOpCode.I2b:
                        il.Emit(OpCodes.Conv_U1);
                        break;
                    case JavaOpCode.I2s:
                        il.Emit(OpCodes.Conv_I2);
                        break;
                    case JavaOpCode.L2i:
                    case JavaOpCode.F2i:
                    case JavaOpCode.D2i:
                        il.Emit(OpCodes.Conv_I4);
                        break;
                    case JavaOpCode.I2l:
                    case JavaOpCode.F2l:
                    case JavaOpCode.D2l:
                        il.Emit(OpCodes.Conv_I8);
                        break;
                    case JavaOpCode.I2f:
                    case JavaOpCode.L2f:
                    case JavaOpCode.D2f:
                        il.Emit(OpCodes.Conv_R4);
                        break;
                    case JavaOpCode.I2d:
                    case JavaOpCode.L2d:
                    case JavaOpCode.F2d:
                        il.Emit(OpCodes.Conv_R8);
                        break;
                    case JavaOpCode.Dup:
                        il.Emit(OpCodes.Dup);
                        break;
                    //default:
                    //    throw new Exception($"Unknown opcode '{op}'.");
                }
            }
        }

        private static ushort Combine(byte a, byte b) => (ushort)((a << 8) | b);

        private static void Ldc(ILProcessor il, JavaClass jc, ushort index)
        {
            IJavaConstant constant = jc.ConstantPool[index];

            if (constant is JavaConstantDouble d)
            {
                il.Emit(OpCodes.Ldc_R8, d.Value);
            }
            else if (constant is JavaConstantFloat f)
            {
                il.Emit(OpCodes.Ldc_R4, f.Value);
            }
            else if (constant is JavaConstantInteger i)
            {
                il.Emit(OpCodes.Ldc_I4, i.Value);
            }
            else if (constant is JavaConstantLong l)
            {
                il.Emit(OpCodes.Ldc_I8, l.Value);
            }
            else if (constant is JavaConstantString s)
            {
                string str = jc.GetConstant<JavaConstantUtf8>(s.StringIndex).Value;
                il.Emit(OpCodes.Ldstr, str);
            }
            else
            {
                throw new Exception($"Unknown constant type for ldc: '{constant.GetType()}'.");
            }
        }

        private static void LoadToIl(ILProcessor il, int index, bool isStatic, int parameters, List<int> wides)
        {
            // 64 bit parameters take up two slots.
            index -= wides.Count(x => x < index);

            if (!isStatic)
            {
                parameters++;
            }

            if (index < parameters)
            {
                switch (index)
                {
                    case 0:
                        il.Emit(OpCodes.Ldarg_0);
                        return;
                    case 1:
                        il.Emit(OpCodes.Ldarg_1);
                        return;
                    case 2:
                        il.Emit(OpCodes.Ldarg_2);
                        return;
                    case 3:
                        il.Emit(OpCodes.Ldarg_3);
                        return;
                    default:
                        il.Emit(OpCodes.Ldarg, (ushort)index);
                        return;
                }
            }
            else
            {
                index -= parameters;
                switch (index)
                {
                    case 0:
                        il.Emit(OpCodes.Ldloc_0);
                        return;
                    case 1:
                        il.Emit(OpCodes.Ldloc_1);
                        return;
                    case 2:
                        il.Emit(OpCodes.Ldloc_2);
                        return;
                    case 3:
                        il.Emit(OpCodes.Ldloc_3);
                        return;
                    default:
                        il.Emit(OpCodes.Ldloc, (ushort)index);
                        return;
                }
            }
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
                    return types.Single;
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
                    return new ArrayType(GetDescriptorType(module, javaTypeName.Substring(1)));
                default:
                    throw new Exception();
            }
        }
    }
}
