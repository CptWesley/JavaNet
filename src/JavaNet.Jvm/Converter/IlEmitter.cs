using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Instructions;
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
        private readonly Dictionary<int, Instruction> first = new Dictionary<int, Instruction>();
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

            for (int i = 0; i < code.Length; i++)
            {
                JavaOpCode op = (JavaOpCode)code[i];
                switch (op)
                {
                    case JavaOpCode.Nop:
                        Emit(i, OpCodes.Nop);
                        break;
                    case JavaOpCode.IReturn:
                    case JavaOpCode.LReturn:
                    case JavaOpCode.FReturn:
                    case JavaOpCode.DReturn:
                    case JavaOpCode.AReturn:
                    case JavaOpCode.Return:
                        Emit(i, OpCodes.Ret);
                        break;
                    case JavaOpCode.ILoad0:
                    case JavaOpCode.LLoad0:
                    case JavaOpCode.FLoad0:
                    case JavaOpCode.DLoad0:
                    case JavaOpCode.ALoad0:
                        Load(i, 0, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad1:
                    case JavaOpCode.LLoad1:
                    case JavaOpCode.FLoad1:
                    case JavaOpCode.DLoad1:
                    case JavaOpCode.ALoad1:
                        Load(i, 1, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad2:
                    case JavaOpCode.LLoad2:
                    case JavaOpCode.FLoad2:
                    case JavaOpCode.DLoad2:
                    case JavaOpCode.ALoad2:
                        Load(i, 2, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad3:
                    case JavaOpCode.LLoad3:
                    case JavaOpCode.FLoad3:
                    case JavaOpCode.DLoad3:
                    case JavaOpCode.ALoad3:
                        Load(i, 3, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad:
                    case JavaOpCode.LLoad:
                    case JavaOpCode.FLoad:
                    case JavaOpCode.DLoad:
                    case JavaOpCode.ALoad:
                        Load(i, code[++i], isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore0:
                    case JavaOpCode.LStore0:
                    case JavaOpCode.FStore0:
                    case JavaOpCode.DStore0:
                    case JavaOpCode.AStore0:
                        Store(i, 0, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore1:
                    case JavaOpCode.LStore1:
                    case JavaOpCode.FStore1:
                    case JavaOpCode.DStore1:
                    case JavaOpCode.AStore1:
                        Store(i, 1, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore2:
                    case JavaOpCode.LStore2:
                    case JavaOpCode.FStore2:
                    case JavaOpCode.DStore2:
                    case JavaOpCode.AStore2:
                        Store(i, 2, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore3:
                    case JavaOpCode.LStore3:
                    case JavaOpCode.FStore3:
                    case JavaOpCode.DStore3:
                    case JavaOpCode.AStore3:
                        Store(i, 3, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore:
                    case JavaOpCode.LStore:
                    case JavaOpCode.FStore:
                    case JavaOpCode.DStore:
                    case JavaOpCode.AStore:
                        Store(i, code[++i], isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IConstM1:
                        Emit(i, OpCodes.Ldc_I4_M1);
                        break;
                    case JavaOpCode.IConst0:
                        Emit(i, OpCodes.Ldc_I4_0);
                        break;
                    case JavaOpCode.IConst1:
                        Emit(i, OpCodes.Ldc_I4_1);
                        break;
                    case JavaOpCode.IConst2:
                        Emit(i, OpCodes.Ldc_I4_2);
                        break;
                    case JavaOpCode.IConst3:
                        Emit(i, OpCodes.Ldc_I4_3);
                        break;
                    case JavaOpCode.IConst4:
                        Emit(i, OpCodes.Ldc_I4_4);
                        break;
                    case JavaOpCode.IConst5:
                        Emit(i, OpCodes.Ldc_I4_5);
                        break;
                    case JavaOpCode.LConst0:
                        Emit(i, OpCodes.Ldc_I8, 0L);
                        break;
                    case JavaOpCode.LConst1:
                        Emit(i, OpCodes.Ldc_I8, 1L);
                        break;
                    case JavaOpCode.FConst0:
                        Emit(i, OpCodes.Ldc_R4, 0f);
                        break;
                    case JavaOpCode.FConst1:
                        Emit(i, OpCodes.Ldc_R4, 1f);
                        break;
                    case JavaOpCode.FConst2:
                        Emit(i, OpCodes.Ldc_R4, 2f);
                        break;
                    case JavaOpCode.DConst0:
                        Emit(i, OpCodes.Ldc_R8, 0d);
                        break;
                    case JavaOpCode.DConst1:
                        Emit(i, OpCodes.Ldc_R8, 1d);
                        break;
                    case JavaOpCode.AConstNull:
                        Emit(i, OpCodes.Ldnull);
                        break;
                    case JavaOpCode.BiPush:
                        Emit(i, OpCodes.Ldc_I4, (int)code[++i]);
                        break;
                    case JavaOpCode.Ldc:
                        Ldc(i, jc, code[++i]);
                        break;
                    case JavaOpCode.LdcW:
                    case JavaOpCode.Ldc2W:
                        Ldc(i, jc, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IAdd:
                    case JavaOpCode.LAdd:
                    case JavaOpCode.FAdd:
                    case JavaOpCode.DAdd:
                        Emit(i, OpCodes.Add);
                        break;
                    case JavaOpCode.ISub:
                    case JavaOpCode.LSub:
                    case JavaOpCode.FSub:
                    case JavaOpCode.DSub:
                        Emit(i, OpCodes.Sub);
                        break;
                    case JavaOpCode.IMul:
                    case JavaOpCode.LMul:
                    case JavaOpCode.FMul:
                    case JavaOpCode.DMul:
                        Emit(i, OpCodes.Mul);
                        break;
                    case JavaOpCode.IDiv:
                    case JavaOpCode.LDiv:
                    case JavaOpCode.FDiv:
                    case JavaOpCode.DDiv:
                        Emit(i, OpCodes.Div);
                        break;
                    case JavaOpCode.IRem:
                    case JavaOpCode.LRem:
                    case JavaOpCode.FRem:
                    case JavaOpCode.DRem:
                        Emit(i, OpCodes.Rem);
                        break;
                    case JavaOpCode.I2b:
                        Emit(i, OpCodes.Conv_U1);
                        break;
                    case JavaOpCode.I2s:
                        Emit(i, OpCodes.Conv_I2);
                        break;
                    case JavaOpCode.L2i:
                    case JavaOpCode.F2i:
                    case JavaOpCode.D2i:
                        Emit(i, OpCodes.Conv_I4);
                        break;
                    case JavaOpCode.I2l:
                    case JavaOpCode.F2l:
                    case JavaOpCode.D2l:
                        Emit(i, OpCodes.Conv_I8);
                        break;
                    case JavaOpCode.I2f:
                    case JavaOpCode.L2f:
                    case JavaOpCode.D2f:
                        Emit(i, OpCodes.Conv_R4);
                        break;
                    case JavaOpCode.I2d:
                    case JavaOpCode.L2d:
                    case JavaOpCode.F2d:
                        Emit(i, OpCodes.Conv_R8);
                        break;
                    case JavaOpCode.Dup:
                        Emit(i, OpCodes.Dup);
                        break;
                    case JavaOpCode.InvokeVirtual:
                    case JavaOpCode.InvokeSpecial:
                        Call(i, jc, Combine(code[++i], code[++i]), false);
                        break;
                    case JavaOpCode.InvokeInterface:
                        Call(i, jc, Combine(code[++i], code[++i]), false);
                        i += 2;
                        break;
                    case JavaOpCode.InvokeDynamic:
                        InvokeDynamic(i, jc, Combine(code[++i], code[++i]));
                        i += 2;
                        break;
                    case JavaOpCode.InvokeStatic:
                        Call(i, jc, Combine(code[++i], code[++i]), true);
                        break;
                    case JavaOpCode.Goto:
                        EmitJump(i, OpCodes.Br, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.GotoW:
                        EmitJump(i, OpCodes.Br, Combine(code[++i], code[++i], code[++i], code[++i]));
                        break;
                    case JavaOpCode.New:
                        Emit(i, OpCodes.Ldtoken, module.GetJavaType(jc, Combine(code[++i], code[++i])));
                        Emit(i, OpCodes.Call, module.GetMethod(typeof(Type), "GetTypeFromHandle"));
                        Emit(i, OpCodes.Call, module.GetMethod(typeof(FormatterServices), "GetUninitializedObject"));
                        break;
                    case JavaOpCode.Pop:
                        Emit(i, il.Create(OpCodes.Pop));
                        break;
                    case JavaOpCode.Pop2:
                        Emit(i, OpCodes.Pop);
                        Emit(i, OpCodes.Pop);
                        break;
                    case JavaOpCode.AThrow:
                        Emit(i, OpCodes.Throw);
                        break;
                    case JavaOpCode.IfICmpEq:
                    case JavaOpCode.IfACmpEq:
                        EmitJump(i, OpCodes.Beq, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpNe:
                    case JavaOpCode.IfACmpNe:
                        EmitJump(i, OpCodes.Bne_Un, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpLt:
                        EmitJump(i, OpCodes.Blt, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpLe:
                        EmitJump(i, OpCodes.Ble, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpGt:
                        EmitJump(i, OpCodes.Bgt, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpGe:
                        EmitJump(i, OpCodes.Bge, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfNull:
                        EmitJump(i, OpCodes.Brfalse, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfNonNull:
                        EmitJump(i, OpCodes.Brtrue, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.ArrayLength:
                        Emit(i, OpCodes.Ldlen);
                        break;
                    case JavaOpCode.GetField:
                        GetField(i, jc, Combine(code[++i], code[++i]), false);
                        break;
                    case JavaOpCode.GetStatic:
                        GetField(i, jc, Combine(code[++i], code[++i]), true);
                        break;
                    case JavaOpCode.PutField:
                        SetField(i, jc, Combine(code[++i], code[++i]), false);
                        break;
                    case JavaOpCode.PutStatic:
                        SetField(i, jc, Combine(code[++i], code[++i]), true);
                        break;
                    default:
                        throw new Exception($"Unknown opcode '{op}'.");
                }
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

        private void Emit(int instructionIndex, OpCode opCode)
            => Emit(instructionIndex, il.Create(opCode));

        private void Emit(int instructionIndex, OpCode opCode, int arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, long arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, float arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, double arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, ushort arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, string arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, TypeReference arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, MethodReference arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, OpCode opCode, FieldReference arg)
            => Emit(instructionIndex, il.Create(opCode, arg));

        private void Emit(int instructionIndex, Instruction instruction)
        {
            if (!first.ContainsKey(instructionIndex))
            {
                first.Add(instructionIndex, instruction);
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
                Emit(instructionIndex, OpCodes.Ldc_R8, d.Value);
            }
            else if (constant is JavaConstantFloat f)
            {
                Emit(instructionIndex, OpCodes.Ldc_R4, f.Value);
            }
            else if (constant is JavaConstantInteger i)
            {
                Emit(instructionIndex, OpCodes.Ldc_I4, i.Value);
            }
            else if (constant is JavaConstantLong l)
            {
                Emit(instructionIndex, OpCodes.Ldc_I8, l.Value);
            }
            else if (constant is JavaConstantString s)
            {
                string str = jc.GetConstant<JavaConstantUtf8>(s.StringIndex).Value;
                Emit(instructionIndex, OpCodes.Ldstr, str);
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
                        Emit(instructionIndex, OpCodes.Ldarg_0);
                        return;
                    case 1:
                        Emit(instructionIndex, OpCodes.Ldarg_1);
                        return;
                    case 2:
                        Emit(instructionIndex, OpCodes.Ldarg_2);
                        return;
                    case 3:
                        Emit(instructionIndex, OpCodes.Ldarg_3);
                        return;
                    default:
                        Emit(instructionIndex, OpCodes.Ldarg, (ushort)index);
                        return;
                }
            }
            else
            {
                index -= parameters;
                switch (index)
                {
                    case 0:
                        Emit(instructionIndex, OpCodes.Ldloc_0);
                        return;
                    case 1:
                        Emit(instructionIndex, OpCodes.Ldloc_1);
                        return;
                    case 2:
                        Emit(instructionIndex, OpCodes.Ldloc_2);
                        return;
                    case 3:
                        Emit(instructionIndex, OpCodes.Ldloc_3);
                        return;
                    default:
                        Emit(instructionIndex, OpCodes.Ldloc, (ushort)index);
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
                Emit(instructionIndex, OpCodes.Starg, (ushort)index);
            }
            else
            {
                index -= parameters;
                switch (index)
                {
                    case 0:
                        Emit(instructionIndex, OpCodes.Stloc_0);
                        return;
                    case 1:
                        Emit(instructionIndex, OpCodes.Stloc_1);
                        return;
                    case 2:
                        Emit(instructionIndex, OpCodes.Stloc_2);
                        return;
                    case 3:
                        Emit(instructionIndex, OpCodes.Stloc_3);
                        return;
                    default:
                        Emit(instructionIndex, OpCodes.Stloc, (ushort)index);
                        return;
                }
            }
        }

        private void Call(int instructionIndex, JavaClass jc, ushort index, bool isStatic)
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
            else
            {
                throw new Exception($"Found constant of invalid type '{constant.GetType().FullName}'.");
            }

            ushort classNameIndex = jc.GetConstant<JavaConstantClass>(classIndex).NameIndex;
            JavaConstantNameAndType nameAndType = jc.GetConstant<JavaConstantNameAndType>(nameAndTypeIndex);
            string descriptor = jc.GetConstant<JavaConstantUtf8>(nameAndType.DescriptorIndex).Value;
            string name = jc.GetConstant<JavaConstantUtf8>(nameAndType.NameIndex).Value;
            OpCode opCode = isStatic ? OpCodes.Call : OpCodes.Callvirt;

            string objectTypeName = jc.GetConstant<JavaConstantUtf8>(classNameIndex).Value;
            TypeDefinition objectType = FindType(objectTypeName);
            if (!objectType.IsPrimitive || name != "clone")
            {
                Emit(instructionIndex, il.Create(opCode, FindMethod(objectType, name, descriptor)));
            }
        }

        private void InvokeDynamic(int instructionIndex, JavaClass jc, ushort index)
        {
            JavaConstantInvokeDynamic constant = jc.GetConstant<JavaConstantInvokeDynamic>(index);
            JavaAttributeBootstrapMethods bootstrapTable = jc.GetAttribute<JavaAttributeBootstrapMethods>();
            JavaBootstrapMethod bootstrapMethod = bootstrapTable.BootstrapMethods[constant.BootstrapMethodAttributeIndex];
            JavaConstantMethodHandle methodHandle = jc.GetConstant<JavaConstantMethodHandle>(bootstrapMethod.MethodReference);

            foreach (ushort i in bootstrapMethod.Arguments)
            {
                Ldc(instructionIndex, jc, i);
            }

            switch (methodHandle.ReferenceKind)
            {
                case ReferenceKind.GetField:
                    GetField(instructionIndex, jc, methodHandle.ReferenceIndex, false);
                    break;
                case ReferenceKind.GetStatic:
                    GetField(instructionIndex, jc, methodHandle.ReferenceIndex, true);
                    break;
                case ReferenceKind.PutField:
                    SetField(instructionIndex, jc, methodHandle.ReferenceIndex, false);
                    break;
                case ReferenceKind.PutStatic:
                    SetField(instructionIndex, jc, methodHandle.ReferenceIndex, true);
                    break;
                case ReferenceKind.InvokeVirtual:
                case ReferenceKind.NewInvokeSpecial:
                case ReferenceKind.InvokeSpecial:
                case ReferenceKind.InvokeInterface:
                    Call(instructionIndex, jc, methodHandle.ReferenceIndex, false);
                    break;
                case ReferenceKind.InvokeStatic:
                    Call(instructionIndex, jc, methodHandle.ReferenceIndex, true);
                    break;
                default:
                    throw new Exception($"Unknown reference kind '{methodHandle.ReferenceKind}'.");
            }
        }

        private void GetField(int instructionIndex, JavaClass jc, ushort index, bool isStatic)
        {
            FieldReference field = FindField(jc, index);
            OpCode opCode = isStatic ? OpCodes.Ldsfld : OpCodes.Ldfld;
            Emit(instructionIndex, opCode, field);
        }

        private void SetField(int instructionIndex, JavaClass jc, ushort index, bool isStatic)
        {
            FieldReference field = FindField(jc, index);
            OpCode opCode = isStatic ? OpCodes.Stsfld : OpCodes.Stfld;
            Emit(instructionIndex, opCode, field);
        }

        private TypeDefinition FindType(string name)
            => name[0] == '[' ? module.GetDescriptorType(name).Resolve() : module.GetJavaType(name);

        private FieldDefinition FindField(JavaClass jc, ushort index)
        {
            JavaConstantFieldReference constant = jc.GetConstant<JavaConstantFieldReference>(index);
            JavaConstantNameAndType nameAndType = jc.GetConstant<JavaConstantNameAndType>(constant.NameAndTypeIndex);
            string objectTypeName = jc.GetConstant<JavaConstantUtf8>(jc.GetConstant<JavaConstantClass>(constant.ClassIndex).NameIndex).Value;
            TypeDefinition objectType = FindType(objectTypeName);
            string fieldName = jc.GetConstant<JavaConstantUtf8>(nameAndType.NameIndex).Value;
            TypeReference fieldType = module.GetReturnType(jc.GetConstant<JavaConstantUtf8>(nameAndType.DescriptorIndex).Value);
            return FindFieldInner(objectType, fieldName, fieldType.FullName);
        }

        private FieldDefinition FindFieldInner(TypeDefinition target, string dotnetName, string fieldTypeName)
        {
            string dotnetFullName = $"{fieldTypeName} {target.FullName}::{dotnetName}";
            FieldDefinition fd = target.Fields.FirstOrDefault(x => x.FullName == dotnetFullName);

            if (fd != null)
            {
                return fd;
            }
            else if (target.BaseType != null)
            {
                fd = FindFieldInner(target.BaseType?.Resolve(), dotnetName, fieldTypeName);
                if (fd != null)
                {
                    return fd;
                }
            }

            foreach (TypeDefinition interfaceDefinition in target.Interfaces.Select(x => x.InterfaceType.Resolve()))
            {
                fd = FindFieldInner(interfaceDefinition, dotnetName, fieldTypeName);
                if (fd != null)
                {
                    return fd;
                }
            }

            return null;
        }

        private MethodDefinition FindMethod(TypeDefinition target, string javaName, string descriptor)
        {
            TypeReference returnType = module.GetReturnType(descriptor);
            TypeReference[] parameterTypes = module.GetParameterTypes(descriptor);
            string paramString = string.Join(",", parameterTypes.Select(x => x.FullName));
            string dotnetName = IdentifierHelper.GetDotNetFullName(javaName);
            MethodDefinition result = FindMethodInner(target, dotnetName, returnType.FullName, paramString);

            if (result == null)
            {
                throw new Exception($"Could not find C# method for JVM signature '{javaName}{descriptor}' on class '{target.FullName}'.");
            }

            return result;
        }

        private MethodDefinition FindMethodInner(TypeDefinition target, string dotnetName, string returnTypeName, string paramString)
        {
            string dotnetFullName = $"{returnTypeName} {target.FullName}::{dotnetName}({paramString})";
            MethodDefinition md = target.Methods.FirstOrDefault(x => x.FullName == dotnetFullName);

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
