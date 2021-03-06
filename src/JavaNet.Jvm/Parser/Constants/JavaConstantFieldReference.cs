﻿namespace JavaNet.Jvm.Parser.Constants
{
    /// <summary>
    /// Represents field reference constants.
    /// </summary>
    /// <seealso cref="IJavaConstant" />
    public class JavaConstantFieldReference : IJavaConstant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaConstantFieldReference"/> class.
        /// </summary>
        /// <param name="classIndex">Index of the class in the constant pool.</param>
        /// <param name="nameAndTypeIndex">Index of the name and type in the constant pool.</param>
        public JavaConstantFieldReference(ushort classIndex, ushort nameAndTypeIndex)
        {
            ClassIndex = classIndex;
            NameAndTypeIndex = nameAndTypeIndex;
        }

        /// <summary>
        /// Gets the tag indicating the type.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public ConstantPoolTag Tag => ConstantPoolTag.FieldReference;

        /// <summary>
        /// Gets the index of the class in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the class in the constant pool.
        /// </value>
        public ushort ClassIndex { get; }

        /// <summary>
        /// Gets the index of the name and type in the constant pool.
        /// </summary>
        /// <value>
        /// The index of the name and type in the constant pool.
        /// </value>
        public ushort NameAndTypeIndex { get; }
    }
}
