/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 0.6.0.0
 */

using System.Collections.Generic;
using Hime.Redist;
using Hime.Redist.Lexer;

namespace Hime.CentralDogma.Input
{
    internal class FileCentralDogmaLexer : Lexer
    {
        private static readonly Automaton automaton = Automaton.Find(typeof(FileCentralDogmaLexer), "FileCentralDogmaLexer.bin");
        public static readonly Symbol[] terminals = {
            new Symbol(0x1, "ε"),
            new Symbol(0x2, "$"),
            new Symbol(0x61, "["),
            new Symbol(0xB, "INTEGER"),
            new Symbol(0x14, "="),
            new Symbol(0x15, ";"),
            new Symbol(0x16, "."),
            new Symbol(0x17, "~"),
            new Symbol(0x19, "("),
            new Symbol(0x1A, ")"),
            new Symbol(0x1B, "*"),
            new Symbol(0x1C, "+"),
            new Symbol(0x1D, "?"),
            new Symbol(0x1E, "{"),
            new Symbol(0x1F, ","),
            new Symbol(0x20, "}"),
            new Symbol(0x21, "-"),
            new Symbol(0x22, "|"),
            new Symbol(0x25, "<"),
            new Symbol(0x26, ">"),
            new Symbol(0x27, "^"),
            new Symbol(0x28, "!"),
            new Symbol(0xA, "NAME"),
            new Symbol(0x2C, ":"),
            new Symbol(0x62, "]"),
            new Symbol(0x7, "SEPARATOR"),
            new Symbol(0xC, "QUOTED_DATA"),
            new Symbol(0xD, "ESCAPEES"),
            new Symbol(0x23, "=>"),
            new Symbol(0x18, ".."),
            new Symbol(0x24, "->"),
            new Symbol(0x2D, "cf"),
            new Symbol(0x63, "cs"),
            new Symbol(0xE, "SYMBOL_TERMINAL_TEXT"),
            new Symbol(0xF, "SYMBOL_TERMINAL_SET"),
            new Symbol(0x12, "SYMBOL_VALUE_UINT8"),
            new Symbol(0x2B, "rules"),
            new Symbol(0x10, "SYMBOL_TERMINAL_UBLOCK"),
            new Symbol(0x11, "SYMBOL_TERMINAL_UCAT"),
            new Symbol(0x13, "SYMBOL_VALUE_UINT16"),
            new Symbol(0x29, "options"),
            new Symbol(0x2E, "grammar"),
            new Symbol(0x2A, "terminals") };
        public FileCentralDogmaLexer(string input) : base(automaton, terminals, 0x7, new System.IO.StringReader(input)) {}
        public FileCentralDogmaLexer(System.IO.TextReader input) : base(automaton, terminals, 0x7, input) {}
    }
}
