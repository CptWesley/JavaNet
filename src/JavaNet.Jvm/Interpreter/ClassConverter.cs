using System.Collections.Generic;
using System.Globalization;
using System.Text;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Methods;
using JavaNet.Jvm.Util;

namespace JavaNet.Jvm.Interpreter
{
    /// <summary>
    /// Converts <see cref="JavaClass"/> instances to C# code.
    /// </summary>
    public class ClassConverter
    {
        /// <summary>
        /// Converts the specified java class.
        /// </summary>
        /// <param name="jc">The java class.</param>
        /// <returns>C# class string.</returns>
        public string Convert(JavaClass jc)
        {
            Guard.NotNull(ref jc, nameof(jc));

            StringBuilder sb = new StringBuilder();

            string classPath = ((JavaConstantUtf8)jc.ConstantPool[((JavaConstantClass)jc.ConstantPool[jc.ThisClassIndex]).NameIndex]).Value;
            string className = GetClassName(classPath);

            sb.Append($"namespace {GetNamespace(classPath)}{{");

            if (jc.AccessFlags.HasFlag(JavaClassAccessFlags.Public))
            {
                sb.Append("public ");
            }

            if (jc.AccessFlags.HasFlag(JavaClassAccessFlags.Abstract))
            {
                sb.Append("abstract ");
            }

            if (jc.AccessFlags.HasFlag(JavaClassAccessFlags.Final))
            {
                sb.Append("sealed ");
            }

            if (jc.AccessFlags.HasFlag(JavaClassAccessFlags.Enum))
            {
                sb.Append("enum ");
            }

            if (jc.AccessFlags.HasFlag(JavaClassAccessFlags.Interface))
            {
                sb.Append("interface ");
            }

            if (!jc.AccessFlags.HasFlag(JavaClassAccessFlags.Interface) && !jc.AccessFlags.HasFlag(JavaClassAccessFlags.Enum))
            {
                sb.Append("class ");
            }

            sb.Append(className).Append('{');

            foreach (JavaMethod method in jc.Methods)
            {
                AppendMethod(sb, jc, method);
            }

            sb.Append('}');

            return sb.Append('}').ToString();
        }

        private static string CorrectClassPath(string classPath)
            => classPath.Replace('/', '.');

        private static string GetNamespace(string classPath)
        {
            classPath = CorrectClassPath(classPath);

            int index = classPath.IndexOf('.');
            if (index < 0)
            {
                return "MissingNamespace";
            }

            return classPath.Substring(0, index);
        }

        private static string GetClassName(string classPath)
        {
            int index = classPath.IndexOf('/');
            if (index < 0)
            {
                return classPath;
            }

            return classPath.Substring(index + 1);
        }

        private void AppendMethod(StringBuilder sb, JavaClass jc, JavaMethod method)
        {
            if (method.AccessFlags.HasFlag(JavaMethodAccessFlags.Public))
            {
                sb.Append("public ");
            }
            else if (method.AccessFlags.HasFlag(JavaMethodAccessFlags.Private))
            {
                sb.Append("private ");
            }
            else if (method.AccessFlags.HasFlag(JavaMethodAccessFlags.Protected))
            {
                sb.Append("protected ");
            }

            if (method.AccessFlags.HasFlag(JavaMethodAccessFlags.Static))
            {
                sb.Append("static ");
            }
            else if (method.AccessFlags.HasFlag(JavaMethodAccessFlags.Abstract))
            {
                sb.Append("abstract ");
            }

            string descriptor = ((JavaConstantUtf8)jc.ConstantPool[method.DescriptorIndex]).Value;
            sb.Append(GetReturnType(descriptor)).Append(" ");

            sb.Append(((JavaConstantUtf8)jc.ConstantPool[method.NameIndex]).Value).Append('(');

            sb.Append(string.Join(", ", GetParameterTypes(descriptor)));

            sb.Append("){");

            sb.Append('}');
        }

        private string[] GetParameterTypes(string descriptor)
        {
            string parameters = descriptor.Substring(1, descriptor.LastIndexOf(')'));

            List<string> types = new List<string>();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == 'L')
                {
                    string temp = parameters.Substring(i);
                    temp = temp.Substring(0, temp.IndexOf(';') + 1);
                    types.Add(GetFieldType(temp));
                    i += temp.Length - 1;
                }
                else if (parameters[i] == '[')
                {
                    string temp = string.Empty;
                    while (parameters[i] == '[')
                    {
                        temp += '[';
                        i++;
                    }

                    temp += parameters[i];
                    types.Add(GetFieldType(temp));
                }
                else if (parameters[i] == ')')
                {
                    break;
                }
                else
                {
                    types.Add(GetFieldType(parameters[i].ToString(CultureInfo.InvariantCulture)));
                }
            }

            return types.ToArray();
        }

        private string GetReturnType(string descriptor)
        {
            string returnPart = descriptor.Substring(descriptor.LastIndexOf(')') + 1);
            return GetFieldType(returnPart);
        }

        private string GetFieldType(string descriptor)
        {
            switch (descriptor)
            {
                case "B":
                    return "byte";
                case "C":
                    return "char";
                case "D":
                    return "double";
                case "F":
                    return "float";
                case "I":
                    return "int";
                case "J":
                    return "long";
                case "S":
                    return "short";
                case "Z":
                    return "bool";
                case "V":
                    return "void";
                default:
                    if (descriptor[0] == '[')
                    {
                        return $"{GetFieldType(descriptor.Substring(1))}[]";
                    }

                    return descriptor.Substring(1, descriptor.Length - 2);
            }
        }
    }
}
