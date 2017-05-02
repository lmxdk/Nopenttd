/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file language.h Information about languages and their files. */

using System;
using System.Collections.Generic;

namespace Nopenttd
{

    public class Language
    {
        /** The actual list of language meta data. */
        //typedef SmallVector<LanguageMetadata, 4> LanguageList;
		//extern LanguageList _languages;

        /** The currently loaded language. */
        public static LanguageMetadata _current_language;
		public static Collator _current_collator;

        bool ReadLanguagePack(LanguageMetadata* lang);
        const LanguageMetadata* GetLanguage(byte newgrflangid);



        /// The (maximum) length of a case/gender string.
        public const byte CASE_GENDER_LEN = 16;

        /// Maximum number of supported genders.
        public const byte MAX_NUM_GENDERS = 8;

        /// Maximum number of supported cases.
        public const byte MAX_NUM_CASES = 16;

        /// The offset for the tab size.
        public const int TAB_SIZE_OFFSET = 0;

        /// The number of bits used for the tab size.
        public const int TAB_SIZE_BITS = 11;

        /// The number of values in a tab.
        public const int TAB_SIZE = 1 << TAB_SIZE_BITS;

        /// The offset for the tab count.
        public const int TAB_COUNT_OFFSET = TAB_SIZE_BITS;

        /// The number of bits used for the amount of tabs.
        public const int TAB_COUNT_BITS = 5;

        /// The amount of tabs.
        public const int TAB_COUNT = 1 << TAB_COUNT_BITS;
    }

/** Header of a language file. */
    public class LanguagePackHeader
    {
        /// Identifier for OpenTTD language files, big endian for "LANG"
        public const uint IDENT = 0x474E414C;

        /// 32-bits identifier
        public uint ident;

        /// 32-bits of auto generated version info which is basically a hash of strings.h
        public uint version;

        /// the international name of this language
        public string name; //[32];

        /// the localized name of this language
        public string own_name; //[32];

        /// the ISO code for the language (not country code)
        public string isocode; //[16];

        /// the offsets
        public ushort[] offsets = new ushort[Language.TAB_COUNT];

        /** Thousand separator used for anything not currencies */
        public string digit_group_separator; //[8];

        /** Thousand separator used for currencies */
        public string digit_group_separator_currency; //[8];

        /** Decimal separator */
        public string digit_decimal_separator; //[8];

        /// number of missing strings.
        public ushort missing;

        /// plural form index
        public byte plural_form;

        /// default direction of the text
        public byte text_dir;

        /**
         * Windows language ID:
         * Windows cannot and will not convert isocodes to something it can use to
         * determine whether a font can be used for the language or not. As a result
         * of that we need to pass the language id via strgen to OpenTTD to tell
         * what language it is in "Windows". The ID is the 'locale identifier' on:
         *   http://msdn.microsoft.com/en-us/library/ms776294.aspx
         */
        /// windows language id
        public ushort winlangid;

        /// newgrf language id
        public byte newgrflangid;

        /// the number of genders of this language
        public byte num_genders;

        /// the number of cases of this language
        public byte num_cases;

        /// pad header to be a multiple of 4
        public byte pad = new byte[3];

        /// the genders used by this translation
        private string[] genders = new string[Language.MAX_NUM_GENDERS]; //[CASE_GENDER_LEN]; 

        /// the cases used by this translation
        private string[] cases = new string[Language.MAX_NUM_CASES]; //[CASE_GENDER_LEN];     

        bool IsValid() const;

        /**
         * Get the index for the given gender.
         * @param gender_str The string representation of the gender.
         * @return The index of the gender, or MAX_NUM_GENDERS when the gender is unknown.
         */
        public byte GetGenderIndex(string gender_str)
        {
            for (byte i = 0; i < genders.Length; i++)
            {
                if (string.Equals(gender_str, genders[i], StringComparison.CurrentCultureIgnoreCase)) return i;
            }
            return (byte) genders.Length;
        }

        /**
         * Get the index for the given case.
         * @param case_str The string representation of the case.
         * @return The index of the case, or MAX_NUM_CASES when the case is unknown.
         */
        public byte GetCaseIndex(string case_str)
        {
            for (byte i = 0; i < cases.Length; i++)
            {
                if (string.Equals(case_str, cases[i], StringComparison.CurrentCultureIgnoreCase)) return i;
            }
            return (byte) cases.Length;
        }
    };

/** Metadata about a single language. */
    public class LanguageMetadata : LanguagePackHeader
    {
        ///< Name of the file we read this data from.
        public string file; //[MAX_PATH];
    };
}
	