using System;
using System.Diagnostics;

namespace JavaNet.Jvm.Util
{
    /// <summary>
    /// Static class for checking arguments.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Checks that an object is not null.
        /// </summary>
        /// <param name="value">The object.</param>
        /// <param name="name">The name of the object.</param>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown is the object is null.</exception>
        [DebuggerHidden]
        public static void NotNull<T>(ref T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
