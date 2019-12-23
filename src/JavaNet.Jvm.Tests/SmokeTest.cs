using System;
using System.IO;
using System.Text;
using CoreResourceManager;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Constants;
using JavaNet.Jvm.Parser.Methods;
using JavaNet.Jvm.Parser.OpCodes;
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
            using (Stream stream = Resource.Get("HelloWorld.class"))
            {
                JavaClass jc = JavaClass.Create(stream);
                StringBuilder sb = new StringBuilder();
                foreach (JavaMethod method in jc.Methods)
                {
                    sb.AppendLine();
                    sb.AppendLine($"method {((JavaConstantUtf8)jc.ConstantPool[method.NameIndex]).Value}");
                    JavaAttributeCode code = GetCode(method);
                    foreach (byte b in code.Code)
                    {
                        sb.AppendLine(((JavaOpCode)b).ToString());
                    }
                }

                throw new Exception(sb.ToString());
            }
        }

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
    }
}
