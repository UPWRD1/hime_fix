/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters and others
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
 ******************************************************************************/

package org.xowl.hime.redist.lexer;

/**
 * Represents a matched terminal information within a lexer's automaton
 *
 * @author Laurent Wouters
 */
public class MatchedTerminal {
    /**
     * The context
     */
    private final char context;
    /**
     * The terminal's index
     */
    private final char index;

    /**
     * Gets the context of the matched terminal
     *
     * @return The context of the matched terminal
     */
    public int getContext() {
        return context;
    }

    /**
     * Gets the index of the matched terminal
     *
     * @return The index of the matched terminal
     */
    public int getIndex() {
        return index;
    }

    /**
     * Initializes this matched terminal data
     *
     * @param context The context
     * @param index   The terminal's index
     */
    protected MatchedTerminal(char context, char index) {
        this.context = context;
        this.index = index;
    }
}
