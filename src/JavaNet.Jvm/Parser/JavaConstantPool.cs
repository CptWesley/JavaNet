using JavaNet.Jvm.Parser.Constants;

namespace JavaNet.Jvm.Parser
{
    /// <summary>
    /// Class mimicking the 1-indexed constant pool in class files.
    /// </summary>
    public class JavaConstantPool
    {
        private readonly IJavaConstant[] _constants;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantPool"/> class.
        /// </summary>
        /// <param name="constants">The constants to initialize it with.</param>
        public JavaConstantPool(IJavaConstant[] constants)
        {
            _constants = constants;
        }

        /// <summary>
        /// Gets the <see cref="IJavaConstant"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="IJavaConstant"/>.
        /// </value>
        /// <param name="key">The index.</param>
        /// <returns>The <see cref="IJavaConstant"/> at the specified index.</returns>
        public IJavaConstant this[int key] => _constants[key - 1];
    }
}
