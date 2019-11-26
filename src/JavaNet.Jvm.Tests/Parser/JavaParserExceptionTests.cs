using System;
using JavaNet.Jvm.Parser;
using Xunit;
using static AssertNet.Assertions;

namespace JavaNet.Jvm.Tests.Parser
{
    /// <summary>
    /// Test class for the <see cref="JavaParserException"/> class.
    /// </summary>
    public static class JavaParserExceptionTests
    {
        /// <summary>
        /// Checks that the empty constructor works.
        /// </summary>
        [Fact]
        public static void NormalConstructorTest()
        {
            AssertThat(() => throw new JavaParserException())
                .ThrowsExactlyException<JavaParserException>()
                .WithNoInnerException();
        }

        /// <summary>
        /// Checks that the constructor with a message works.
        /// </summary>
        [Fact]
        public static void MessageConstructorTest()
        {
            AssertThat(() => throw new JavaParserException("hello world"))
                .ThrowsExactlyException<JavaParserException>()
                .WithMessage("hello world")
                .WithNoInnerException();
        }

        /// <summary>
        /// Checks that the constructor with an inner exception works.
        /// </summary>
        [Fact]
        public static void InnerExceptionConstructorTest()
        {
            AssertThat(() => throw new JavaParserException("hello world", new Exception()))
                .ThrowsExactlyException<JavaParserException>()
                .WithMessage("hello world")
                .WithInnerException<Exception>();
        }
    }
}
