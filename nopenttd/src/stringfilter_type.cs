/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file stringfilter_type.h Searching and filtering using a stringterm. */

using System;
using Nopenttd.Core;
using Nopenttd.src;

namespace Nopenttd
{

//#include "core/smallvec_type.hpp"
//#include "strings_type.h"

/**
 * String filter and state.
 *
 * The filter takes a stringterm and parses it into words separated by whitespace.
 * The whitespace-separation can be avoided by quoting words in the searchterm using " or '.
 * The quotation characters can be nested or concatenated in a unix-shell style.
 *
 * When filtering an item, all words are checked for matches, and the filter matches if every word
 * matched. So, effectively this is a AND search for all entered words.
 *
 * Once the filter is set up using SetFilterTerm, multiple items can be filtered consecutively.
 *  1. For every item first call ResetState() which resets the matching-state.
 *  2. Pass all lines of the item via AddLine() to the filter.
 *  3. Check the matching-result for the item via GetState().
 */
    //struct
    public class StringFilter
    {
        /** State of a single filter word */

        private struct WordState
        {
            /// Word to filter for.
            string start;

            /// Already matched?       
            bool match;
        };

        /// Parsed filter string. Words separated by 0.
        private string filter_buffer;

        /// Word index and filter state.       
        private SmallVector<WordState> word_index;

        /// Summary of filter state: Number of words matched.       
        private uint word_matches;

        /// Match case-sensitively (usually a static variable).
        private Shared<bool> case_sensitive;

        /**
         * Constructor for filter.
         * @param case_sensitive Pointer to a (usually static) variable controlling the case-sensitivity. NULL means always case-insensitive.
         */

        public StringFilter(Shared<bool> case_sensitive = null)
        {
            filter_buffer = null;
            word_matches = 0;
            this.case_sensitive = case_sensitive;
        }

        public void SetFilterTerm(string str)
        {
            throw new NotImplementedException("Dummy");
        }

        /**
         * Check whether any filter words were entered.
         * @return true if no words were entered.
         */

        public bool IsEmpty()
        {
            return this.word_index.Length() == 0;
        }

        public void ResetState()
        {
            throw new NotImplementedException("Dummy");
        }

        public void AddLine(string str)
        {
            throw new NotImplementedException("Dummy");
        }
        public void AddLine(StringID str)
        {
            throw new NotImplementedException("Dummy");
        }

        /**
         * Get the matching state of the current item.
         * @return true if matched.
         */

        public bool GetState()
        {
            return this.word_matches == this.word_index.Length();
        }
    }
}