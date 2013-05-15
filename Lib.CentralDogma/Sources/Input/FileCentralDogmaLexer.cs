/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 0.6.0.0
 */

using System.Collections.Generic;
using Hime.Redist.Symbols;
using Hime.Redist.Lexer;
using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Input
{
    internal class FileCentralDogmaLexer : TextLexer
    {
        private static readonly TextLexerAutomaton automaton = TextLexerAutomaton.FindAutomaton(typeof(FileCentralDogmaLexer), "FileCentralDogmaLexer.bin");
        public static readonly Terminal[] terminals = {
            new Terminal(0x1, "ε"),
            new Terminal(0x2, "$"),
            new Terminal(0x61, "["),
            new Terminal(0xB, "INTEGER"),
            new Terminal(0x14, "="),
            new Terminal(0x15, ";"),
            new Terminal(0x16, "."),
            new Terminal(0x17, "~"),
            new Terminal(0x19, "("),
            new Terminal(0x1A, ")"),
            new Terminal(0x1B, "*"),
            new Terminal(0x1C, "+"),
            new Terminal(0x1D, "?"),
            new Terminal(0x1E, "{"),
            new Terminal(0x1F, ","),
            new Terminal(0x20, "}"),
            new Terminal(0x21, "-"),
            new Terminal(0x22, "|"),
            new Terminal(0x25, "<"),
            new Terminal(0x26, ">"),
            new Terminal(0x27, "^"),
            new Terminal(0x28, "!"),
            new Terminal(0xA, "NAME"),
            new Terminal(0x2C, ":"),
            new Terminal(0x62, "]"),
            new Terminal(0x7, "SEPARATOR"),
            new Terminal(0xC, "QUOTED_DATA"),
            new Terminal(0xD, "ESCAPEES"),
            new Terminal(0x23, "=>"),
            new Terminal(0x18, ".."),
            new Terminal(0x24, "->"),
            new Terminal(0x2D, "cf"),
            new Terminal(0x63, "cs"),
            new Terminal(0xE, "SYMBOL_TERMINAL_TEXT"),
            new Terminal(0xF, "SYMBOL_TERMINAL_SET"),
            new Terminal(0x12, "SYMBOL_VALUE_UINT8"),
            new Terminal(0x2B, "rules"),
            new Terminal(0x10, "SYMBOL_TERMINAL_UBLOCK"),
            new Terminal(0x11, "SYMBOL_TERMINAL_UCAT"),
            new Terminal(0x13, "SYMBOL_VALUE_UINT16"),
            new Terminal(0x29, "options"),
            new Terminal(0x2E, "grammar"),
            new Terminal(0x2A, "terminals") };
        public FileCentralDogmaLexer(string input) : base(automaton, terminals, 0x7, new System.IO.StringReader(input)) {}
        public FileCentralDogmaLexer(System.IO.TextReader input) : base(automaton, terminals, 0x7, input) {}
    }
}
