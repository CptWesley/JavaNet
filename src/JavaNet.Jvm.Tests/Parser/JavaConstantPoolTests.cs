using System;
using JavaNet.Jvm.Parser;
using JavaNet.Jvm.Parser.Constants;
using Xunit;
using static AssertNet.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="JavaConstantPool"/> class.
    /// </summary>
    public class JavaConstantPoolTests
    {
        private readonly JavaConstantPool _jcp;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantPoolTests"/> class.
        /// </summary>
        public JavaConstantPoolTests()
        {
            _jcp = new JavaConstantPool(new IJavaConstant[1]);
        }

        /// <summary>
        /// Checks that we can't access index 0.
        /// </summary>
        [Fact]
        public void TryGetZero()
        {
            AssertThat(() => { IJavaConstant c = _jcp[0]; }).ThrowsExactlyException<IndexOutOfRangeException>();
        }

        /// <summary>
        /// Checks that we can't access index 2.
        /// </summary>
        [Fact]
        public void TryGetTwo()
        {
            AssertThat(() => { IJavaConstant c = _jcp[2]; }).ThrowsExactlyException<IndexOutOfRangeException>();
        }

        /// <summary>
        /// Checks that we can access index 1.
        /// </summary>
        [Fact]
        public void TryGetOne()
        {
            AssertThat(() => { IJavaConstant c = _jcp[1]; }).DoesNotThrowException();
            AssertThat(_jcp[1]).IsNull();
        }
    }
}
