using JavaNet.Jvm.Parser.Fields;
using Xunit;
using static AssertNet.Xunit.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="JavaFieldAccessFlags"/> class.
    /// </summary>
    public static class JavaFieldAccessFlagsTests
    {
        /// <summary>
        /// Checks that all values are correct.
        /// </summary>
        [Fact]
        public static void ValueTest()
        {
            AssertThat((int)JavaFieldAccessFlags.Public).IsEqualTo(0x0001);
            AssertThat((int)JavaFieldAccessFlags.Private).IsEqualTo(0x0002);
            AssertThat((int)JavaFieldAccessFlags.Protected).IsEqualTo(0x0004);
            AssertThat((int)JavaFieldAccessFlags.Static).IsEqualTo(0x0008);
            AssertThat((int)JavaFieldAccessFlags.Final).IsEqualTo(0x0010);
            AssertThat((int)JavaFieldAccessFlags.Volatile).IsEqualTo(0x0040);
            AssertThat((int)JavaFieldAccessFlags.Transient).IsEqualTo(0x0080);
            AssertThat((int)JavaFieldAccessFlags.Synthetic).IsEqualTo(0x1000);
            AssertThat((int)JavaFieldAccessFlags.Enum).IsEqualTo(0x4000);
        }
    }
}
