/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file strings_func.h Functions related to OTTD's strings. */

#ifndef STRINGS_FUNC_H
#define STRINGS_FUNC_H

#include "strings_type.h"
#include "string_type.h"
#include "gfx_type.h"

extern StringParameters _global_string_params;

char *GetString(char *buffr, StringID string, const char *last);
char *GetStringWithArgs(char *buffr, StringID string, StringParameters *args, const char *last, uint case_index = 0, bool game_script = false);
const char *GetStringPtr(StringID string);

uint ConvertKmhishSpeedToDisplaySpeed(uint speed);
uint ConvertDisplaySpeedToKmhishSpeed(uint speed);

void InjectDParam(uint amount);

/**
 * Set a string parameter \a v at index \a n in a given array \a s.
 * @param s Array of string parameters.
 * @param n Index of the string parameter.
 * @param v Value of the string parameter.
 */
static inline void SetDParamX(uint64 *s, uint n, uint64 v)
{
	s[n] = v;
}

/**
 * Set a string parameter \a v at index \a n in the global string parameter array.
 * @param n Index of the string parameter.
 * @param v Value of the string parameter.
 */
static inline void SetDParam(uint n, uint64 v)
{
	_global_string_params.SetParam(n, v);
}

void SetDParamMaxValue(uint n, uint64 max_value, uint min_count = 0, FontSize size = FS_NORMAL);
void SetDParamMaxDigits(uint n, uint count, FontSize size = FS_NORMAL);

void SetDParamStr(uint n, const char *str);

void CopyInDParam(int offs, const uint64 *src, int num);
void CopyOutDParam(uint64 *dst, int offs, int num);
void CopyOutDParam(uint64 *dst, const char **strings, StringID string, int num);

/**
 * Get the current string parameter at index \a n from parameter array \a s.
 * @param s Array of string parameters.
 * @param n Index of the string parameter.
 * @return Value of the requested string parameter.
 */
static inline uint64 GetDParamX(const uint64 *s, uint n)
{
	return s[n];
}

/**
 * Get the current string parameter at index \a n from the global string parameter array.
 * @param n Index of the string parameter.
 * @return Value of the requested string parameter.
 */
static inline uint64 GetDParam(uint n)
{
	return _global_string_params.GetParam(n);
}

extern TextDirection _current_text_dir; ///< Text direction of the currently selected language

void InitializeLanguagePacks();
const char *GetCurrentLanguageIsoCode();

int CDECL StringIDSorter(const StringID *a, const StringID *b);

/**
 * A searcher for missing glyphs.
 */
class MissingGlyphSearcher {
public:
	/** Make sure everything gets destructed right. */
	virtual ~MissingGlyphSearcher() {}

	/**
	 * Get the next string to search through.
	 * @return The next string or NULL if there is none.
	 */
	virtual const char *NextString() = 0;

	/**
	 * Get the default (font) size of the string.
	 * @return The font size.
	 */
	virtual FontSize DefaultSize() = 0;

	/**
	 * Reset the search, i.e. begin from the beginning again.
	 */
	virtual void Reset() = 0;

	/**
	 * Whether to search for a monospace font or not.
	 * @return True if searching for monospace.
	 */
	virtual bool Monospace() = 0;

	/**
	 * Set the right font names.
	 * @param settings  The settings to modify.
	 * @param font_name The new font name.
	 */
	virtual void SetFontNames(struct FreeTypeSettings *settings, const char *font_name) = 0;

	bool FindMissingGlyphs(const char **str);
};

void CheckForMissingGlyphs(bool base_font = true, MissingGlyphSearcher *search = NULL);

#endif /* STRINGS_FUNC_H */
