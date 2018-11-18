using System.IO;
using CoreResourceManager;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Constants;
using Xunit;
using static AssertNet.Xunit.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="JavaClass"/> class.
    /// </summary>
    public static class JavaClassTests
    {
        /// <summary>
        /// Checks that the hello world class is parsed correctly.
        /// </summary>
        [Fact]
        public static void HelloWorldTest()
        {
            using (Stream stream = Resource.Get("HelloWorld.class"))
            {
                JavaClass jc = JavaClass.Create(stream);
                AssertThat(stream.Position).IsEqualTo(stream.Length);
                AssertThat(jc.Magic).IsEqualTo(0xCAFEBABE);
                AssertThat(jc.AccessFlags).IsEqualTo(JavaClassAccessFlags.Public | JavaClassAccessFlags.Super);
                AssertThat(jc.FieldsCount).IsEqualTo(0);
                AssertThat(jc.InterfacesCount).IsEqualTo(0);
                AssertThat(((JavaConstantUtf8)jc.ConstantPool[((JavaConstantClass)jc.ConstantPool[jc.ThisClassIndex]).NameIndex]).Value)
                    .IsEqualTo("HelloWorld");
                AssertThat(((JavaConstantUtf8)jc.ConstantPool[((JavaConstantClass)jc.ConstantPool[jc.SuperClassIndex]).NameIndex]).Value)
                    .IsEqualTo("java/lang/Object");
                AssertThat(jc.MethodsCount).IsEqualTo(2);
                AssertThat(((JavaConstantUtf8)jc.ConstantPool[jc.Methods[0].NameIndex]).Value).IsEqualTo("<init>");
                AssertThat(((JavaConstantUtf8)jc.ConstantPool[jc.Methods[1].NameIndex]).Value).IsEqualTo("main");
            }
        }
    }
}
