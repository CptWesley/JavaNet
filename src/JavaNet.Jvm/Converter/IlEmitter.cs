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
        public static readonly HashSet<JavaOpCode> missing = new HashSet<JavaOpCode>();
        private readonly ILProcessor il;
        private readonly ModuleDefinition module;
        private readonly Dictionary<int, Instruction> first = new Dictionary<int, Instruction>();
        private readonly List<(Instruction Instruction, OpCode OpCode, int TargetAddress)> jumps = new List<(Instruction, OpCode, int)>();
        private readonly Dictionary<int, VariableDefinition> locals = new Dictionary<int, VariableDefinition>();

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
                int address = i;
                JavaOpCode op = (JavaOpCode)code[i];
                switch (op)
                {
                    case JavaOpCode.Nop:
                        Emit(address, OpCodes.Nop);
                        break;
                    case JavaOpCode.IReturn:
                    case JavaOpCode.LReturn:
                    case JavaOpCode.FReturn:
                    case JavaOpCode.DReturn:
                    case JavaOpCode.AReturn:
                    case JavaOpCode.Return:
                        Emit(address, OpCodes.Ret);
                        break;
                    case JavaOpCode.ILoad0:
                    case JavaOpCode.LLoad0:
                    case JavaOpCode.FLoad0:
                    case JavaOpCode.DLoad0:
                    case JavaOpCode.ALoad0:
                        Load(address, 0, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.ILoad1:
                    case JavaOpCode.LLoad1:
                    case JavaOpCode.FLoad1:
                    case JavaOpCode.DLoad1:
                    case JavaOpCode.ALoad1:
                        Load(address, 1, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.ILoad2:
                    case JavaOpCode.LLoad2:
                    case JavaOpCode.FLoad2:
                    case JavaOpCode.DLoad2:
                    case JavaOpCode.ALoad2:
                        Load(address, 2, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.ILoad3:
                    case JavaOpCode.LLoad3:
                    case JavaOpCode.FLoad3:
                    case JavaOpCode.DLoad3:
                    case JavaOpCode.ALoad3:
                        Load(address, 3, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.ILoad:
                    case JavaOpCode.LLoad:
                    case JavaOpCode.FLoad:
                    case JavaOpCode.DLoad:
                    case JavaOpCode.ALoad:
                        Load(address, code[++i], isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.IStore0:
                    case JavaOpCode.LStore0:
                    case JavaOpCode.FStore0:
                    case JavaOpCode.DStore0:
                    case JavaOpCode.AStore0:
                        Store(address, 0, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.IStore1:
                    case JavaOpCode.LStore1:
                    case JavaOpCode.FStore1:
                    case JavaOpCode.DStore1:
                    case JavaOpCode.AStore1:
                        Store(address, 1, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.IStore2:
                    case JavaOpCode.LStore2:
                    case JavaOpCode.FStore2:
                    case JavaOpCode.DStore2:
                    case JavaOpCode.AStore2:
                        Store(address, 2, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.IStore3:
                    case JavaOpCode.LStore3:
                    case JavaOpCode.FStore3:
                    case JavaOpCode.DStore3:
                    case JavaOpCode.AStore3:
                        Store(address, 3, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.IStore:
                    case JavaOpCode.LStore:
                    case JavaOpCode.FStore:
                    case JavaOpCode.DStore:
                    case JavaOpCode.AStore:
                        Store(address, code[++i], isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.IConstM1:
                        Emit(address, OpCodes.Ldc_I4_M1);
                        break;
                    case JavaOpCode.IConst0:
                        Emit(address, OpCodes.Ldc_I4_0);
                        break;
                    case JavaOpCode.IConst1:
                        Emit(address, OpCodes.Ldc_I4_1);
                        break;
                    case JavaOpCode.IConst2:
                        Emit(address, OpCodes.Ldc_I4_2);
                        break;
                    case JavaOpCode.IConst3:
                        Emit(address, OpCodes.Ldc_I4_3);
                        break;
                    case JavaOpCode.IConst4:
                        Emit(address, OpCodes.Ldc_I4_4);
                        break;
                    case JavaOpCode.IConst5:
                        Emit(address, OpCodes.Ldc_I4_5);
                        break;
                    case JavaOpCode.LConst0:
                        Emit(address, OpCodes.Ldc_I8, 0L);
                        break;
                    case JavaOpCode.LConst1:
                        Emit(address, OpCodes.Ldc_I8, 1L);
                        break;
                    case JavaOpCode.FConst0:
                        Emit(address, OpCodes.Ldc_R4, 0f);
                        break;
                    case JavaOpCode.FConst1:
                        Emit(address, OpCodes.Ldc_R4, 1f);
                        break;
                    case JavaOpCode.FConst2:
                        Emit(address, OpCodes.Ldc_R4, 2f);
                        break;
                    case JavaOpCode.DConst0:
                        Emit(address, OpCodes.Ldc_R8, 0d);
                        break;
                    case JavaOpCode.DConst1:
                        Emit(address, OpCodes.Ldc_R8, 1d);
                        break;
                    case JavaOpCode.AConstNull:
                        Emit(address, OpCodes.Ldnull);
                        break;
                    case JavaOpCode.BiPush:
                        Emit(address, OpCodes.Ldc_I4, (int)code[++i]);
                        break;
                    case JavaOpCode.SiPush:
                        Emit(address, OpCodes.Ldc_I4, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.Ldc:
                        Ldc(address, jc, code[++i]);
                        break;
                    case JavaOpCode.LdcW:
                    case JavaOpCode.Ldc2W:
                        Ldc(address, jc, Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IInc:
                        int incrementIndex = code[++i];
                        Load(address, incrementIndex, isStatic, parametersTypes, op);
                        Emit(address, OpCodes.Ldc_I4, code[++i]);
                        Emit(address, OpCodes.Add);
                        Store(address, incrementIndex, isStatic, parametersTypes, op);
                        break;
                    case JavaOpCode.IAdd:
                    case JavaOpCode.LAdd:
                    case JavaOpCode.FAdd:
                    case JavaOpCode.DAdd:
                        Emit(address, OpCodes.Add);
                        break;
                    case JavaOpCode.ISub:
                    case JavaOpCode.LSub:
                    case JavaOpCode.FSub:
                    case JavaOpCode.DSub:
                        Emit(address, OpCodes.Sub);
                        break;
                    case JavaOpCode.IMul:
                    case JavaOpCode.LMul:
                    case JavaOpCode.FMul:
                    case JavaOpCode.DMul:
                        Emit(address, OpCodes.Mul);
                        break;
                    case JavaOpCode.IDiv:
                    case JavaOpCode.LDiv:
                    case JavaOpCode.FDiv:
                    case JavaOpCode.DDiv:
                        Emit(address, OpCodes.Div);
                        break;
                    case JavaOpCode.IRem:
                    case JavaOpCode.LRem:
                    case JavaOpCode.FRem:
                    case JavaOpCode.DRem:
                        Emit(address, OpCodes.Rem);
                        break;
                    case JavaOpCode.IShL:
                    case JavaOpCode.LShL:
                        Emit(address, OpCodes.Shl);
                        break;
                    case JavaOpCode.IShR:
                    case JavaOpCode.LShR:
                        Emit(address, OpCodes.Shr);
                        break;
                    case JavaOpCode.IuShR:
                    case JavaOpCode.LuShR:
                        Emit(address, OpCodes.Shr_Un);
                        break;
                    case JavaOpCode.IXor:
                    case JavaOpCode.LXor:
                        Emit(address, OpCodes.Xor);
                        break;
                    case JavaOpCode.IOr:
                    case JavaOpCode.LOr:
                        Emit(address, OpCodes.Or);
                        break;
                    case JavaOpCode.IAnd:
                    case JavaOpCode.LAnd:
                        Emit(address, OpCodes.And);
                        break;
                    case JavaOpCode.INeg:
                    case JavaOpCode.LNeg:
                        Emit(address, OpCodes.Neg);
                        break;
                    case JavaOpCode.I2b:
                        Emit(address, OpCodes.Conv_U1);
                        break;
                    case JavaOpCode.I2c:
                        Emit(address, OpCodes.Conv_U2);
                        break;
                    case JavaOpCode.I2s:
                        Emit(address, OpCodes.Conv_I2);
                        break;
                    case JavaOpCode.L2i:
                    case JavaOpCode.F2i:
                    case JavaOpCode.D2i:
                        Emit(address, OpCodes.Conv_I4);
                        break;
                    case JavaOpCode.I2l:
                    case JavaOpCode.F2l:
                    case JavaOpCode.D2l:
                        Emit(address, OpCodes.Conv_I8);
                        break;
                    case JavaOpCode.I2f:
                    case JavaOpCode.L2f:
                    case JavaOpCode.D2f:
                        Emit(address, OpCodes.Conv_R4);
                        break;
                    case JavaOpCode.I2d:
                    case JavaOpCode.L2d:
                    case JavaOpCode.F2d:
                        Emit(address, OpCodes.Conv_R8);
                        break;
                    case JavaOpCode.Dup:
                        Emit(address, OpCodes.Dup);
                        break;
                    case JavaOpCode.InvokeVirtual:
                    case JavaOpCode.InvokeSpecial:
                        Call(address, jc, Combine(code[++i], code[++i]), false);
                        break;
                    case JavaOpCode.InvokeInterface:
                        Call(address, jc, Combine(code[++i], code[++i]), false);
                        i += 2;
                        break;
                    case JavaOpCode.InvokeDynamic:
                        InvokeDynamic(address, jc, Combine(code[++i], code[++i]));
                        i += 2;
                        break;
                    case JavaOpCode.InvokeStatic:
                        Call(address, jc, Combine(code[++i], code[++i]), true);
                        break;
                    case JavaOpCode.Goto:
                        EmitJump(address, OpCodes.Br, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.GotoW:
                        EmitJump(address, OpCodes.Br, (short)Combine(code[++i], code[++i], code[++i], code[++i]));
                        break;
                    case JavaOpCode.New:
                        Emit(address, OpCodes.Ldtoken, module.GetJavaType(jc, Combine(code[++i], code[++i])));
                        Emit(address, OpCodes.Call, module.GetMethod(typeof(Type), "GetTypeFromHandle"));
                        Emit(address, OpCodes.Call, module.GetMethod(typeof(FormatterServices), "GetUninitializedObject"));
                        break;
                    case JavaOpCode.Pop:
                        Emit(address, OpCodes.Pop);
                        break;
                    case JavaOpCode.Pop2:
                        Emit(address, OpCodes.Pop);
                        Emit(address, OpCodes.Pop);
                        break;
                    case JavaOpCode.AThrow:
                        Emit(address, OpCodes.Throw);
                        break;
                    case JavaOpCode.IfICmpEq:
                    case JavaOpCode.IfACmpEq:
                        EmitJump(address, OpCodes.Beq, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpNe:
                    case JavaOpCode.IfACmpNe:
                        EmitJump(address, OpCodes.Bne_Un, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpLt:
                        EmitJump(address, OpCodes.Blt, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpLe:
                        EmitJump(address, OpCodes.Ble, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpGt:
                        EmitJump(address, OpCodes.Bgt, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfICmpGe:
                        EmitJump(address, OpCodes.Bge, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfNull:
                    case JavaOpCode.IfEq:
                        EmitJump(address, OpCodes.Brfalse, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfNonNull:
                    case JavaOpCode.IfNe:
                        EmitJump(address, OpCodes.Brtrue, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfLt:
                        Emit(address, OpCodes.Ldc_I4_0);
                        EmitJump(address, OpCodes.Blt, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfLe:
                        Emit(address, OpCodes.Ldc_I4_0);
                        EmitJump(address, OpCodes.Ble, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfGt:
                        Emit(address, OpCodes.Ldc_I4_0);
                        EmitJump(address, OpCodes.Bgt, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.IfGe:
                        Emit(address, OpCodes.Ldc_I4_0);
                        EmitJump(address, OpCodes.Bge, (short)Combine(code[++i], code[++i]));
                        break;
                    case JavaOpCode.ArrayLength:
                        Emit(address, OpCodes.Ldlen);
                        break;
                    case JavaOpCode.GetField:
                        GetField(address, jc, Combine(code[++i], code[++i]), false);
                        break;
                    case JavaOpCode.GetStatic:
                        GetField(address, jc, Combine(code[++i], code[++i]), true);
                        break;
                    case JavaOpCode.PutField:
                        SetField(address, jc, Combine(code[++i], code[++i]), false);
                        break;
                    case JavaOpCode.PutStatic:
                        SetField(address, jc, Combine(code[++i], code[++i]), true);
                        break;
                    case JavaOpCode.NewArray:
                        Emit(address, OpCodes.Newarr, GetArrayType((PrimitiveArrayType)code[++i]));
                        break;
                    case JavaOpCode.ANewArray:
                        Emit(address, OpCodes.Newarr, module.GetJavaType(jc, Combine(code[++i], code[++i])));
                        break;
                    case JavaOpCode.BAStore:
                        Emit(address, OpCodes.Stelem_I1);
                        break;
                    case JavaOpCode.CAStore:
                    case JavaOpCode.SAStore:
                        Emit(address, OpCodes.Stelem_I2);
                        break;
                    case JavaOpCode.IAStore:
                        Emit(address, OpCodes.Stelem_I4);
                        break;
                    case JavaOpCode.LAStore:
                        Emit(address, OpCodes.Stelem_I8);
                        break;
                    case JavaOpCode.FAStore:
                        Emit(address, OpCodes.Stelem_R4);
                        break;
                    case JavaOpCode.DAStore:
                        Emit(address, OpCodes.Stelem_R8);
                        break;
                    case JavaOpCode.AAStore:
                        Emit(address, OpCodes.Stelem_Ref);
                        break;
                    case JavaOpCode.BALoad:
                        Emit(address, OpCodes.Ldelem_U1);
                        break;
                    case JavaOpCode.CALoad:
                        Emit(address, OpCodes.Ldelem_U2);
                        break;
                    case JavaOpCode.SALoad:
                        Emit(address, OpCodes.Ldelem_I2);
                        break;
                    case JavaOpCode.IALoad:
                        Emit(address, OpCodes.Ldelem_I4);
                        break;
                    case JavaOpCode.LALoad:
                        Emit(address, OpCodes.Ldelem_I8);
                        break;
                    case JavaOpCode.FALoad:
                        Emit(address, OpCodes.Ldelem_R4);
                        break;
                    case JavaOpCode.DALoad:
                        Emit(address, OpCodes.Ldelem_R8);
                        break;
                    case JavaOpCode.AALoad:
                        Emit(address, OpCodes.Ldelem_Ref);
                        break;
                    case JavaOpCode.CheckCast:
                        Emit(address, OpCodes.Castclass, module.GetJavaType(jc, Combine(code[++i], code[++i])));
                        break;
                    case JavaOpCode.InstanceOf:
                        Emit(address, OpCodes.Isinst, module.GetJavaType(jc, Combine(code[++i], code[++i])));
                        break;
                    default:
                        missing.Add(op);
                        //throw new Exception($"Unknown opcode '{op}'.");
                        return;
                }
            }

            AddLocals();
            SetupJumps();
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

        private void AddLocals()
        {
            int i = 0;
            foreach (KeyValuePair<int, VariableDefinition> pair in locals.OrderBy(x => x.Key))
            {
                for (; i < pair.Key; i++)
                {
                    il.Body.Variables.Add(new VariableDefinition(module.TypeSystem.Object));
                }

                il.Body.Variables.Add(pair.Value);
                i++;
            }
        }

        private void SetupJumps()
        {
            foreach (var jump in jumps)
            {
                Instruction targetInstruction = first[jump.TargetAddress];
                Instruction newInstruction = il.Create(jump.OpCode, targetInstruction);
                il.Replace(jump.Instruction, newInstruction);
            }
        }

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

        private void Emit(int instructionIndex, OpCode opCode, VariableDefinition arg)
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

        private void Load(int instructionIndex, int index, bool isStatic, string[] parametersTypes, JavaOpCode op)
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
                if (!locals.TryGetValue(index, out VariableDefinition var))
                {
                    var = new VariableDefinition(GetTargetType(op));
                    locals.Add(index, var);
                }

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
                        Emit(instructionIndex, OpCodes.Ldloc, var);
                        return;
                }
            }
        }

        private void Store(int instructionIndex, int index, bool isStatic, string[] parametersTypes, JavaOpCode op)
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
                if (!locals.TryGetValue(index, out VariableDefinition var))
                {
                    var = new VariableDefinition(GetTargetType(op));
                    locals.Add(index, var);
                }

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
                        Emit(instructionIndex, OpCodes.Stloc, var);
                        return;
                }
            }
        }

        private TypeReference GetTargetType(JavaOpCode op)
        {
            switch (op)
            {
                case JavaOpCode.IInc:
                case JavaOpCode.IStore0:
                case JavaOpCode.IStore1:
                case JavaOpCode.IStore2:
                case JavaOpCode.IStore3:
                case JavaOpCode.IStore:
                case JavaOpCode.ILoad0:
                case JavaOpCode.ILoad1:
                case JavaOpCode.ILoad2:
                case JavaOpCode.ILoad3:
                case JavaOpCode.ILoad:
                    return module.TypeSystem.Int32;
                case JavaOpCode.LStore0:
                case JavaOpCode.LStore1:
                case JavaOpCode.LStore2:
                case JavaOpCode.LStore3:
                case JavaOpCode.LStore:
                case JavaOpCode.LLoad0:
                case JavaOpCode.LLoad1:
                case JavaOpCode.LLoad2:
                case JavaOpCode.LLoad3:
                case JavaOpCode.LLoad:
                    return module.TypeSystem.Int64;
                case JavaOpCode.FStore0:
                case JavaOpCode.FStore1:
                case JavaOpCode.FStore2:
                case JavaOpCode.FStore3:
                case JavaOpCode.FStore:
                case JavaOpCode.FLoad0:
                case JavaOpCode.FLoad1:
                case JavaOpCode.FLoad2:
                case JavaOpCode.FLoad3:
                case JavaOpCode.FLoad:
                    return module.TypeSystem.Single;
                case JavaOpCode.DStore0:
                case JavaOpCode.DStore1:
                case JavaOpCode.DStore2:
                case JavaOpCode.DStore3:
                case JavaOpCode.DStore:
                case JavaOpCode.DLoad0:
                case JavaOpCode.DLoad1:
                case JavaOpCode.DLoad2:
                case JavaOpCode.DLoad3:
                case JavaOpCode.DLoad:
                    return module.TypeSystem.Double;
                case JavaOpCode.AStore0:
                case JavaOpCode.AStore1:
                case JavaOpCode.AStore2:
                case JavaOpCode.AStore3:
                case JavaOpCode.AStore:
                case JavaOpCode.ALoad0:
                case JavaOpCode.ALoad1:
                case JavaOpCode.ALoad2:
                case JavaOpCode.ALoad3:
                case JavaOpCode.ALoad:
                    return module.TypeSystem.Object;
                default:
                    throw new ArgumentException($"Could not find target type for opcode '{op}'.");
            }
        }

        private TypeReference GetArrayType(PrimitiveArrayType type)
        {
            switch (type)
            {
                case PrimitiveArrayType.Boolean:
                    return module.TypeSystem.Boolean;
                case PrimitiveArrayType.Byte:
                    return module.TypeSystem.Byte;
                case PrimitiveArrayType.Char:
                    return module.TypeSystem.Char;
                case PrimitiveArrayType.Double:
                    return module.TypeSystem.Double;
                case PrimitiveArrayType.Float:
                    return module.TypeSystem.Single;
                case PrimitiveArrayType.Int:
                    return module.TypeSystem.Int32;
                case PrimitiveArrayType.Long:
                    return module.TypeSystem.Int64;
                case PrimitiveArrayType.Short:
                    return module.TypeSystem.Int16;
                default:
                    throw new ArgumentException($"Unknown primitive array type: {type}");
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
            TypeDefinition objectType = module.GetJavaTypeDefinition(objectTypeName);
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

        private FieldDefinition FindField(JavaClass jc, ushort index)
        {
            JavaConstantFieldReference constant = jc.GetConstant<JavaConstantFieldReference>(index);
            JavaConstantNameAndType nameAndType = jc.GetConstant<JavaConstantNameAndType>(constant.NameAndTypeIndex);
            string objectTypeName = jc.GetConstant<JavaConstantUtf8>(jc.GetConstant<JavaConstantClass>(constant.ClassIndex).NameIndex).Value;
            TypeDefinition objectType = module.GetJavaTypeDefinition(objectTypeName);
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
