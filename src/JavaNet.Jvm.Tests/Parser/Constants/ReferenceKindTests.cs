using JavaNet.Jvm.Parser.Constants;
using Xunit;
using static AssertNet.Xunit.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="ReferenceKind"/> class.
    /// </summary>
    public static class ReferenceKindTests
    {
        /// <summary>
        /// Checks that all values are correct.
        /// </summary>
        [Fact]
        public static void ValueTest()
        {
            AssertThat((int)ReferenceKind.GetField).IsEqualTo(1);
            AssertThat((int)ReferenceKind.GetStatic).IsEqualTo(2);
            AssertThat((int)ReferenceKind.PutField).IsEqualTo(3);
            AssertThat((int)ReferenceKind.PutStatic).IsEqualTo(4);
            AssertThat((int)ReferenceKind.InvokeVirtual).IsEqualTo(5);
            AssertThat((int)ReferenceKind.InvokeStatic).IsEqualTo(6);
            AssertThat((int)ReferenceKind.InvokeSpecial).IsEqualTo(7);
            AssertThat((int)ReferenceKind.NewInvokeSpecial).IsEqualTo(8);
            AssertThat((int)ReferenceKind.InvokeInterface).IsEqualTo(9);
        }
    }
}
