using JavaNet.Jvm.Parser.Constants;
using Xunit;
using static AssertNet.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="ConstantPoolTag"/> class.
    /// </summary>
    public static class ConstantPoolTagTests
    {
        /// <summary>
        /// Checks that all values are correct.
        /// </summary>
        [Fact]
        public static void ValueTest()
        {
            AssertThat((int)ConstantPoolTag.Utf8).IsEqualTo(1);
            AssertThat((int)ConstantPoolTag.Integer).IsEqualTo(3);
            AssertThat((int)ConstantPoolTag.Float).IsEqualTo(4);
            AssertThat((int)ConstantPoolTag.Long).IsEqualTo(5);
            AssertThat((int)ConstantPoolTag.Double).IsEqualTo(6);
            AssertThat((int)ConstantPoolTag.Class).IsEqualTo(7);
            AssertThat((int)ConstantPoolTag.String).IsEqualTo(8);
            AssertThat((int)ConstantPoolTag.FieldReference).IsEqualTo(9);
            AssertThat((int)ConstantPoolTag.MethodReference).IsEqualTo(10);
            AssertThat((int)ConstantPoolTag.InterfaceMethodReference).IsEqualTo(11);
            AssertThat((int)ConstantPoolTag.NameAndType).IsEqualTo(12);
            AssertThat((int)ConstantPoolTag.MethodHandle).IsEqualTo(15);
            AssertThat((int)ConstantPoolTag.MethodType).IsEqualTo(16);
            AssertThat((int)ConstantPoolTag.InvokeDynamic).IsEqualTo(18);
        }
    }
}
