using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Instructions;
using JavaNet.Jvm.Parser.Methods;
using JavaNet.Jvm.Util;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace JavaNet.Jvm.Converter
{
    /// <summary>
    /// Class used for emitting ilcode.
    /// </summary>
    public class IlEmitter
    {
        private readonly ILProcessor il;
        private readonly ModuleDefinition module;
        private readonly List<Instruction> first = new List<Instruction>();
        private readonly List<(Instruction Instruction, OpCode OpCode, int TargetAddress)> jumps = new List<(Instruction, OpCode, int)>();

        /// <summary>
        /// Initializes a new instance of the <see cref="IlEmitter"/> class.
        /// </summary>
        /// <param name="il">The il processor.</param>
        /// <param name="module">The cecil module definition.</param>
        public IlEmitter(ILProcessor il, ModuleDefinition module)
        {
            this.il = il;
            this.module = module;
        }

        /// <summary>
        /// Emits the specified byte code as il code.
        /// </summary>
        /// <param name="jc">The java class context.</param>
        /// <param name="code">The java byte code.</param>
        /// <param name="isStatic">If set to <c>true</c> the method containing the byte code is static.</param>
        /// <param name="parametersTypes">The parameters types.</param>
        public void EmitMethod(JavaClass jc, byte[] code, bool isStatic, string[] parametersTypes)
        {
            Guard.NotNull(ref jc, nameof(jc));
            Guard.NotNull(ref code, nameof(code));
            Guard.NotNull(ref parametersTypes, nameof(parametersTypes));

            int address = 0;

            for (int i = 0; i < code.Length; i++)
            {
                JavaOpCode op = (JavaOpCode)code[i];
                switch (op)
                {
                    case JavaOpCode.Nop:
                        Emit(address, il.Create(OpCodes.Nop));
                        break;
                    case JavaOpCode.IReturn:
                    case JavaOpCode.LReturn:
                    case JavaOpCode.FReturn:
                    case JavaOpCode.DReturn:
                    case JavaOpCode.AReturn:
                    case JavaOpCode.Return:
                        Emit(address, il.Create(OpCodes.Ret));
                        break;
                    case JavaOpCode.ILoad0:
                    case JavaOpCode.LLoad0:
                    case JavaOpCode.FLoad0:
                    case JavaOpCode.DLoad0:
                    case JavaOpCode.ALoad0:
                        Load(address, 0, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad1:
                    case JavaOpCode.LLoad1:
                    case JavaOpCode.FLoad1:
                    case JavaOpCode.DLoad1:
                    case JavaOpCode.ALoad1:
                        Load(address, 1, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad2:
                    case JavaOpCode.LLoad2:
                    case JavaOpCode.FLoad2:
                    case JavaOpCode.DLoad2:
                    case JavaOpCode.ALoad2:
                        Load(address, 2, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad3:
                    case JavaOpCode.LLoad3:
                    case JavaOpCode.FLoad3:
                    case JavaOpCode.DLoad3:
                    case JavaOpCode.ALoad3:
                        Load(address, 3, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad:
                    case JavaOpCode.LLoad:
                    case JavaOpCode.FLoad:
                    case JavaOpCode.DLoad:
                    case JavaOpCode.ALoad:
                        Load(address, code[++i], isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore0:
                    case JavaOpCode.LStore0:
                    case JavaOpCode.FStore0:
                    case JavaOpCode.DStore0:
                    case JavaOpCode.AStore0:
                        Store(address, 0, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore1:
                    case JavaOpCode.LStore1:
                    case JavaOpCode.FStore1:
                    case JavaOpCode.DStore1:
                    case JavaOpCode.AStore1:
                        Store(address, 1, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore2:
                    case JavaOpCode.LStore2:
                    case JavaOpCode.FStore2:
                    case JavaOpCode.DStore2:
                    case JavaOpCode.AStore2:
                        Store(address, 2, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore3:
                    case JavaOpCode.LStore3:
                    case JavaOpCode.FStore3:
                    case JavaOpCode.DStore3:
                    case JavaOpCode.AStore3:
                        Store(address, 3, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore:
                    case JavaOpCode.LStore:
                    case JavaOpCode.FStore:
                    case JavaOpCode.DStore:
                    case JavaOpCode.AStore:
                        Store(address, code[++i], isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IConstM1:
                        Emit(address, il.Create(OpCodes.Ldc_I4_M1));
                        break;
                    case JavaOpCode.IConst0:
                        Emit(address, il.Create(OpCodes.Ldc_I4_0));
                        break;
                    case JavaOpCode.IConst1:
                        Emit(address, il.Create(OpCodes.Ldc_I4_1));
                        break;
                    case JavaOpCode.IConst2:
                        Emit(address, il.Create(OpCodes.Ldc_I4_2));
                        break;
                    case JavaOpCode.IConst3:
                        Emit(address, il.Create(OpCodes.Ldc_I4_3));
                        break;
                    case JavaOpCode.IConst4:
                        Emit(address, il.Create(OpCodes.Ldc_I4_4));
                        break;
                    case JavaOpCode.IConst5:
                        Emit(address, il.Create(OpCodes.Ldc_I4_5));
                        break;
                    case JavaOpCode.LConst0:
                        Emit(address, il.Create(OpCodes.Ldc_I8, 0L));
                        break;
                    case JavaOpCode.LConst1:
                        Emit(address, il.Create(OpCodes.Ldc_I8, 1L));
                        break;
                    case JavaOpCode.FConst0:
                        Emit(address, il.Create(OpCodes.Ldc_R4, 0f));
                        break;
                    case JavaOpCode.FConst1:
                        Emit(address, il.Create(OpCodes.Ldc_R4, 1f));
                        break;
                    case JavaOpCode.FConst2:
                        Emit(address, il.Create(OpCodes.Ldc_R4, 2f));
                        break;
                    case JavaOpCode.DConst0:
                        Emit(address, il.Create(OpCodes.Ldc_R8, 0d));
                        break;
                    case JavaOpCode.DConst1:
                        Emit(address, il.Create(OpCodes.Ldc_R8, 1d));
                        break;
                    case JavaOpCode.AConstNull:
                        Emit(address, il.Create(OpCodes.Ldnull));
                        break;
                    case JavaOpCode.BiPush:
                        Emit(address, il.Create(OpCodes.Ldc_I4, (int)code[++i]));
                        break;
                    case JavaOpCode.Ldc:
                        Ldc(address, jc, code[++i]);
                        break;
                    case JavaOpCode.LdcW:
                    case JavaOpCode.Ldc2W:
                        Ldc(address, jc, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IAdd:
                    case JavaOpCode.LAdd:
                    case JavaOpCode.FAdd:
                    case JavaOpCode.DAdd:
                        Emit(address, il.Create(OpCodes.Add));
                        break;
                    case JavaOpCode.ISub:
                    case JavaOpCode.LSub:
                    case JavaOpCode.FSub:
                    case JavaOpCode.DSub:
                        Emit(address, il.Create(OpCodes.Sub));
                        break;
                    case JavaOpCode.IMul:
                    case JavaOpCode.LMul:
                    case JavaOpCode.FMul:
                    case JavaOpCode.DMul:
                        Emit(address, il.Create(OpCodes.Mul));
                        break;
                    case JavaOpCode.IDiv:
                    case JavaOpCode.LDiv:
                    case JavaOpCode.FDiv:
                    case JavaOpCode.DDiv:
                        Emit(address, il.Create(OpCodes.Div));
                        break;
                    case JavaOpCode.IRem:
                    case JavaOpCode.LRem:
                    case JavaOpCode.FRem:
                    case JavaOpCode.DRem:
                        Emit(address, il.Create(OpCodes.Rem));
                        break;
                    case JavaOpCode.I2b:
                        Emit(address, il.Create(OpCodes.Conv_U1));
                        break;
                    case JavaOpCode.I2s:
                        Emit(address, il.Create(OpCodes.Conv_I2));
                        break;
                    case JavaOpCode.L2i:
                    case JavaOpCode.F2i:
                    case JavaOpCode.D2i:
                        Emit(address, il.Create(OpCodes.Conv_I4));
                        break;
                    case JavaOpCode.I2l:
                    case JavaOpCode.F2l:
                    case JavaOpCode.D2l:
                        Emit(address, il.Create(OpCodes.Conv_I8));
                        break;
                    case JavaOpCode.I2f:
                    case JavaOpCode.L2f:
                    case JavaOpCode.D2f:
                        Emit(address, il.Create(OpCodes.Conv_R4));
                        break;
                    case JavaOpCode.I2d:
                    case JavaOpCode.L2d:
                    case JavaOpCode.F2d:
                        Emit(address, il.Create(OpCodes.Conv_R8));
                        break;
                    case JavaOpCode.Dup:
                        Emit(address, il.Create(OpCodes.Dup));
                        break;
                    case JavaOpCode.InvokeSpecial:
                    case JavaOpCode.InvokeVirtual:
                        CallVirt(address, jc, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.Goto:
                        EmitJump(address, OpCodes.Br, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.GotoW:
                        EmitJump(address, OpCodes.Br, Combine(code[++i], code[++i], code[++i], code[++i]));
                        break;
                    case JavaOpCode.New:
                        Emit(address, il.Create(OpCodes.Ldtoken, module.GetJavaType(jc, Combine(code[++i], code[++i]))));
                        Emit(address, il.Create(OpCodes.Call, module.GetMethod(typeof(Type), "GetTypeFromHandle")));
                        Emit(address, il.Create(OpCodes.Call, module.GetMethod(typeof(FormatterServices), "GetUninitializedObject")));
                        break;
                    case JavaOpCode.Pop:
                        Emit(address, il.Create(OpCodes.Pop));
                        break;
                    case JavaOpCode.Pop2:
                        Emit(address, il.Create(OpCodes.Pop));
                        Emit(address, il.Create(OpCodes.Pop));
                        break;
                    default:
                        throw new Exception($"Unknown opcode '{op}'.");
                        return;
                }

                address++;
            }

            foreach (var jump in jumps)
            {
                Console.WriteLine($"Starting jump in: {jc.GetName()}");
                Instruction targetInstruction = first[jump.TargetAddress];
                Instruction newInstruction = il.Create(jump.OpCode, targetInstruction);
                il.Replace(jump.Instruction, newInstruction);
                Console.WriteLine($"Finished jump in: {jc.GetName()}");
            }
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

        private static ushort Combine(byte a, byte b) => (ushort)((a << 8) | b);

        private static int Combine(byte a, byte b, byte c, byte d) => (a << 24) | (b << 16) | (c << 8) | d;

        private void Emit(int instructionIndex, Instruction instruction)
        {
            if (first.Count == instructionIndex)
            {
                first.Add(instruction);
            }

            il.Append(instruction);
        }

        private void EmitJump(int instructionIndex, OpCode op, int offset)
        {
            Instruction instruction = il.Create(OpCodes.Nop);
            Emit(instructionIndex, instruction);
            jumps.Add((instruction, op, instructionIndex + offset));
        }

        private void Ldc(int instructionIndex, JavaClass jc, ushort index)
        {
            IJavaConstant constant = jc.ConstantPool[index];

            if (constant is JavaConstantDouble d)
            {
                Emit(instructionIndex, il.Create(OpCodes.Ldc_R8, d.Value));
            }
            else if (constant is JavaConstantFloat f)
            {
                Emit(instructionIndex, il.Create(OpCodes.Ldc_R4, f.Value));
            }
            else if (constant is JavaConstantInteger i)
            {
                Emit(instructionIndex, il.Create(OpCodes.Ldc_I4, i.Value));
            }
            else if (constant is JavaConstantLong l)
            {
                Emit(instructionIndex, il.Create(OpCodes.Ldc_I8, l.Value));
            }
            else if (constant is JavaConstantString s)
            {
                string str = jc.GetConstant<JavaConstantUtf8>(s.StringIndex).Value;
                Emit(instructionIndex, il.Create(OpCodes.Ldstr, str));
            }
            else
            {
                throw new Exception($"Unknown constant type for ldc: '{constant.GetType()}' in '{jc.GetName()}'.");
            }
        }

        private void Load(int instructionIndex, int index, bool isStatic, string[] parametersTypes)
        {
            // 64 bit parameters take up two slots.
            index -= GetWides(parametersTypes, isStatic).Count(x => x < index);
            int parameters = parametersTypes.Length;

            if (!isStatic)
            {
                parameters++;
            }

            if (index < parameters)
            {
                switch (index)
                {
                    case 0:
                        Emit(instructionIndex, il.Create(OpCodes.Ldarg_0));
                        return;
                    case 1:
                        Emit(instructionIndex, il.Create(OpCodes.Ldarg_1));
                        return;
                    case 2:
                        Emit(instructionIndex, il.Create(OpCodes.Ldarg_2));
                        return;
                    case 3:
                        Emit(instructionIndex, il.Create(OpCodes.Ldarg_3));
                        return;
                    default:
                        Emit(instructionIndex, il.Create(OpCodes.Ldarg, (ushort)index));
                        return;
                }
            }
            else
            {
                index -= parameters;
                switch (index)
                {
                    case 0:
                        Emit(instructionIndex, il.Create(OpCodes.Ldloc_0));
                        return;
                    case 1:
                        Emit(instructionIndex, il.Create(OpCodes.Ldloc_1));
                        return;
                    case 2:
                        Emit(instructionIndex, il.Create(OpCodes.Ldloc_2));
                        return;
                    case 3:
                        Emit(instructionIndex, il.Create(OpCodes.Ldloc_3));
                        return;
                    default:
                        Emit(instructionIndex, il.Create(OpCodes.Ldloc, (ushort)index));
                        return;
                }
            }
        }

        private void Store(int instructionIndex, int index, bool isStatic, string[] parametersTypes)
        {
            // 64 bit parameters take up two slots.
            index -= GetWides(parametersTypes, isStatic).Count(x => x < index);
            int parameters = parametersTypes.Length;

            if (!isStatic)
            {
                parameters++;
            }

            if (index < parameters)
            {
                Emit(instructionIndex, il.Create(OpCodes.Starg, (ushort)index));
            }
            else
            {
                index -= parameters;
                switch (index)
                {
                    case 0:
                        Emit(instructionIndex, il.Create(OpCodes.Stloc_0));
                        return;
                    case 1:
                        Emit(instructionIndex, il.Create(OpCodes.Stloc_1));
                        return;
                    case 2:
                        Emit(instructionIndex, il.Create(OpCodes.Stloc_2));
                        return;
                    case 3:
                        Emit(instructionIndex, il.Create(OpCodes.Stloc_3));
                        return;
                    default:
                        Emit(instructionIndex, il.Create(OpCodes.Stloc, (ushort)index));
                        return;
                }
            }
        }

        private void CallVirt(int instructionIndex, JavaClass jc, ushort index)
        {
            IJavaConstant constant = jc.ConstantPool[index];

            ushort classIndex = ushort.MaxValue;
            ushort nameAndTypeIndex = ushort.MaxValue;
            if (constant is JavaConstantMethodReference m)
            {
                classIndex = m.ClassIndex;
                nameAndTypeIndex = m.NameAndTypeIndex;
            }
            else if (constant is JavaConstantInterfaceMethodReference i)
            {
                classIndex = i.ClassIndex;
                nameAndTypeIndex = i.NameAndTypeIndex;
            }

            ushort classNameIndex = jc.GetConstant<JavaConstantClass>(classIndex).NameIndex;
            JavaConstantNameAndType nameAndType = jc.GetConstant<JavaConstantNameAndType>(nameAndTypeIndex);
            string descriptor = jc.GetConstant<JavaConstantUtf8>(nameAndType.DescriptorIndex).Value;
            string name = jc.GetConstant<JavaConstantUtf8>(nameAndType.NameIndex).Value;

            string objectTypeName = jc.GetConstant<JavaConstantUtf8>(classNameIndex).Value;
            TypeDefinition objectType = objectTypeName[0] == '[' ? module.GetDescriptorType(objectTypeName).Resolve() : module.GetJavaType(objectTypeName);
            if (!objectType.IsPrimitive || name != "clone")
            {
                Emit(instructionIndex, il.Create(OpCodes.Callvirt, FindMethod(objectType, name, descriptor)));
            }
        }

        private MethodReference FindMethod(TypeDefinition target, string javaName, string descriptor)
        {
            bool print = javaName == "descriptorString";

            TypeReference returnType = module.GetReturnType(descriptor);
            TypeReference[] parameterTypes = module.GetParameterTypes(descriptor);
            string paramString = string.Join(",", parameterTypes.Select(x => x.FullName));
            string dotnetName = IdentifierHelper.GetDotNetFullName(javaName);
            MethodReference result = FindMethodInner(target, dotnetName, returnType.FullName, paramString);

            if (result == null)
            {
                throw new Exception($"Could not find C# method for JVM signature '{javaName}{descriptor}' on class '{target.FullName}'.");
            }

            return result;
        }

        private MethodReference FindMethodInner(TypeDefinition target, string dotnetName, string returnTypeName, string paramString)
        {
            string dotnetFullName = $"{returnTypeName} {target.FullName}::{dotnetName}({paramString})";
            MethodReference md = target.Methods.FirstOrDefault(x => x.FullName == dotnetFullName);

            if (md != null)
            {
                return md;
            }
            else if (target.BaseType != null)
            {
                md = FindMethodInner(target.BaseType?.Resolve(), dotnetName, returnTypeName, paramString);
                if (md != null)
                {
                    return md;
                }
            }

            foreach (TypeDefinition interfaceDefinition in target.Interfaces.Select(x => x.InterfaceType.Resolve()))
            {
                md = FindMethodInner(interfaceDefinition, dotnetName, returnTypeName, paramString);
                if (md != null)
                {
                    return md;
                }
            }

            return null;
        }
    }
}
