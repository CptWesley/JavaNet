using System;
using System.IO;
using System.Reflection;
using System.Text;
using CoreResourceManager;
using JavaNet.Jvm.Converter;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Instructions;
using JavaNet.Jvm.Parser.Methods;
using Xunit;

namespace JavaNet.Jvm.Tests
{
    /// <summary>
    /// Contains smoke tests.
    /// </summary>
    public class SmokeTest
    {
        /// <summary>
        /// Checks that we can parse and execute the hello world class correctly.
        /// </summary>
        [Fact]
        public void HelloWorld()
        {
            using Stream stream = Resource.Get("HelloWorld.class");
            JavaClass jc = JavaClass.Create(stream);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"class {jc.GetPackageName()}.{jc.GetName()} : {jc.SuperClassIndex}");
            foreach (JavaMethod method in jc.Methods)
            {
                sb.AppendLine();
                sb.AppendLine($"method {((JavaConstantUtf8)jc.ConstantPool[method.NameIndex]).Value}{((JavaConstantUtf8)jc.ConstantPool[method.DescriptorIndex]).Value}");
                byte[] code = GetCode(method).Code;
                for (int i = 0; i < code.Length; i++)
                {
                    JavaOpCode op = (JavaOpCode)code[i];
                    string printed = op.ToString();

                    switch (op)
                    {
                        case JavaOpCode.GetStatic:
                            JavaConstantFieldReference fieldRef = (JavaConstantFieldReference)jc.ConstantPool[GetIndex(code[++i], code[++i])];
                            JavaConstantClass c = (JavaConstantClass)jc.ConstantPool[fieldRef.ClassIndex];
                            JavaConstantUtf8 className = (JavaConstantUtf8)jc.ConstantPool[c.NameIndex];
                            JavaConstantNameAndType nameAndType = (JavaConstantNameAndType)jc.ConstantPool[fieldRef.NameAndTypeIndex];
                            JavaConstantUtf8 fieldName = (JavaConstantUtf8)jc.ConstantPool[nameAndType.NameIndex];
                            JavaConstantUtf8 descriptor = (JavaConstantUtf8)jc.ConstantPool[nameAndType.DescriptorIndex];
                            printed = $"{printed} {className.Value}/{fieldName.Value} {descriptor.Value}";
                            break;
                        case JavaOpCode.InvokeVirtual:
                        case JavaOpCode.InvokeSpecial:
                            JavaConstantMethodReference methodRef = (JavaConstantMethodReference)jc.ConstantPool[GetIndex(code[++i], code[++i])];
                            JavaConstantClass c2 = (JavaConstantClass)jc.ConstantPool[methodRef.ClassIndex];
                            JavaConstantUtf8 className2 = (JavaConstantUtf8)jc.ConstantPool[c2.NameIndex];
                            JavaConstantNameAndType nameAndType2 = (JavaConstantNameAndType)jc.ConstantPool[methodRef.NameAndTypeIndex];
                            JavaConstantUtf8 methodName = (JavaConstantUtf8)jc.ConstantPool[nameAndType2.NameIndex];
                            JavaConstantUtf8 descriptor2 = (JavaConstantUtf8)jc.ConstantPool[nameAndType2.DescriptorIndex];
                            printed = $"{printed} {className2.Value}/{methodName.Value}{descriptor2.Value}";
                            break;
                        case JavaOpCode.BiPush:
                            printed = $"{printed} {code[++i]}";
                            break;
                        case JavaOpCode.Ldc:
                            JavaConstantString str = (JavaConstantString)jc.ConstantPool[code[++i]];
                            JavaConstantUtf8 strVal = (JavaConstantUtf8)jc.ConstantPool[str.StringIndex];
                            printed = $"{printed} \"{strVal.Value}\"";
                            break;
                    }

                    sb.AppendLine(printed);
                }
            }

            throw new Exception(sb.ToString());
        }

        private int GetIndex(byte i1, byte i2)
            => (i1 << 8) | i2;

        private JavaAttributeCode GetCode(JavaMethod method)
        {
            foreach (IJavaAttribute attribute in method.Attributes)
            {
                if (attribute is JavaAttributeCode code)
                {
                    return code;
                }
            }

            throw new Exception("No code attribute found.");
        }

        [Fact]
        public void Loading()
        {
            AssemblyConverter converter = new AssemblyConverter("HelloWorld");

            foreach (string file in Resource.GetNames("MultipleTypes"))
            {
                using Stream stream = Resource.Get(file);
                JavaClass jc = JavaClass.Create(stream);
                converter.Include(jc);
            }

            byte[] bytes = converter.Convert();
            Assembly assembly = Assembly.Load(bytes);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (Type t in assembly.GetTypes())
            {
                sb.AppendLine($"Type: {t.FullName}");
            }

            throw new Exception(sb.ToString());
        }
    }
}
