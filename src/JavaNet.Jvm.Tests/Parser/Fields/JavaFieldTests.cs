using JavaNet.Jvm.Parser.Attributes;
using JavaNet.Jvm.Parser.Fields;
using Xunit;
using static AssertNet.Assertions;

namespace JavaNet.Jvm.Tests.Parser.Fields
{
    /// <summary>
    /// Test class for the <see cref="JavaField" /> class
    /// </summary>
    public static class JavaFieldTests
    {
        /// <summary>
        /// Constructors the test.
        /// </summary>
        [Fact]
        public static void ConstructorTest()
        {
            JavaFieldAccessFlags flags = JavaFieldAccessFlags.Enum | JavaFieldAccessFlags.Private;
            ushort us1 = 1;
            ushort us2 = 2;
            ushort us3 = 3;
            IJavaAttribute[] attributes = new IJavaAttribute[2];
            JavaField jf = new JavaField(flags, us1, us2, us3, attributes);
            AssertThat(jf.AccessFlags).IsEqualTo(flags);
            AssertThat(jf.NameIndex).IsEqualTo(us1);
            AssertThat(jf.DescriptorIndex).IsEqualTo(us2);
            AssertThat(jf.AttributesCount).IsEqualTo(us3);
            AssertThat(jf.Attributes).ContainsExactly(attributes);
        }
    }
}
