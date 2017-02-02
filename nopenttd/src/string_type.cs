/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file string_type.h Types for strings. */

using System;

namespace Nopenttd
{

    public class StringConstants
    {
/** A non-breaking space. */
        public const string NBSP = "\xC2\xA0";

/** A left-to-right marker, marks the next character as left-to-right. */
        public const string LRM = "\xE2\x80\x8E";

        /* The following are directional formatting codes used to get the LTR and RTL strings right:
         * http://www.unicode.org/unicode/reports/tr9/#Directional_Formatting_Codes */

        /// The next character acts like a left-to-right character.
        public const int CHAR_TD_LRM = 0x200E;

        /// The next character acts like a right-to-left character.
        public const int CHAR_TD_RLM = 0x200F;

        /// The following text is embedded left-to-right.
        public const int CHAR_TD_LRE = 0x202A;

        /// The following text is embedded right-to-left.
        public const int CHAR_TD_RLE = 0x202B;

        /// Force the following characters to be treated as left-to-right characters.
        public const int CHAR_TD_LRO = 0x202D;

        /// Force the following characters to be treated as right-to-left characters.
        public const int CHAR_TD_RLO = 0x202E;

        /// Restore the text-direction state to before the last LRE, RLE, LRO or RLO.
        public const int CHAR_TD_PDF = 0x202C;
    }

/**
 * Valid filter types for IsValidChar.
 */

    public enum CharSetFilter
    {
        /// Both numeric and alphabetic and spaces and stuff
        CS_ALPHANUMERAL,

        /// Only numeric ones    
        CS_NUMERAL,

        /// Only numbers and spaces    
        CS_NUMERAL_SPACE,

        /// Only alphabetic values    
        CS_ALPHA,

        /// Only hexadecimal characters    
        CS_HEXADECIMAL,
    }

/** Type for wide characters, i.e. non-UTF8 encoded unicode characters. */
//typedef uint32 WChar;


/** Settings for the string validation. */

    [Flags]
    public enum StringValidationSettings
    {
        /// Allow nothing and replace nothing.
        SVS_NONE = 0,

        /// Replace the unknown/bad bits with question marks.
        SVS_REPLACE_WITH_QUESTION_MARK = 1 << 0,

        /// Allow newlines.
        SVS_ALLOW_NEWLINE = 1 << 1,

        /// Allow the special control codes.
        SVS_ALLOW_CONTROL_CODE = 1 << 2,
    }
}