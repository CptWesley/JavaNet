using System;
using System.Collections.Generic;
using System.Linq;
using JavaNet.Jvm.Parser;
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
        public void Emit(JavaClass jc, byte[] code, bool isStatic, string[] parametersTypes)
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
                        Load(0, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad1:
                    case JavaOpCode.LLoad1:
                    case JavaOpCode.FLoad1:
                    case JavaOpCode.DLoad1:
                    case JavaOpCode.ALoad1:
                        Load(1, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad2:
                    case JavaOpCode.LLoad2:
                    case JavaOpCode.FLoad2:
                    case JavaOpCode.DLoad2:
                    case JavaOpCode.ALoad2:
                        Load(2, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad3:
                    case JavaOpCode.LLoad3:
                    case JavaOpCode.FLoad3:
                    case JavaOpCode.DLoad3:
                    case JavaOpCode.ALoad3:
                        Load(3, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.ILoad:
                    case JavaOpCode.LLoad:
                    case JavaOpCode.FLoad:
                    case JavaOpCode.DLoad:
                    case JavaOpCode.ALoad:
                        Load(code[++i], isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore0:
                    case JavaOpCode.LStore0:
                    case JavaOpCode.FStore0:
                    case JavaOpCode.DStore0:
                    case JavaOpCode.AStore0:
                        Store(0, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore1:
                    case JavaOpCode.LStore1:
                    case JavaOpCode.FStore1:
                    case JavaOpCode.DStore1:
                    case JavaOpCode.AStore1:
                        Store(1, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore2:
                    case JavaOpCode.LStore2:
                    case JavaOpCode.FStore2:
                    case JavaOpCode.DStore2:
                    case JavaOpCode.AStore2:
                        Store(2, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore3:
                    case JavaOpCode.LStore3:
                    case JavaOpCode.FStore3:
                    case JavaOpCode.DStore3:
                    case JavaOpCode.AStore3:
                        Store(3, isStatic, parametersTypes);
                        break;
                    case JavaOpCode.IStore:
                    case JavaOpCode.LStore:
                    case JavaOpCode.FStore:
                    case JavaOpCode.DStore:
                    case JavaOpCode.AStore:
                        Store(code[++i], isStatic, parametersTypes);
                        break;
                    case JavaOpCode.AConstNull:
                        il.Emit(OpCodes.Ldnull);
                        break;
                    case JavaOpCode.BiPush:
                        il.Emit(OpCodes.Ldc_I4, (int)code[++i]);
                        break;
                    case JavaOpCode.Ldc:
                        Ldc(jc, code[++i]);
                        break;
                    case JavaOpCode.LdcW:
                    case JavaOpCode.Ldc2W:
                        Ldc(jc, Combine(code[++i], code[++i]));
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
                    case JavaOpCode.InvokeSpecial:
                    case JavaOpCode.InvokeVirtual:
                        CallVirt(jc, Combine(code[++i], code[++i]));
                        break;
                    default:
                        throw new Exception($"Unknown opcode '{op}'.");
                }
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

        private void Ldc(JavaClass jc, ushort index)
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

        private void Load(int index, bool isStatic, string[] parametersTypes)
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

        private void Store(int index, bool isStatic, string[] parametersTypes)
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
                il.Emit(OpCodes.Starg, (ushort)index);
            }
            else
            {
                index -= parameters;
                switch (index)
                {
                    case 0:
                        il.Emit(OpCodes.Stloc_0);
                        return;
                    case 1:
                        il.Emit(OpCodes.Stloc_1);
                        return;
                    case 2:
                        il.Emit(OpCodes.Stloc_2);
                        return;
                    case 3:
                        il.Emit(OpCodes.Stloc_3);
                        return;
                    default:
                        il.Emit(OpCodes.Stloc, (ushort)index);
                        return;
                }
            }
        }

        private void CallVirt(JavaClass jc, ushort index)
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
            TypeDefinition type = module.GetJavaType(jc.GetConstant<JavaConstantUtf8>(classNameIndex).Value);
            module.ImportReference(type.Methods.Where(x => x.FullName == $"{}");
        }
    }
}
