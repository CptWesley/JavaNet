using JavaNet.Jvm.Parser;
using Xunit;
using static AssertNet.Xunit.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="JavaClassAccessFlags"/> class.
    /// </summary>
    public static class JavaAccessFlagsTests
    {
        /// <summary>
        /// Checks that all values are correct.
        /// </summary>
        [Fact]
        public static void ValueTest()
        {
            AssertThat((int)JavaClassAccessFlags.Public).IsEqualTo(0x0001);
            AssertThat((int)JavaClassAccessFlags.Final).IsEqualTo(0x0010);
            AssertThat((int)JavaClassAccessFlags.Super).IsEqualTo(0x0020);
            AssertThat((int)JavaClassAccessFlags.Interface).IsEqualTo(0x0200);
            AssertThat((int)JavaClassAccessFlags.Abstract).IsEqualTo(0x0400);
            AssertThat((int)JavaClassAccessFlags.Synthetic).IsEqualTo(0x1000);
            AssertThat((int)JavaClassAccessFlags.Annotation).IsEqualTo(0x2000);
            AssertThat((int)JavaClassAccessFlags.Enum).IsEqualTo(0x4000);
        }
    }
}
