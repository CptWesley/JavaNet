using JavaNet.Jvm.Parser.Methods;
using Xunit;
using static AssertNet.Xunit.Assertions;

namespace JavaNet.Jvm.Tests.Parser.Methods
{
    /// <summary>
    /// Test class for the <see cref="JavaMethodAccessFlags"/> class.
    /// </summary>
    public static class JavaMethodAccessFlagsTests
    {
        /// <summary>
        /// Checks that all values are correct.
        /// </summary>
        [Fact]
        public static void ValueTest()
        {
            AssertThat((int)JavaMethodAccessFlags.Public).IsEqualTo(0x0001);
            AssertThat((int)JavaMethodAccessFlags.Private).IsEqualTo(0x0002);
            AssertThat((int)JavaMethodAccessFlags.Protected).IsEqualTo(0x0004);
            AssertThat((int)JavaMethodAccessFlags.Static).IsEqualTo(0x0008);
            AssertThat((int)JavaMethodAccessFlags.Final).IsEqualTo(0x0010);
            AssertThat((int)JavaMethodAccessFlags.Synchronized).IsEqualTo(0x0020);
            AssertThat((int)JavaMethodAccessFlags.Bridge).IsEqualTo(0x0040);
            AssertThat((int)JavaMethodAccessFlags.Synthetic).IsEqualTo(0x1000);
            AssertThat((int)JavaMethodAccessFlags.VariableArguments).IsEqualTo(0x0080);
            AssertThat((int)JavaMethodAccessFlags.Native).IsEqualTo(0x0100);
            AssertThat((int)JavaMethodAccessFlags.Strict).IsEqualTo(0x0800);
        }
    }
}
