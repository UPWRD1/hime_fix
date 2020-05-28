/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

//! Module for generating lexer code in C#

use crate::errors::Error;
use crate::grammars::{Grammar, TerminalRef, TerminalSet, PREFIX_GENERATED_TERMINAL};
use crate::output::get_lexer_bin_name_net;
use crate::output::helper::to_upper_camel_case;
use crate::{Modifier, CRATE_VERSION};
use std::fs::File;
use std::io::{self, Write};
use std::path::PathBuf;

/// Writes a generated .Net file header
fn write_header(writer: &mut dyn Write) -> Result<(), Error> {
    writeln!(writer, "/*")?;
    writeln!(writer, " * WARNING: this file has been generated by")?;
    writeln!(writer, " * Hime Parser Generator {}", CRATE_VERSION)?;
    writeln!(writer, " */")?;
    Ok(())
}

/// Generates code for the specified file
pub fn write(
    path: Option<&String>,
    file_name: String,
    grammar: &Grammar,
    expected: &TerminalSet,
    separator: Option<TerminalRef>,
    nmespace: &str,
    modifier: Modifier
) -> Result<(), Error> {
    let mut final_path = PathBuf::new();
    if let Some(path) = path {
        final_path.push(path);
    }
    final_path.push(file_name);
    let file = File::create(final_path)?;
    let mut writer = io::BufWriter::new(file);

    let name = to_upper_camel_case(&grammar.name);
    let base_lexer = if grammar.contexts.len() > 1 {
        "ContextSensitiveLexer"
    } else {
        "ContextFreeLexer"
    };
    let modifier = match modifier {
        Modifier::Public => "public",
        Modifier::Internal => "internal"
    };
    let bin_name = get_lexer_bin_name_net(grammar);
    let separator = match separator {
        None => 0xFFFF,
        Some(terminal_ref) => terminal_ref.sid()
    };

    write_header(&mut writer)?;
    writeln!(writer)?;
    writeln!(writer, "using System.CodeDom.Compiler;")?;
    writeln!(writer, "using System.Collections.Generic;")?;
    writeln!(writer, "using System.IO;")?;
    writeln!(writer, "using Hime.Redist;")?;
    writeln!(writer, "using Hime.Redist.Lexer;")?;
    writeln!(writer)?;
    writeln!(writer, "namespace {}", nmespace)?;
    writeln!(writer, "{{")?;

    writeln!(writer, "\t/// <summary>")?;
    writeln!(writer, "\t/// Represents a lexer")?;
    writeln!(writer, "\t/// </summary>")?;
    writeln!(
        writer,
        "\t[GeneratedCodeAttribute(\"Hime.SDK\", \"{}\")]",
        CRATE_VERSION
    )?;
    writeln!(
        writer,
        "\t{} class {}Lexer : {}",
        modifier, &name, base_lexer
    )?;
    writeln!(writer, "\t{{")?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// The automaton for this lexer")?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(writer, "\t\tprivate static readonly Automaton commonAutomaton = Automaton.Find(typeof({}Lexer), \"{}\");", &name, &bin_name)?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// Contains the constant IDs for the terminals for this lexer"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t[GeneratedCodeAttribute(\"Hime.SDK\", \"{}\")]",
        CRATE_VERSION
    )?;
    writeln!(writer, "\t\tpublic class ID")?;
    writeln!(writer, "\t\t{{")?;
    for terminal_ref in expected.content.iter().skip(2) {
        let terminal = grammar.get_terminal(terminal_ref.sid()).unwrap();
        if terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        }
        writeln!(writer, "\t\t\t/// <summary>")?;
        writeln!(
            writer,
            "\t\t\t/// The unique identifier for terminal {}",
            &terminal.name
        )?;
        writeln!(writer, "\t\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t\tpublic const int Terminal{} = 0x{:04X};",
            to_upper_camel_case(&terminal.name),
            terminal.id
        )?;
    }
    writeln!(writer, "\t\t}}")?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// Contains the constant IDs for the contexts for this lexer"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t[GeneratedCodeAttribute(\"Hime.SDK\", \"{}\")]",
        CRATE_VERSION
    )?;
    writeln!(writer, "\t\tpublic class Context")?;
    writeln!(writer, "\t\t{{")?;
    writeln!(writer, "\t\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t\t/// The unique identifier for the default context"
    )?;
    writeln!(writer, "\t\t\t/// </summary>")?;
    writeln!(writer, "\t\t\tpublic const int Default = 0;")?;
    for (index, context) in grammar.contexts.iter().enumerate().skip(1) {
        writeln!(writer, "\t\t\t/// <summary>")?;
        writeln!(
            writer,
            "\t\t\t/// The unique identifier for context {}",
            context
        )?;
        writeln!(writer, "\t\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t\tpublic const int {} = 0x{:04X};",
            to_upper_camel_case(context),
            index
        )?;
    }
    writeln!(writer, "\t\t}}")?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// The collection of terminals matched by this lexer"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(writer, "\t\t/// <remarks>")?;
    writeln!(
        writer,
        "\t\t/// The terminals are in an order consistent with the automaton,"
    )?;
    writeln!(writer, "\t\t/// so that terminal indices in the automaton can be used to retrieve the terminals in this table")?;
    writeln!(writer, "\t\t/// </remarks>")?;
    writeln!(
        writer,
        "\t\tprivate static readonly Symbol[] terminals = {{"
    )?;
    writeln!(writer, "\t\t\tnew Symbol(0x0001, \"ε\"),")?;
    write!(writer, "\t\t\tnew Symbol(0x0002, \"$\")")?;
    for terminal_ref in expected.content.iter().skip(2) {
        let terminal = grammar.get_terminal(terminal_ref.sid()).unwrap();
        writeln!(writer, ",")?;
        write!(writer, "\t\t\t")?;
        write!(
            writer,
            "new Symbol(0x{:04X}, \"{}\")",
            terminal.id,
            terminal.value.replace("\"", "\\\"")
        )?;
    }
    writeln!(writer, " }};")?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// Initializes a new instance of the lexer")?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t/// <param name=\"input\">The lexer's input</param>"
    )?;
    writeln!(
        writer,
        "\t\tpublic {}Lexer(string input) : base(commonAutomaton, terminals, 0x{:04X}, input) {{}}",
        &name, separator
    )?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// Initializes a new instance of the lexer")?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t/// <param name=\"input\">The lexer's input</param>"
    )?;
    writeln!(writer, "\t\tpublic {}Lexer(TextReader input) : base(commonAutomaton, terminals, 0x{:04X}, input) {{}}", &name,separator)?;

    writeln!(writer, "\t}}")?;
    writeln!(writer, "}}")?;
    Ok(())
}
