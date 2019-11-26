using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Methods;
using Xunit;
using static AssertNet.Assertions;

namespace JavaNet.Jvm.Tests.Parser.Methods
{
    /// <summary>
    /// Test class for the <see cref="JavaMethod" /> class
    /// </summary>
    public static class JavaMethodTests
    {
        /// <summary>
        /// Constructors the test.
        /// </summary>
        [Fact]
        public static void ConstructorTest()
        {
            JavaMethodAccessFlags flags = JavaMethodAccessFlags.Native | JavaMethodAccessFlags.Private;
            ushort us1 = 1;
            ushort us2 = 2;
            ushort us3 = 3;
            IJavaAttribute[] attributes = new IJavaAttribute[2];
            JavaMethod jm = new JavaMethod(flags, us1, us2, us3, attributes);
            AssertThat(jm.AccessFlags).IsEqualTo(flags);
            AssertThat(jm.NameIndex).IsEqualTo(us1);
            AssertThat(jm.DescriptorIndex).IsEqualTo(us2);
            AssertThat(jm.AttributesCount).IsEqualTo(us3);
            AssertThat(jm.Attributes).ContainsExactly(attributes);
        }
    }
}
