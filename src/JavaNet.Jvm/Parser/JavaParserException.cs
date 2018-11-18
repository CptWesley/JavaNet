using System;

namespace JavaNet.Jvm.Parser
{
    /// <summary>
    /// Exception thrown when something goes wrong wile parsing.
    /// </summary>
    /// <seealso cref="Exception" />
    public class JavaParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaParserException"/> class.
        /// </summary>
        public JavaParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public JavaParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaParserException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public JavaParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
