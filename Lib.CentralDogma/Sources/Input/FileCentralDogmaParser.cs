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
using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Input
{
    internal class FileCentralDogmaParser : LRkParser
    {
        private static readonly LRkAutomaton automaton = LRkAutomaton.Find(typeof(FileCentralDogmaParser), "FileCentralDogmaParser.bin");
        private static readonly Symbol[] variables = {
            new Symbol(0x2F, "option"), 
            new Symbol(0x30, "terminal_def_atom_any"), 
            new Symbol(0x31, "terminal_def_atom_unicode"), 
            new Symbol(0x32, "terminal_def_atom_text"), 
            new Symbol(0x33, "terminal_def_atom_set"), 
            new Symbol(0x34, "terminal_def_atom_ublock"), 
            new Symbol(0x35, "terminal_def_atom_ucat"), 
            new Symbol(0x36, "terminal_def_atom_span"), 
            new Symbol(0x37, "terminal_def_atom"), 
            new Symbol(0x38, "terminal_def_element"), 
            new Symbol(0x39, "terminal_def_cardinalilty"), 
            new Symbol(0x3A, "terminal_def_repetition"), 
            new Symbol(0x3B, "terminal_def_fragment"), 
            new Symbol(0x3C, "terminal_def_restrict"), 
            new Symbol(0x3D, "terminal_definition"), 
            new Symbol(0x3E, "terminal_subgrammar"), 
            new Symbol(0x3F, "terminal"), 
            new Symbol(0x40, "rule_sym_action"), 
            new Symbol(0x41, "rule_sym_virtual"), 
            new Symbol(0x42, "rule_sym_ref_params"), 
            new Symbol(0x43, "rule_sym_ref_template"), 
            new Symbol(0x44, "rule_sym_ref_simple"), 
            new Symbol(0x45, "rule_def_atom"), 
            new Symbol(0x46, "rule_def_element"), 
            new Symbol(0x47, "rule_def_tree_action"), 
            new Symbol(0x48, "rule_def_repetition"), 
            new Symbol(0x49, "rule_def_fragment"), 
            new Symbol(0x4A, "rule_def_restrict"), 
            new Symbol(0x4B, "rule_def_choice"), 
            new Symbol(0x4C, "rule_definition"), 
            new Symbol(0x4D, "rule_template_params"), 
            new Symbol(0x4E, "cf_rule_template"), 
            new Symbol(0x4F, "cf_rule_simple"), 
            new Symbol(0x50, "grammar_options"), 
            new Symbol(0x51, "grammar_terminals"), 
            new Symbol(0x52, "grammar_cf_rules"), 
            new Symbol(0x53, "grammar_parency"), 
            new Symbol(0x54, "cf_grammar"), 
            new Symbol(0x55, "_v10"), 
            new Symbol(0x56, "_v12"), 
            new Symbol(0x57, "_v14"), 
            new Symbol(0x58, "_v18"), 
            new Symbol(0x59, "_v1C"), 
            new Symbol(0x5A, "_v1D"), 
            new Symbol(0x5B, "_v1E"), 
            new Symbol(0x5C, "_v1F"), 
            new Symbol(0x5D, "_v21"), 
            new Symbol(0x5E, "_v23"), 
            new Symbol(0x5F, "_v25"), 
            new Symbol(0x60, "_v27"), 
            new Symbol(0x64, "cs_rule_context"), 
            new Symbol(0x65, "cs_rule_template"), 
            new Symbol(0x66, "cs_rule_simple"), 
            new Symbol(0x67, "grammar_cs_rules"), 
            new Symbol(0x68, "cs_grammar"), 
            new Symbol(0x69, "_v2C"), 
            new Symbol(0x6A, "file_item"), 
            new Symbol(0x6B, "file"), 
            new Symbol(0x6C, "_v2E"), 
            new Symbol(0x6D, "_Axiom_") };
        private static readonly Symbol[] virtuals = {
            new Symbol(0, "range"), 
            new Symbol(0, "concat"), 
            new Symbol(0, "emptypart") };
        public FileCentralDogmaParser(FileCentralDogmaLexer lexer) : base (automaton, variables, virtuals, null, lexer) { }
    }
}
