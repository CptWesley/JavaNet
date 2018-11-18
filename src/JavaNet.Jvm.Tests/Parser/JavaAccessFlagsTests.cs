using JavaNet.Jvm.Parser;
using Xunit;
using static AssertNet.Xunit.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="JavaAccessFlags"/> class.
    /// </summary>
    public static class JavaAccessFlagsTests
    {
        /// <summary>
        /// Checks that all values are correct.
        /// </summary>
        [Fact]
        public static void ValueTest()
        {
            AssertThat((int)JavaAccessFlags.Public).IsEqualTo(0x0001);
            AssertThat((int)JavaAccessFlags.Final).IsEqualTo(0x0010);
            AssertThat((int)JavaAccessFlags.Super).IsEqualTo(0x0020);
            AssertThat((int)JavaAccessFlags.Interface).IsEqualTo(0x0200);
            AssertThat((int)JavaAccessFlags.Abstract).IsEqualTo(0x0400);
            AssertThat((int)JavaAccessFlags.Synthetic).IsEqualTo(0x1000);
            AssertThat((int)JavaAccessFlags.Annotation).IsEqualTo(0x2000);
            AssertThat((int)JavaAccessFlags.Enum).IsEqualTo(0x4000);
        }
    }
}
