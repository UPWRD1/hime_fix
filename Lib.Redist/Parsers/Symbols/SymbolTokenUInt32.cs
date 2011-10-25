﻿/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an unsigned 32-bit integer matched by a lexer
    /// </summary>
    public sealed class SymbolTokenUInt32 : SymbolToken
    {
        private uint value;
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return value; } }
        /// <summary>
        /// Get the binary data represented by this token
        /// </summary>
        public uint ValueUInt32 { get { return value; } }
        /// <summary>
        /// Initializes a new instance of the SymbolTokenUInt32 class
        /// </summary>
        /// <param name="ClassName">Token's class name</param>
        /// <param name="ClassSID">Token's class ID</param>
        /// <param name="Value">Token binary value</param>
        public SymbolTokenUInt32(string ClassName, ushort ClassSID, uint Value) : base(ClassSID, ClassName) { value = Value; }
    }
}