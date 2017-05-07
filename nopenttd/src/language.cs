/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file language.h Information about languages and their files. */

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
}
	