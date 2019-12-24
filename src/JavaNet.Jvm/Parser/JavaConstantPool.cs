using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JavaNet.Jvm.Parser.Constants;

namespace JavaNet.Jvm.Parser
{
    /// <summary>
    /// Class mimicking the 1-indexed constant pool in class files.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification = "Mimicks JVM specification name more accurately.")]
    public class JavaConstantPool : IEnumerable<IJavaConstant>
    {
        private readonly IJavaConstant[] constants;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantPool"/> class.
        /// </summary>
        /// <param name="constants">The constants to initialize it with.</param>
        public JavaConstantPool(IJavaConstant[] constants)
        {
            this.constants = constants;
        }

        /// <summary>
        /// Gets the <see cref="IJavaConstant"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="IJavaConstant"/>.
        /// </value>
        /// <param name="key">The index.</param>
        /// <returns>The <see cref="IJavaConstant"/> at the specified index.</returns>
        public IJavaConstant this[int key] => constants[key - 1];

        /// <inheritdoc/>
        public IEnumerator<IJavaConstant> GetEnumerator()
            => ((IEnumerable<IJavaConstant>)constants).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
