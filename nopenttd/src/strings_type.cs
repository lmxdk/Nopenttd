/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file strings_type.h Types related to strings. */

namespace Nopenttd
{

/**
 * Numeric value that represents a string, independent of the selected language.
 */

    public struct StringID
    {
        public ushort Id { get; set; }

        public StringID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(StringID id)
        {
            return id.Id;
        }

        public static implicit operator StringID(ushort id)
        {
            return new StringID(id);
        }
    }



/** Directions a text can go to */

    public enum TextDirection
    {
        /// Text is written left-to-right by default
        TD_LTR,

        /// Text is written right-to-left by default
        TD_RTL,
    }

/** Special string constants */

    public enum SpecialStrings
    {

        /* special strings for town names. the town name is generated dynamically on request. */
        SPECSTR_TOWNNAME_START = 0x20C0,
        SPECSTR_TOWNNAME_ENGLISH = SPECSTR_TOWNNAME_START,
        SPECSTR_TOWNNAME_FRENCH,
        SPECSTR_TOWNNAME_GERMAN,
        SPECSTR_TOWNNAME_AMERICAN,
        SPECSTR_TOWNNAME_LATIN,
        SPECSTR_TOWNNAME_SILLY,
        SPECSTR_TOWNNAME_SWEDISH,
        SPECSTR_TOWNNAME_DUTCH,
        SPECSTR_TOWNNAME_FINNISH,
        SPECSTR_TOWNNAME_POLISH,
        SPECSTR_TOWNNAME_SLOVAK,
        SPECSTR_TOWNNAME_NORWEGIAN,
        SPECSTR_TOWNNAME_HUNGARIAN,
        SPECSTR_TOWNNAME_AUSTRIAN,
        SPECSTR_TOWNNAME_ROMANIAN,
        SPECSTR_TOWNNAME_CZECH,
        SPECSTR_TOWNNAME_SWISS,
        SPECSTR_TOWNNAME_DANISH,
        SPECSTR_TOWNNAME_TURKISH,
        SPECSTR_TOWNNAME_ITALIAN,
        SPECSTR_TOWNNAME_CATALAN,
        SPECSTR_TOWNNAME_LAST = SPECSTR_TOWNNAME_CATALAN,

        /* special strings for company names on the form "TownName transport". */
        SPECSTR_COMPANY_NAME_START = 0x70EA,
        SPECSTR_COMPANY_NAME_LAST = SPECSTR_COMPANY_NAME_START + SPECSTR_TOWNNAME_LAST - SPECSTR_TOWNNAME_START,

        SPECSTR_SILLY_NAME = 0x70E5,
        SPECSTR_ANDCO_NAME = 0x70E6,
        SPECSTR_PRESIDENT_NAME = 0x70E7,

        /* reserve MAX_LANG strings for the *.lng files */
        SPECSTR_LANGUAGE_START = 0x7100,
        SPECSTR_LANGUAGE_END = SPECSTR_LANGUAGE_START + (int) StringConstants.MAX_LANG - 1,

        /* reserve 32 strings for various screen resolutions */
        SPECSTR_RESOLUTION_START = SPECSTR_LANGUAGE_END + 1,
        SPECSTR_RESOLUTION_END = SPECSTR_RESOLUTION_START + 0x1F,
    }
}