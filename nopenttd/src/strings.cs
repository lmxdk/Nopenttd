/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file strings.cpp Handling of translated strings. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Nopenttd.Core;
using Nopenttd.src;
using Nopenttd.src.Core.Exceptions;
using Nopenttd.src.Settings;

namespace Nopenttd
{
    public class Strings
    {

        /// The file (name) stored in the configuration.
        string _config_language_file;             /// The actual list of language meta data
        public static List<LanguageMetadata> _languages = new List<LanguageMetadata>();                          /// The currently loaded language.
		public static LanguageMetadata _current_language = null;
        /// Text direction of the currently selected language.
        public TextDirection _current_text_dir;

        /// Collator for the language currently in use.
        CompareInfo _current_collator = null;              //maybe use CultureInfo/CompareInfo?


        /// Global array of string parameters. To access, use #SetDParam.
        public static ulong[] _global_string_params_data = new ulong[20];
        /// Type of parameters stored in #_decode_parameters
        public static  char[] _global_string_params_type = new char[20];
        StringParameters _global_string_params = new StringParameters(_global_string_params_data, _global_string_params_type);


/**
 * Set DParam n to some number that is suitable for string size computations.
 * @param n Index of the string parameter.
 * @param max_value The biggest value which shall be displayed.
 *                  For the result only the number of digits of \a max_value matter.
 * @param min_count Minimum number of digits independent of \a max.
 * @param size  Font of the number
 */
void SetDParamMaxValue(uint n, ulong max_value, uint min_count, FontSize size)
{
	uint num_digits = 1;
	while (max_value >= 10) {
		num_digits++;
		max_value /= 10;
	}
	SetDParamMaxDigits(n, Math.Max(min_count, num_digits), size);
}

/**
 * Set DParam n to some number that is suitable for string size computations.
 * @param n Index of the string parameter.
 * @param count Number of digits which shall be displayable.
 * @param size  Font of the number
 */
void SetDParamMaxDigits(uint n, uint count, FontSize size)
{
	uint front, next;
	GetBroadestDigit(&front, &next, size);
	ulong val = count > 1 ? front : next;
	for (; count > 1; count--) {
		val = 10 * val + next;
	}
	SetDParam(n, val);
}

/**
 * Copy \a num string parameters from array \a src into the global string parameter array.
 * @param offs Index in the global array to copy the first string parameter to.
 * @param src  Source array of string parameters.
 * @param num  Number of string parameters to copy.
 */
void CopyInDParam(int offs, const ulong *src, int num)
{

	MemCpyT(_global_string_params.GetPointerToOffset(offs), src, num);
}

/**
 * Copy \a num string parameters from the global string parameter array to the \a dst array.
 * @param dst  Destination array of string parameters.
 * @param offs Index in the global array to copy the first string parameter from.
 * @param num  Number of string parameters to copy.
 */
void CopyOutDParam(ulong *dst, int offs, int num)
{
	MemCpyT(dst, _global_string_params.GetPointerToOffset(offs), num);
}

/**
 * Copy \a num string parameters from the global string parameter array to the \a dst array.
 * Furthermore clone raw string parameters into \a strings and amend the data in \a dst.
 * @param dst     Destination array of string parameters.
 * @param strings Destination array for clone of the raw strings. Must be of same length as dst. Deallocation left to the caller.
 * @param string  The string used to determine where raw strings are and where there are no raw strings.
 * @param num     Number of string parameters to copy.
 */
void CopyOutDParam(ulong *dst, const char **strings, StringID string, int num)
{
	char buf[DRAW_STRING_BUFFER];
	GetString(buf, stringof(buf));

	MemCpyT(dst, _global_string_params.GetPointerToOffset(0), num);
	for (int i = 0; i < num; i++) {
		if (_global_string_params.HasTypeInformation() && _global_string_params.GetTypeAtOffset(i) == StringControlCode.SCC_RAW_STRING_POINTER) {
			strings[i] = stredup((const char *)(size_t)_global_string_params.GetParam(i));
			dst[i] = (size_t)strings[i];
		} else {
			strings[i] = null;
		}
	}
}

static char *StationGetSpecialString(StringBuilder buff, int x);
static char *GetSpecialTownNameString(StringBuilder buff, int ind, uint32 seed);
static char *GetSpecialNameString(StringBuilder buff, int ind, StringParameters *args);

static char *FormatString(StringBuilder buff, const char *str, StringParameters *args, uint case_index = 0, bool game_script = false, bool dry_run = false);

static char **_langpack_offs;
static LanguagePack *_langpack;
static uint _langtab_num[TAB_COUNT];   ///< Offset into langpack offs
static uint _langtab_start[TAB_COUNT]; ///< Offset into langpack offs
static bool _scan_for_gender_data = false;  ///< Are we scanning for the gender of the current string? (instead of formatting it)


const char *GetStringPtr(StringID string)
{
	switch (GB(string, TAB_COUNT_OFFSET, TAB_COUNT_BITS)) {
		case GAME_TEXT_TAB: return GetGameStringPtr(GB(string, TAB_SIZE_OFFSET, TAB_SIZE_BITS));
		/* 0xD0xx and 0xD4xx IDs have been converted earlier. */
		case 26: NOT_REACHED();
		case 28: return GetGRFStringPtr(GB(string, TAB_SIZE_OFFSET, TAB_SIZE_BITS));
		case 29: return GetGRFStringPtr(GB(string, TAB_SIZE_OFFSET, TAB_SIZE_BITS) + 0x0800);
		case 30: return GetGRFStringPtr(GB(string, TAB_SIZE_OFFSET, TAB_SIZE_BITS) + 0x1000);
		default: return _langpack_offs[_langtab_start[GB(string, TAB_COUNT_OFFSET, TAB_COUNT_BITS)] + GB(string, TAB_SIZE_OFFSET, TAB_SIZE_BITS)];
	}
}

/**
 * Get a parsed string with most special stringcodes replaced by the string parameters.
 * @param buffr  Pointer to a string buffer where the formatted string should be written to.
 * @param string
 * @param args   Arguments for the string.
 * @param last   Pointer just past the end of \a buffr.
 * @param case_index  The "case index". This will only be set when FormatString wants to print the string in a different case.
 * @param game_script The string is coming directly from a game script.
 * @return       Pointer to the final zero byte of the formatted string.
 */
public string GetStringWithArgs(StringBuilder buff, StringID str, StringParameters args, uint case_index, bool game_script)
{
	if (str == 0) return GetStringWithArgs(buff, STR_UNDEFINED, args);

	uint index = BitMath.GB(str, Language.TAB_SIZE_OFFSET, Language.TAB_SIZE_BITS);
	uint tab   = BitMath.GB(str, Language.TAB_COUNT_OFFSET, Language.TAB_COUNT_BITS);

	switch (tab) {
		case 4:
			if (index >= 0xC0 && !game_script) {
				return GetSpecialTownNameString(buff, index - 0xC0, args.GetInt32());
			}
			break;

		case 14:
			if (index >= 0xE4 && !game_script) {
				return GetSpecialNameString(buff, index - 0xE4, args);
			}
			break;

		case 15:
			/* Old table for custom names. This is no longer used */
			if (!game_script) {
				error("Incorrect conversion of custom name string.");
			}
			break;

		case GAME_TEXT_TAB:
			return FormatString(buff, GetGameStringPtr(index), args, case_index, true);

		case 26:
			throw new NotReachedException();

		case 28:
			return FormatString(buff, GetGRFStringPtr(index), args, case_index);

		case 29:
			return FormatString(buff, GetGRFStringPtr(index + 0x0800), args, case_index);

		case 30:
			return FormatString(buff, GetGRFStringPtr(index + 0x1000), args, case_index);
	}

	if (index >= _langtab_num[tab]) {
		if (game_script) {
			return GetStringWithArgs(buff, STR_UNDEFINED, args);
		}
		error("String 0x%X is invalid. You are probably using an old version of the .lng file.\n", str);
	}

	return FormatString(buff, GetStringPtr(string), args, case_index);
}

string GetString(StringBuilder buff, StringID str)
{
	_global_string_params.ClearTypeInformation();
	_global_string_params.offset = 0;
	return GetStringWithArgs(buff, str, _global_string_params);
}


/**
 * This function is used to "bind" a C string to a OpenTTD dparam slot.
 * @param n slot of the string
 * @param str string to bind
 */
void SetDParamStr(uint n, string str)
{
	SetDParam(n, (ulong)str);
}

/**
 * Shift the string parameters in the global string parameter array by \a amount positions, making room at the beginning.
 * @param amount Number of positions to shift.
 */
void InjectDParam(uint amount)
{
	_global_string_params.ShiftParameters(amount);
}

private const int max_digits = 20;
/**
 * Format a number into a string.
 * @param buff      the buffer to write to
 * @param number    the number to write down
 * @param last      the last element in the buffer
 * @param separator the thousands-separator to use
 * @param zerofill  minimum number of digits to print for the integer part. The number will be filled with zeros at the front if necessary.
 * @param fractional_digits number of fractional digits to display after a decimal separator. The decimal separator is inserted
 *                          in front of the \a fractional_digits last digit of \a number.
 * @return till where we wrote
 */
static void FormatNumber(StringBuilder buff, long number, string separator, int zerofill = 1, int fractional_digits = 0)
{
	ulong divisor = 10000000000000000000UL;
	zerofill += fractional_digits;
	int thousands_offset = (max_digits - fractional_digits - 1) % 3;

	if (number < 0) {
		buff.Append("-");
		number = -number;
	}

	//TODO replace with logic from .net locale

	ulong num = (ulong)number;
	ulong tot = 0;
	for (int i = 0; i < max_digits; i++) {
		if (i == max_digits - fractional_digits) {
			var decimal_separator = _settings_game.locale.digit_decimal_separator;
		    if (decimal_separator == null)
		    {
		        decimal_separator = _langpack.digit_decimal_separator;
		    }
			buff.Append(decimal_separator);
		}

		ulong quot = 0;
		if (num >= divisor) {
			quot = num / divisor;
			num = num % divisor;
		}
		if ((tot |= quot) != 0 || i >= max_digits - zerofill) {
			buff.Append((int)quot);
		    if ((i % 3) == thousands_offset && i < max_digits - 1 - fractional_digits)
		    {
		        buff.Append(separator);
		    }
		}

		divisor /= 10;
	}
}

static void FormatCommaNumber(StringBuilder buff, long number, int fractional_digits = 0)
{
	var separator = _settings_game.locale.digit_group_separator;
    if (separator == null)
    {
        separator = _langpack.digit_group_separator;
    }
	FormatNumber(buff, number, separator, 1, fractional_digits);
}

static void FormatNoCommaNumber(StringBuilder buff, long number)
{
	FormatNumber(buff, number, "");
}

static void FormatZerofillNumber(StringBuilder buff, long number, long count)
{
	FormatNumber(buff, number, "", count);
}

static void FormatHexNumber(StringBuilder buff, ulong number)
{
    buff.AppendFormat("{0:x}", number);
}

/*                                   1   2^10  2^20  2^30  2^40  2^50  2^60 */
static readonly string[] iec_prefixes = new [] { "", "Ki", "Mi", "Gi", "Ti", "Pi", "Ei" };

/**
 * Format a given number as a number of bytes with the SI prefix.
 * @param buff   the buffer to write to
 * @param number the number of bytes to write down
 * @param last   the last element in the buffer
 * @return till where we wrote
 */
static void FormatBytes(StringBuilder buff, long number)
{
	Debug.Assert(number >= 0);
	
	uint id = 1;
	while (number >= 1024 * 1024) {
		number /= 1024;
		id++;
	}

	var decimal_separator = _settings_game.locale.digit_decimal_separator;
    if (decimal_separator == null)
    {
        decimal_separator = _langpack.digit_decimal_separator;
    }

	if (number < 1024) {
		id = 0;
		buff.Append((int)number);
	} else if (number < 1024 * 10) {
		buff.Append($"{(int)number / 1024}{decimal_separator}{((int)(number % 1024) * 100 / 1024):00}");
	} else if (number < 1024 * 100) {
		buff.Append($"{(int)number / 1024}{decimal_separator}{((int)(number % 1024) * 10 / 1024):0}");
	} else {
		Debug.Assert(number < 1024 * 1024);
		buff.Append((int)number / 1024);
	}

	Debug.Assert(id < iec_prefixes.Length);
	buff.Append($"{StringConstants.NBSP}{iec_prefixes[id]}B");	
}

static void FormatYmdString(StringBuilder buff, Date date, uint case_index)
{
	YearMonthDay ymd;
	ConvertDateToYMD(date, &ymd);

	long args[] = {ymd.day + STR_DAY_NUMBER_1ST - 1, STR_MONTH_ABBREV_JAN + ymd.month, ymd.year};
	var tmp_params = new StringParameters(args);
	FormatString(buff, GetStringPtr(STR_FORMAT_DATE_LONG), tmp_params, case_index);
}

static void FormatMonthAndYear(StringBuilder buff, Date date, uint case_index)
{
	YearMonthDay ymd;
	ConvertDateToYMD(date, &ymd);

	long args[] = {STR_MONTH_JAN + ymd.month, ymd.year};
	var tmp_params = new StringParameters(args);
	FormatString(buff, GetStringPtr(STR_FORMAT_DATE_SHORT), tmp_params, case_index);
}

static void FormatTinyOrISODate(StringBuilder buff, Date date, StringID str)
{
	YearMonthDay ymd;
	ConvertDateToYMD(date, &ymd);

	char day[3];
	char month[3];
	/* We want to zero-pad the days and months */
	seprintf(day,   lastof(day),   "%02i", ymd.day);
	seprintf(monthof(month), "%02i", ymd.month + 1);

	long args[] = {(long)(size_t)day, (long)(size_t)month, ymd.year};
	var tmp_params = new StringParameters(args);
	FormatString(buff, GetStringPtr(str), tmp_params);
}

static void FormatGenericCurrency(StringBuilder buff, CurrencySpec spec, Money number, bool compact)
{
	/* We are going to make number absolute for printing, so
	 * keep this piece of data as we need it later on */
	bool negative = number < 0;
	var multiplier = "";

	number *= spec.rate;

	/* convert from negative */
	if (number < 0) {		
		buff.AppendAsUtfChar((int)StringControlCode.SCC_RED);
		buff.Append("-");
		number = -number;
	}

	/* Add prefix part, following symbol_pos specification.
	 * Here, it can can be either 0 (prefix) or 2 (both prefix and suffix).
	 * The only remaining value is 1 (suffix), so everything that is not 1 */
    if (spec.symbol_pos != 1)
    {
        buff.Append(spec.prefix);
    }

	/* for huge numbers, compact the number into k or M */
	if (compact) {
		/* Take care of the 'k' rounding. Having 1 000 000 k
		 * and 1 000 M is inconsistent, so always use 1 000 M. */
		if (number >= 1000000000 - 500) {
			number = (number + 500000) / 1000000;
			multiplier = StringConstants.NBSP + "M";
		} else if (number >= 1000000) {
			number = (number + 500) / 1000;
			multiplier = StringConstants.NBSP + "k";
		}
	}

	var separator = _settings_game.locale.digit_group_separator_currency;
    if (separator == null)
    {
        separator = string.IsNullOrEmpty(_currency.separator)
            ? _langpack.digit_group_separator_currency
            : _currency.separator;
    }
	FormatNumber(buff, number, separator);
	buff.Append(multiplier);

	/* Add suffix part, following symbol_pos specification.
	 * Here, it can can be either 1 (suffix) or 2 (both prefix and suffix).
	 * The only remaining value is 1 (prefix), so everything that is not 0 */
    if (spec.symbol_pos != 0)
    {
        buff.Append(spec.suffix);
    }

	if (negative) {
		buff.AppendAsUtfChar((int)StringControlCode.SCC_PREVIOUS_COLOUR);
	}
}

/**
 * Determine the "plural" index given a plural form and a number.
 * @param count       The number to get the plural index of.
 * @param plural_form The plural form we want an index for.
 * @return The plural index for the given form.
 */
static int DeterminePluralForm(long count, int plural_form)
{
	/* The absolute value determines plurality */
	ulong n = (ulong)Math.Abs(count);

	switch (plural_form) {
		default:
			throw new NotReachedException();

		/* Two forms: singular used for one only.
		 * Used in:
		 *   Danish, Dutch, English, German, Norwegian, Swedish, Estonian, Finnish,
		 *   Greek, Hebrew, Italian, Portuguese, Spanish, Esperanto */
		case 0:
			return n != 1 ? 1 : 0;

		/* Only one form.
		 * Used in:
		 *   Hungarian, Japanese, Korean, Turkish */
		case 1:
			return 0;

		/* Two forms: singular used for 0 and 1.
		 * Used in:
		 *   French, Brazilian Portuguese */
		case 2:
			return n > 1 ? 1 : 0;

		/* Three forms: special cases for 0, and numbers ending in 1 except when ending in 11.
		 * Note: Cases are out of order for hysterical reasons. '0' is last.
		 * Used in:
		 *   Latvian */
		case 3:
			return n % 10 == 1 && n % 100 != 11 ? 0 : n != 0 ? 1 : 2;

		/* Five forms: special cases for 1, 2, 3 to 6, and 7 to 10.
		 * Used in:
		 *   Gaelige (Irish) */
		case 4:
			return n == 1 ? 0 : n == 2 ? 1 : n < 7 ? 2 : n < 11 ? 3 : 4;

		/* Three forms: special cases for numbers ending in 1 except when ending in 11, and 2 to 9 except when ending in 12 to 19.
		 * Used in:
		 *   Lithuanian */
		case 5:
			return n % 10 == 1 && n % 100 != 11 ? 0 : n % 10 >= 2 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2;

		/* Three forms: special cases for numbers ending in 1 except when ending in 11, and 2 to 4 except when ending in 12 to 14.
		 * Used in:
		 *   Croatian, Russian, Ukrainian */
		case 6:
			return n % 10 == 1 && n % 100 != 11 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2;

		/* Three forms: special cases for 1, and numbers ending in 2 to 4 except when ending in 12 to 14.
		 * Used in:
		 *   Polish */
		case 7:
			return n == 1 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2;

		/* Four forms: special cases for numbers ending in 01, 02, and 03 to 04.
		 * Used in:
		 *   Slovenian */
		case 8:
			return n % 100 == 1 ? 0 : n % 100 == 2 ? 1 : n % 100 == 3 || n % 100 == 4 ? 2 : 3;

		/* Two forms: singular used for numbers ending in 1 except when ending in 11.
		 * Used in:
		 *   Icelandic */
		case 9:
			return n % 10 == 1 && n % 100 != 11 ? 0 : 1;

		/* Three forms: special cases for 1, and 2 to 4
		 * Used in:
		 *   Czech, Slovak */
		case 10:
			return n == 1 ? 0 : n >= 2 && n <= 4 ? 1 : 2;

		/* Two forms: cases for numbers ending with a consonant, and with a vowel.
		 * Korean doesn't have the concept of plural, but depending on how a
		 * number is pronounced it needs another version of a particle.
		 * As such the plural system is misused to give this distinction.
		 */
		case 11:
			switch (n % 10) {
				case 0: // yeong
				case 1: // il
				case 3: // sam
				case 6: // yuk
				case 7: // chil
				case 8: // pal
					return 0;

				case 2: // i
				case 4: // sa
				case 5: // o
				case 9: // gu
					return 1;

				default:
				    throw new NotReachedException();
            }

		/* Four forms: special cases for 1, 0 and numbers ending in 02 to 10, and numbers ending in 11 to 19.
		 * Used in:
		 *  Maltese */
		case 12:
			return (n == 1 ? 0 : n == 0 || (n % 100 > 1 && n % 100 < 11) ? 1 : (n % 100 > 10 && n % 100 < 20) ? 2 : 3);
		/* Four forms: special cases for 1 and 11, 2 and 12, 3 .. 10 and 13 .. 19, other
		 * Used in:
		 *  Scottish Gaelic */
		case 13:
			return ((n == 1 || n == 11) ? 0 : (n == 2 || n == 12) ? 1 : ((n > 2 && n < 11) || (n > 12 && n < 20)) ? 2 : 3);
	}
}

static const char *ParseStringChoice(const char *b, uint form, char **dst)
{
	/* <NUM> {Length of each string} {each string} */
	uint n = (byte)*b++;
	uint pos, i, mypos = 0;

	for (i = pos = 0; i != n; i++) {
		uint len = (byte)*b++;
		if (i == form) mypos = pos;
		pos += len;
	}

	*dst += seprintf(*dst, "%s", b + mypos);
	return b + pos;
}

/** Helper for unit conversion. */
struct UnitConversion {
	/// Amount to multiply upon conversion.
	public int multiplier;    /// Amount to shift upon conversion.
	public int shift;

    public UnitConversion(int multiplier, int shift)
    {
        this.multiplier = multiplier;
        this.shift = shift;
    }
	
	/**
	 * Convert value from OpenTTD's internal unit into the displayed value.
	 * @param input The input to convert.
	 * @param round Whether to round the value or not.
	 * @return The converted value.
	 */
	public long ToDisplay(long input, bool round = true)
	{
		return ((input * this.multiplier) + (round && this.shift != 0 ? 1 << (this.shift - 1) : 0)) >> this.shift;
	}

	/**
	 * Convert the displayed value back into a value of OpenTTD's internal unit.
	 * @param input The input to convert.
	 * @param round Whether to round the value up or not.
	 * @param divider Divide the return value by this.
	 * @return The converted value.
	 */
	public long FromDisplay(long input, bool round = true, long divider = 1) 
	{
		return ((input << this.shift) + (round ? (this.multiplier * divider) - 1 : 0)) / (this.multiplier * divider);
	}
};

/** Information about a specific unit system. */
struct Units {
	/// Conversion
	public UnitConversion c;/// String for the unit 
	public StringID s;

    public Units(UnitConversion c, StringID s)
    {
        this.c = c;
        this.s = s;
    }
};

/** Information about a specific unit system with a long variant. */
struct UnitsLong {   /// Conversion
	public UnitConversion c;/// String for the short variant of the unit 
	public StringID s;      /// String for the long variant of the unit 
	public StringID l;

    public UnitsLong(UnitConversion c, StringID s, StringID l)
    {
        this.c = c;
        this.s = s;
        this.l = l;
    }
};

/** Unit conversions for velocity. */
static readonly Units[] _units_velocity = {
	new Units(new UnitConversion(1,0), STR_UNITS_VELOCITY_IMPERIAL ),
	new Units(new UnitConversion( 103,  6), STR_UNITS_VELOCITY_METRIC   ),
    new Units(new UnitConversion(1831, 12), STR_UNITS_VELOCITY_SI       ),
};

/** Unit conversions for velocity. */
static readonly Units[] _units_power = {
	new Units(new UnitConversion(   1,  0), STR_UNITS_POWER_IMPERIAL ),
	new Units(new UnitConversion(4153, 12), STR_UNITS_POWER_METRIC   ),
new Units(new UnitConversion(6109, 13), STR_UNITS_POWER_SI       ),
};

/** Unit conversions for weight. */
static readonly UnitsLong[] _units_weight = {
	new UnitsLong(new UnitConversion(4515, 12), STR_UNITS_WEIGHT_SHORT_IMPERIAL, STR_UNITS_WEIGHT_LONG_IMPERIAL ),
	new UnitsLong(new UnitConversion(   1,  0), STR_UNITS_WEIGHT_SHORT_METRIC,   STR_UNITS_WEIGHT_LONG_METRIC   ),
	new UnitsLong(new UnitConversion(1000,  0), STR_UNITS_WEIGHT_SHORT_SI,       STR_UNITS_WEIGHT_LONG_SI       ),
};

/** Unit conversions for volume. */
static readonly UnitsLong[] _units_volume = {
	new UnitsLong(new UnitConversion(4227,  4), STR_UNITS_VOLUME_SHORT_IMPERIAL, STR_UNITS_VOLUME_LONG_IMPERIAL ),
	new UnitsLong(new UnitConversion(1000,  0), STR_UNITS_VOLUME_SHORT_METRIC,   STR_UNITS_VOLUME_LONG_METRIC   ),
	new UnitsLong(new UnitConversion(   1,  0), STR_UNITS_VOLUME_SHORT_SI,       STR_UNITS_VOLUME_LONG_SI       ),
};

/** Unit conversions for force. */
static readonly Units[] _units_force = {
	new Units(new UnitConversion(3597,  4), STR_UNITS_FORCE_IMPERIAL ),
	new Units(new UnitConversion(3263,  5), STR_UNITS_FORCE_METRIC   ),
	new Units(new UnitConversion(   1,  0), STR_UNITS_FORCE_SI       ),
};

/** Unit conversions for height. */
static readonly Units[] _units_height = {
	new Units(new UnitConversion(   3,  0), STR_UNITS_HEIGHT_IMPERIAL ), // "Wrong" conversion factor for more nicer GUI values
	new Units(new UnitConversion(   1,  0), STR_UNITS_HEIGHT_METRIC   ),
	new Units(new UnitConversion(   1,  0), STR_UNITS_HEIGHT_SI       ),
};

/**
 * Convert the given (internal) speed to the display speed.
 * @param speed the speed to convert
 * @return the converted speed.
 */
uint ConvertSpeedToDisplaySpeed(uint speed)
{
	/* For historical reasons we don't want to mess with the
	 * conversion for speed. So, don't round it and keep the
	 * original conversion factors instead of the real ones. */
	return _units_velocity[_settings_game.locale.units_velocity].c.ToDisplay(speed, false);
}

/**
 * Convert the given display speed to the (internal) speed.
 * @param speed the speed to convert
 * @return the converted speed.
 */
uint ConvertDisplaySpeedToSpeed(uint speed)
{
	return _units_velocity[_settings_game.locale.units_velocity].c.FromDisplay(speed);
}

/**
 * Convert the given km/h-ish speed to the display speed.
 * @param speed the speed to convert
 * @return the converted speed.
 */
uint ConvertKmhishSpeedToDisplaySpeed(uint speed)
{
	return _units_velocity[_settings_game.locale.units_velocity].c.ToDisplay(speed * 10, false) / 16;
}

/**
 * Convert the given display speed to the km/h-ish speed.
 * @param speed the speed to convert
 * @return the converted speed.
 */
uint ConvertDisplaySpeedToKmhishSpeed(uint speed)
{
	return _units_velocity[_settings_game.locale.units_velocity].c.FromDisplay(speed * 16, true, 10);
}

public  enum ReadIntState
{
            LeadingWhiteSpace,
			Sign,
			Zero,
			X,
			Digits,
            TrailingWhiteSpace
        }

        private long ReadHexInt(IndexedString enumerator)
        {
            //tries to mimic c style strtol

            var builder = new StringBuilder();
            var state = ReadIntState.LeadingWhiteSpace;            

            while (enumerator.HasContent)
            {
                var c = enumerator.Current;
			switch (state)
                {
                    case ReadIntState.LeadingWhiteSpace:
                        if (char.IsWhiteSpace(c))
                        {
                            //skip whitespace
                            enumerator.MoveNext();
                        }
                        else
                        {
							//reparse current as sign
                            state = ReadIntState.Sign;
                        }
					
                        break;
                    case ReadIntState.Sign:
                        if (c == '+' || c == '-')
                        {
                            builder.Append(c);
                            enumerator.MoveNext();
                        }
						//else no sign, reparse current as sign 0                            
                        
                        state = ReadIntState.Zero;
                        break;
                    case ReadIntState.Zero:
                        if (c == '0')
                        {
                            builder.Append(c);
                            enumerator.MoveNext();
                            state = ReadIntState.X;
						}
                        else
                        {
                            //else no 0x, reparse current as digits
                            state = ReadIntState.Digits;
                        }
                        break;
                    case ReadIntState.X:
                        if (c == 'x' || c == 'X')
                        {
                            builder.Append(c);
                            enumerator.MoveNext();
                        }
                        //else no x, reparse current as digits                            
                        state = ReadIntState.Digits;
                break;
                    case ReadIntState.Digits:
                        if ((c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || (c >= '0' && c <= '9'))
                        {
                            builder.Append(c);
                            enumerator.MoveNext();
                        }
                        else
                        {
                            state = ReadIntState.TrailingWhiteSpace;
                        }
                break;
                    case ReadIntState.TrailingWhiteSpace:
                        if (char.IsWhiteSpace(c))
                        {
                            //skip whitespace
                            enumerator.MoveNext();
                        }
                        else
                        {
                            //reparse current as sign
                            
                        }
                break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }                                
            }

            if (state < ReadIntState.Digits)
            {
				return 0;
            }
            return Convert.ToInt64(builder.ToString(), 16);
        }

/**
 * Parse most format codes within a string and write the result to a buffer.
 * @param buff  The buffer to write the final string to.
 * @param str   The original string with format codes.
 * @param args  Pointer to extra arguments used by various string codes.
 * @param case_index
 * @param last  Pointer to just past the end of the buff array.
 * @param dry_run True when the argt array is not yet initialized.
 */
public static FormatString(StringBuilder buff, string str_arg, StringParameters args, uint case_index, bool game_script, bool dry_run)
{
	uint orig_offset = args.offset;

	/* When there is no array with types there is no need to do a dry run. */
	if (args.HasTypeInformation() && !dry_run) {
		if (UsingNewGRFTextStack()) {
			/* Values from the NewGRF text stack are only copied to the normal
			 * argv array at the time they are encountered. That means that if
			 * another string command references a value later in the string it
			 * would fail. We solve that by running FormatString twice. The first
			 * pass makes sure the argv array is correctly filled and the second
			 * pass can reference later values without problems. */
			struct TextRefStack backup = CreateTextRefStackBackup();
			FormatString(buff, str_arg, args, case_index, game_script, true);
			RestoreTextRefStackBackup(backup);
		} else {
			FormatString(buff, str_arg, args, case_index, game_script, true);
		}
		/* We have to restore the original offset here to to read the correct values. */
		args.offset = orig_offset;
	}

	uint next_substr_case_index = 0;
	//char *buf_start = buff;
    var str_stack = new Stack<IndexedString>();
	str_stack.Push(new StringEnumerator(str_arg));

	for (;;)
	{
	    IndexedString str;
		while (str_stack.Any())
		{	
			str = str_stack.Peek();
		    if (str.HasContent == false || str.Current == '\0')
		    {
		        str_stack.Pop();
		    }
		    else
		    {
		        break;
		    }
		}
	    if (str_stack.Any() == false)
	    {
	        break;
	    }
	    var b = str.ConsumeUtfChar();

		if (StringControlCode.SCC_NEWGRF_FIRST <= b && b <= StringControlCode.SCC_NEWGRF_LAST) {
			/* We need to pass some stuff as it might be modified; oh boy. */
			//todo: should argve be passed here too?
			//TODO b = RemapNewGRFStringControlCode(b, buf_start, buff, ref str, (long *)args.GetDataPointer(), args.GetDataLeft(), dry_run);
			if (b == 0) {continue;}
		}

	
		switch (b) {
			case StringControlCode.SCC_ENCODED: {
				var sub_args_data = new ulong[20];
				var sub_args_type = new char[20];
				var sub_args = new StringParameters(sub_args_data, sub_args_type);

				sub_args.ClearTypeInformation();

			    var s = str.Clone();
			    var p = str.Clone();
                var stringid = ReadHexInt(p);
			    
				if (p.HasContent == false || (p.Current != ':' && p.Current != '\0'))
				{
				    p.SkipUntil('\0');
				    str.Index = p.Index;
					buff.Append("(invalid SCC_ENCODED)");
					break;
				}
				if (stringid >= Language.TAB_SIZE)
				{				    
				    p.SkipUntil('\0');
				    str.Index = p.Index;
                    buff.Append("(invalid StringID)");
					break;
				}

			    int i = 0;
				while (p.HasContent && p.Current != '\0' && i < 20) {
					ulong param;
				    s.Index = p.Index;
					/* Find the next value */
					bool instring = false;
					bool escape = false;
					for (;p.HasContent;p.MoveNext())
					{
						if (p.Current == '\\') {
							escape = true;
							continue;
						}
						if (p.Current == '"' && escape) {
							escape = false;
							continue;
						}
						escape = false;

						if (p.Current == '"') {
							instring = !instring;
							continue;
						}
						if (instring) {
							continue;
						}

					    if (p.Current == ':')
					    {
					        break;
					    }
					    if (p.Current == '\0')
					    {
					        break;
					    }
					}

					if (s.Current != '"') {
						/* Check if we want to look up another string */
						var l = s.DecodeUtfChar();					    
						bool lookup = (l == StringControlCode.SCC_ENCODED);
					    param = ReadHexInt(s);

						if (lookup) {
							if (param >= Language.TAB_SIZE)
							{
							    p.SkipUntil('\0');
							    str.Index = p.Index;
								buff.Append("(invalid sub-StringID)");
								break;
							}
						    param = MakeStringID(Language.TEXT_TAB_GAMESCRIPT_START, param);
						}

						sub_args.SetParam(i++, param);
					} else
					{
					    var g = str.Text.Substring(s.Index, p.Index - s.Index - 1);
						sub_args.SetParam(i++, (ulong)g);
					}
				}
				/* If we didn't error out, we can actually print the string. */
				if (str.HasContent && str.Current != '\0')
				{
				    str.Index = p.Index;
				    GetStringWithArgs(buff, MakeStringID(Language.TEXT_TAB_GAMESCRIPT_START, stringid), sub_args, true);
				}
				break;
			}

			case StringControlCode.SCC_NEWGRF_STRINL: {
				StringID substr = (StringID)(ushort)str.ConsumeUtfChar();
				str.MoveNext();
				str_stack.Push(GetStringPtr(substr));
				break;
			}

			case StringControlCode.SCC_NEWGRF_PRINT_WORD_STRING_ID: {
				StringID substr = (ushort)args.GetInt32(StringControlCode.SCC_NEWGRF_PRINT_WORD_STRING_ID);
				str_stack.Push(GetStringPtr(substr));
				case_index = next_substr_case_index;
				next_substr_case_index = 0;
				break;
			}


			case StringControlCode.SCC_GENDER_LIST: { // {G 0 Der Die Das}
				/* First read the meta data from the language file. */			    
				uint offset = orig_offset + (byte)str.Current;
			    str.MoveNext();
				int gender = 0;
				if (!dry_run && args.GetTypeAtOffset(offset) != 0) {
					/* Now we need to figure out what text to resolve, i.e.
					 * what do we need to draw? So get the actual raw string
					 * first using the control code to get said string. */
					var input = args.GetTypeAtOffset(offset)); //utf8encode

					/* Now do the string formatting. */
				    var buf = new StringBuilder();
					bool old_sgd = _scan_for_gender_data;
					_scan_for_gender_data = true;
					var tmp_params = new StringParameters(args.GetPointerToOffset(offset), args.num_param - offset, null);
					FormatString(buf, input, tmp_params);
					_scan_for_gender_data = old_sgd;

					/* And determine the string. */
				    var c = buf[0]; //Utf8Consume
					/* Does this string have a gender, if so, set it */
				    if (c == StringControlCode.SCC_GENDER_INDEX)
				    {
				        gender = (byte)buf[0];
				    }
				}
				ParseStringChoice(str, gender, buff);
				break;
			}

			/* This sets up the gender for the string.
			 * We just ignore this one. It's used in {G 0 Der Die Das} to determine the case. */
			case StringControlCode.SCC_GENDER_INDEX: // {GENDER 0}
			    if (_scan_for_gender_data)
			    {
			        buff.AppendAsUtfChar((int)StringControlCode.SCC_GENDER_INDEX);
			        buff.Append(str.Current);
			    }
			    str.MoveNext();

				break;

			case StringControlCode.SCC_PLURAL_LIST: { // {P}
				int plural_form = (int)str.Current;          // contains the plural form for this string
			    str.MoveNext();
				uint offset = orig_offset + (byte)str.Current;
			    str.MoveNext();
				long v = args.GetPointerToOffset(offset); // contains the number that determines plural
				ParseStringChoice(str, DeterminePluralForm(v, plural_form), buff);
				break;
			}

			case StringControlCode.SCC_ARG_INDEX: { // Move argument pointer
				args.offset = orig_offset + (byte) str.Current;
			    str.MoveNext();
				break;
			}

			case StringControlCode.SCC_SET_CASE: { // {SET_CASE}
				/* This is a pseudo command, it's outputted when someone does {STRING.ack}
				 * The modifier is added to all subsequent GetStringWithArgs that accept the modifier. */
				next_substr_case_index = (byte) str.Current;
			    str.MoveNext();
				break;
			}

			case StringControlCode.SCC_SWITCH_CASE: { // {Used to implement case switching}
				/* <0x9E> <NUM CASES> <CASE1> <LEN1> <STRING1> <CASE2> <LEN2> <STRING2> <CASE3> <LEN3> <STRING3> <STRINGDEFAULT>
				 * Each LEN is printed using 2 bytes in big endian order. */
				uint num = (byte)str.Current;
			    str.MoveNext();
				while (num != 0) {
					if ((byte) str.Current == case_index) {
						/* Found the case, adjust str pointer and continue */
						str.MoveNext(3);
						break;
					}
				    var i = str.Index;
					/* Otherwise skip to the next case */
				    str.MoveNext(3 + (str.Text[1+i] << 8) + str.Text[2+i];
					num--;
				}
				break;
			}

			case StringControlCode.SCC_REVISION: // {REV}
				buff.Append(_openttd_revision);
				break;

			case StringControlCode.SCC_RAW_STRING_POINTER: { // {RAW_STRING}
				if (game_script) break;
				var otherStr = (string)args.Getlong(StringControlCode.SCC_RAW_STRING_POINTER); 
				FormatString(buff, otherStr, args);
				break;
			}

			case StringControlCode.SCC_STRING: {// {STRING}
				StringID stringId = args.GetInt32(StringControlCode.SCC_STRING);
				if (game_script && BitMath.GB(stringId, Language.TAB_COUNT_OFFSET, Language.TAB_COUNT_BITS) != GAME_TEXT_TAB) break;
				/* WARNING. It's prohibited for the included string to consume any arguments.
				 * For included strings that consume argument, you should use STRING1, STRING2 etc.
				 * To debug stuff you can set argv to null and it will tell you */
				var tmp_params = new StringParameters(args.GetDataPointer(), args.GetDataLeft(), null);
				GetStringWithArgs(buff, stringId, tmp_params, next_substr_case_index, game_script);
				next_substr_case_index = 0;
				break;
			}

			case StringControlCode.SCC_STRING1:
			case StringControlCode.SCC_STRING2:
			case StringControlCode.SCC_STRING3:
			case StringControlCode.SCC_STRING4:
			case StringControlCode.SCC_STRING5:
			case StringControlCode.SCC_STRING6:
			case StringControlCode.SCC_STRING7: { // {STRING1..7}
				/* Strings that consume arguments */
				StringID str = args.GetInt32(b);
				if (game_script && BitMath.GB(str, Language.TAB_COUNT_OFFSET, Language.TAB_COUNT_BITS) != GAME_TEXT_TAB) break;
				uint size = b - StringControlCode.SCC_STRING1 + 1;
				if (game_script && size > args.GetDataLeft()) {
					buff.Append("(too many parameters)");
				} else {
					var sub_args = new StringParameters(args, size);
					GetStringWithArgs(buff, str, sub_args, next_substr_case_index, game_script);
				}
				next_substr_case_index = 0;
				break;
			}

			case StringControlCode.SCC_COMMA: // {COMMA}
				FormatCommaNumber(buff, args.Getlong(StringControlCode.SCC_COMMA));
				break;

			case StringControlCode.SCC_DECIMAL: {// {DECIMAL}
				long number = args.Getlong(StringControlCode.SCC_DECIMAL);
				int digits = args.GetInt32(StringControlCode.SCC_DECIMAL);
				FormatCommaNumber(buff, number, digits);
				break;
			}

			case StringControlCode.SCC_NUM: // {NUM}
				FormatNoCommaNumber(buff, args.Getlong(StringControlCode.SCC_NUM));
				break;

			case StringControlCode.SCC_ZEROFILL_NUM: { // {ZEROFILL_NUM}
				long num = args.Getlong();
				FormatZerofillNumber(buff, num, args.Getlong());
				break;
			}

			case StringControlCode.SCC_HEX: // {HEX}
				FormatHexNumber(buff, (ulong)args.Getlong(StringControlCode.SCC_HEX));
				break;

			case StringControlCode.SCC_BYTES: // {BYTES}
				FormatBytes(buff, args.Getlong());
				break;

			case StringControlCode.SCC_CARGO_TINY: { // {CARGO_TINY}
				/* Tiny description of cargotypes. Layout:
				 * param 1: cargo type
				 * param 2: cargo count */
				CargoID cargo = args.GetInt32(StringControlCode.SCC_CARGO_TINY);
				if (cargo >= CargoSpec.GetArraySize()) break;

				StringID cargo_str = CargoSpec.Get(cargo).units_volume;
				long amount = 0;
				switch (cargo_str) {
					case STR_TONS:
						amount = _units_weight[_settings_game.locale.units_weight].c.ToDisplay(args.Getlong());
						break;

					case STR_LITERS:
						amount = _units_volume[_settings_game.locale.units_volume].c.ToDisplay(args.Getlong());
						break;

					default: {
						amount = args.Getlong();
						break;
					}
				}

				FormatCommaNumber(buff, amount);
				break;
			}

			case StringControlCode.SCC_CARGO_SHORT: { // {CARGO_SHORT}
				/* Short description of cargotypes. Layout:
				 * param 1: cargo type
				 * param 2: cargo count */
				CargoID cargo = args.GetInt32(StringControlCode.SCC_CARGO_SHORT);
				if (cargo >= CargoSpec.GetArraySize()) break;

				StringID cargo_str = CargoSpec.Get(cargo).units_volume;
				switch (cargo_str) {
					case STR_TONS: {
						Debug.Assert(_settings_game.locale.units_weight < _units_weight.Length));
						var args_array = new long[] {_units_weight[_settings_game.locale.units_weight].c.ToDisplay(args.Getlong())};
						var tmp_params = new StringParameters(args_array);
						FormatString(buff, GetStringPtr(_units_weight[_settings_game.locale.units_weight].l), tmp_params);
						break;
					}

					case STR_LITERS: {
					    Debug.Assert(_settings_game.locale.units_volume < _units_volume.Length);
						var args_array = new long[] {_units_volume[_settings_game.locale.units_volume].c.ToDisplay(args.Getlong())};
var tmp_params = new StringParameters(args_array);

                        FormatString(buff, GetStringPtr(_units_volume[_settings_game.locale.units_volume].l), tmp_params);
						break;
					}

					default: {
						var tmp_params = new StringParameters(args, 1);
						 GetStringWithArgs(buff, cargo_str, tmp_params);
						break;
					}
				}
				break;
			}

			case StringControlCode.SCC_CARGO_LONG: { // {CARGO_LONG}
				/* First parameter is cargo type, second parameter is cargo count */
				CargoID cargo = args.GetInt32(StringControlCode.SCC_CARGO_LONG);
				if (cargo != CargoTypes.CT_INVALID && cargo >= CargoSpec.GetArraySize()) break;

				StringID cargo_str = (cargo == CargoTypes.CT_INVALID) ? STR_QUANTITY_N_A : CargoSpec.Get(cargo).quantifier;
				var tmp_args = new StringParameters(args, 1);
				GetStringWithArgs(buff, cargo_str, &tmp_args);
				break;
			}

			case StringControlCode.SCC_CARGO_LIST: { // {CARGO_LIST}
				uint cmask = args.GetInt32(StringControlCode.SCC_CARGO_LIST);
				bool first = true;

			    for (var index = 0; index<_sorted_cargo_specs_size; index++)
			    {
			        var cs = _sorted_cargo_specs[index];
					if (!BitMath.HasBit(cmask, cs.Index())) continue;

					//if (buff >= last - 2) break; // ',' and ' '

					if (first) {
						first = false;
					} else {
						/* Add a comma if this is not the first item */
						buff.Append(", ");
					}

					GetStringWithArgs(buff, cs.name, args, next_substr_case_index, game_script);
				}

				/* If first is still true then no cargo is accepted */
			    if (first)
			    {
			        GetStringWithArgs(buff, STR_JUST_NOTHING, args, next_substr_case_index, game_script);
			    }

				//*buff = '\0';
				next_substr_case_index = 0;

				break;
			}

			case StringControlCode.SCC_CURRENCY_SHORT: // {CURRENCY_SHORT}
				FormatGenericCurrency(buff, _currency, args.Getlong(), true);
				break;

			case StringControlCode.SCC_CURRENCY_LONG: // {CURRENCY_LONG}
				FormatGenericCurrency(buff, _currency, args.Getlong(StringControlCode.SCC_CURRENCY_LONG), false);
				break;

			case StringControlCode.SCC_DATE_TINY: // {DATE_TINY}
				FormatTinyOrISODate(buff, args.GetInt32(StringControlCode.SCC_DATE_TINY), STR_FORMAT_DATE_TINY);
				break;

			case StringControlCode.SCC_DATE_SHORT: // {DATE_SHORT}
				FormatMonthAndYear(buff, args.GetInt32(StringControlCode.SCC_DATE_SHORT), next_substr_case_index);
				next_substr_case_index = 0;
				break;

			case StringControlCode.SCC_DATE_LONG: // {DATE_LONG}
				FormatYmdString(buff, args.GetInt32(StringControlCode.SCC_DATE_LONG), next_substr_case_index);
				next_substr_case_index = 0;
				break;

			case StringControlCode.SCC_DATE_ISO: // {DATE_ISO}
				FormatTinyOrISODate(buff, args.GetInt32(), STR_FORMAT_DATE_ISO);
				break;

			case StringControlCode.SCC_FORCE: { // {FORCE}
				Debug.Assert(_settings_game.locale.units_force < _units_force.Length);
				var args_array = new long[] {_units_force[_settings_game.locale.units_force].c.ToDisplay(args.Getlong())};
				var tmp_params = new StringParameters(args_array);
				FormatString(buff, GetStringPtr(_units_force[_settings_game.locale.units_force].s), tmp_params);
				break;
			}

			case StringControlCode.SCC_HEIGHT: { // {HEIGHT}
				Debug.Assert(_settings_game.locale.units_height < _units_height.Length);
			    var args_array = new long[] { _units_height[_settings_game.locale.units_height].c.ToDisplay(args.Getlong())};
var tmp_params = new StringParameters(args_array);

                FormatString(buff, GetStringPtr(_units_height[_settings_game.locale.units_height].s), tmp_params);
				break;
			}

			case StringControlCode.SCC_POWER: { // {POWER}
				Debug.Assert(_settings_game.locale.units_power < _units_power.Length);
			    var args_array = new long[] { _units_power[_settings_game.locale.units_power].c.ToDisplay(args.Getlong())};
var tmp_params = new StringParameters(args_array);

                FormatString(buff, GetStringPtr(_units_power[_settings_game.locale.units_power].s), tmp_params);
				break;
			}

			case StringControlCode.SCC_VELOCITY: { // {VELOCITY}
				Debug.Assert(_settings_game.locale.units_velocity < _units_velocity.Length);
			    var args_array = new long[] { ConvertKmhishSpeedToDisplaySpeed(args.Getlong(StringControlCode.SCC_VELOCITY))};
var tmp_params = new StringParameters(args_array);

                FormatString(buff, GetStringPtr(_units_velocity[_settings_game.locale.units_velocity].s),tmp_params);
				break;
			}

			case StringControlCode.SCC_VOLUME_SHORT: { // {VOLUME_SHORT}
				Debug.Assert(_settings_game.locale.units_volume < _units_volume.Length);
			    var args_array = new long[] { _units_volume[_settings_game.locale.units_volume].c.ToDisplay(args.Getlong())};
var tmp_params = new StringParameters(args_array);

                FormatString(buff, GetStringPtr(_units_volume[_settings_game.locale.units_volume].s), tmp_params);
				break;
			}

			case StringControlCode.SCC_VOLUME_LONG: { // {VOLUME_LONG}
				Debug.Assert(_settings_game.locale.units_volume < _units_volume.Length);
			    var args_array = new long[] { _units_volume[_settings_game.locale.units_volume].c.ToDisplay(args.Getlong(StringControlCode.SCC_VOLUME_LONG))};
var tmp_params = new StringParameters(args_array);

                FormatString(buff, GetStringPtr(_units_volume[_settings_game.locale.units_volume].l), tmp_params);
				break;
			}

			case StringControlCode.SCC_WEIGHT_SHORT: { // {WEIGHT_SHORT}
				Debug.Assert(_settings_game.locale.units_weight < _units_weight.Length);
			    var args_array = new long[] { _units_weight[_settings_game.locale.units_weight].c.ToDisplay(args.Getlong())};
var tmp_params = new StringParameters(args_array);

                FormatString(buff, GetStringPtr(_units_weight[_settings_game.locale.units_weight].s), tmp_params);
				break;
			}

			case StringControlCode.SCC_WEIGHT_LONG: { // {WEIGHT_LONG}
				Debug.Assert(_settings_game.locale.units_weight < _units_weight.Length);
			    var args_array = new long[] { _units_weight[_settings_game.locale.units_weight].c.ToDisplay(args.Getlong(StringControlCode.SCC_WEIGHT_LONG))};
var tmp_params = new StringParameters(args_array);

                FormatString(buff, GetStringPtr(_units_weight[_settings_game.locale.units_weight].l), tmp_params);
				break;
			}

			case StringControlCode.SCC_COMPANY_NAME: { // {COMPANY}
				var c = Company.GetIfValid(args.GetInt32());
				if (c == null) break;

				if (c.name != null) {
					var args_array = new long [] {(long)c.name};
var tmp_params = new StringParameters(args_array);

                    GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
				    var args_array = new long[] { c.name_2};
var tmp_params = new StringParameters(args_array);

                    GetStringWithArgs(buff, c.name_1, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_COMPANY_NUM: { // {COMPANY_NUM}
				var company = (CompanyID)args.GetInt32();

				/* Nothing is added for AI or inactive companies */
				if (Company.IsValidHumanID(company)) {
				    var args_array = new long[] { company + 1};
var tmp_params = new StringParameters(args_array);

                    GetStringWithArgs(buff, STR_FORMAT_COMPANY_NUM, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_DEPOT_NAME: { // {DEPOT}
				VehicleType vt = (VehicleType)args.GetInt32(StringControlCode.SCC_DEPOT_NAME);
				if (vt == VEH_AIRCRAFT) {
				    var args_array = new ulong[] { (ulong)args.GetInt32()};
					var types_array = new char[] {StringControlCode.SCC_STATION_NAME};
					var tmp_params = new StringParameters(args_array, 1, types_array);
					GetStringWithArgs(buff, STR_FORMAT_DEPOT_NAME_AIRCRAFT, tmp_params);
					break;
				}

				var d = Depot.Get(args.GetInt32());
				if (d.name != null) {
					var args_array = new long[] {(long)d.name};
				    var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					var args_array = new long[] {d.town.index, d.town_cn + 1};
				    var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_FORMAT_DEPOT_NAME_TRAIN + 2 * vt + (d.town_cn == 0 ? 0 : 1), tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_ENGINE_NAME: { // {ENGINE}
				var e = Engine.GetIfValid(args.GetInt32(StringControlCode.SCC_ENGINE_NAME));
				if (e == null) break;

				if (e.name != null && e.IsEnabled()) {
					var args_array = new long[] {(long)e.name};
				    var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
				    var tmp_params = new StringParameters(null, 0, null);
					GetStringWithArgs(buff, e.info.string_id, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_GROUP_NAME: { // {GROUP}
				var g = Group.GetIfValid(args.GetInt32());
				if (g == null) break;

				if (g.name != null) {
				    var args_array = new long[] { (long)g.name};
				    var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					var args_array = new long[] {g.index};
				    var tmp_params = new StringParameters(args_array);

					GetStringWithArgs(buff, STR_FORMAT_GROUP_NAME, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_INDUSTRY_NAME: { // {INDUSTRY}
				var i = Industry.GetIfValid(args.GetInt32(StringControlCode.SCC_INDUSTRY_NAME));
				if (i == null) break;

				if (_scan_for_gender_data) {
					/* Gender is defined by the industry type.
					 * STR_FORMAT_INDUSTRY_NAME may have the town first, so it would result in the gender of the town name */
					var tmp_params = new StringParameters(null, 0, null);
					FormatString(buff, GetStringPtr(GetIndustrySpec(i.type).name), tmp_params, next_substr_case_index);
				} else {
					/* First print the town name and the industry type name. */
					var args_array = new long[2] {i.town.index, GetIndustrySpec(i.type).name};
					var tmp_params = new StringParameters(args_array);

					FormatString(buff, GetStringPtr(STR_FORMAT_INDUSTRY_NAME), tmp_params, next_substr_case_index);
				}
				next_substr_case_index = 0;
				break;
			}

			case StringControlCode.SCC_PRESIDENT_NAME: { // {PRESIDENT_NAME}
				var c = Company.GetIfValid(args.GetInt32(StringControlCode.SCC_PRESIDENT_NAME));
				if (c == null) break;

				if (c.president_name != null) {
					var args_array = new long[] {(long)c.president_name};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					var args_array = new long[] {c.president_name_2};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, c.president_name_1, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_STATION_NAME: { // {STATION}
				StationID sid = args.GetInt32(StringControlCode.SCC_STATION_NAME);
				var st = Station.GetIfValid(sid);

				if (st == null) {
					/* The station doesn't exist anymore. The only place where we might
					 * be "drawing" an invalid station is in the case of cargo that is
					 * in transit. */
					var tmp_params = new StringParameters(null, 0, null);
					GetStringWithArgs(buff, STR_UNKNOWN_STATION, tmp_params);
					break;
				}

				if (st.name != null) {
					var args_array = new long[] {(long)st.name};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					StringID str = st.string_id;
					if (st.indtype != IT_INVALID) {
						/* Special case where the industry provides the name for the station */
						var indsp = GetIndustrySpec(st.indtype);

						/* Industry GRFs can change which might remove the station name and
						 * thus cause very strange things. Here we check for that before we
						 * actually set the station name. */
						if (indsp.station_name != STR_null && indsp.station_name != STR_UNDEFINED) {
							str = indsp.station_name;
						}
					}

					var args_array = new long[] {STR_TOWN_NAME, st.town.index, st.index};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, str, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_TOWN_NAME: { // {TOWN}
				var t = Town.GetIfValid(args.GetInt32(StringControlCode.SCC_TOWN_NAME));
				if (t == null) break;

				if (t.name != null) {
					var args_array = new long[] {(long)t.name};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					GetTownName(buff, t);
				}
				break;
			}

			case StringControlCode.SCC_WAYPOINT_NAME: { // {WAYPOINT}
				var wp = Waypoint.GetIfValid(args.GetInt32(StringControlCode.SCC_WAYPOINT_NAME));
				if (wp == null) break;

				if (wp.name != null) {
					var args_array = new long[] {(long)wp.name};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					var args_array = new long[] {wp.town.index, wp.town_cn + 1};
					var tmp_params = new StringParameters(args_array);
					StringID str = ((wp.string_id == STR_SV_STNAME_BUOY) ? STR_FORMAT_BUOY_NAME : STR_FORMAT_WAYPOINT_NAME);
					if (wp.town_cn != 0) str.MoveNext();
					GetStringWithArgs(buff, str, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_VEHICLE_NAME: { // {VEHICLE}
				var v = Vehicle.GetIfValid(args.GetInt32(StringControlCode.SCC_VEHICLE_NAME));
				if (v == null) break;

				if (v.name != null) {
					var args_array = new long[] {(long)v.name};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					var args_array = new long[] {v.unitnumber};
					var tmp_params = new StringParameters(args_array);

					StringID strindId;
					switch (v.type) {
						default:           strindId = STR_INVALID_VEHICLE; break;
						case VEH_TRAIN:    strindId = STR_SV_TRAIN_NAME; break;
						case VEH_ROAD:     strindId = STR_SV_ROAD_VEHICLE_NAME; break;
						case VEH_SHIP:     strindId = STR_SV_SHIP_NAME; break;
						case VEH_AIRCRAFT: strindId = STR_SV_AIRCRAFT_NAME; break;
					}

					GetStringWithArgs(buff, strindId, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_SIGN_NAME: { // {SIGN}
			    var si = Sign.GetIfValid(args.GetInt32());
				if (si == null) break;

				if (si.name != null) {
					var args_array = new long[] {(long)si.name};
					var tmp_params = new StringParameters(args_array);
					GetStringWithArgs(buff, STR_JUST_RAW_STRING, tmp_params);
				} else {
					var tmp_params = new StringParameters(null, 0, null);
					GetStringWithArgs(buff, STR_DEFAULT_SIGN_NAME, tmp_params);
				}
				break;
			}

			case StringControlCode.SCC_STATION_FEATURES: { // {STATIONFEATURES}
				buff = StationGetSpecialString(buff, args.GetInt32(StringControlCode.SCC_STATION_FEATURES));
				break;
			}

			default:
				buff.AppendAsUtfChar(b);
				break;
		}
	}
}


public static void StationGetSpecialString(StringBuilder buff, int x)
{
	if (x & FACIL_TRAIN)      buff.AppendAsUtfChar(StringControlCode.SCC_TRAIN);
	if (x & FACIL_TRUCK_STOP) buff.AppendAsUtfChar(StringControlCode.SCC_LORRY);
	if (x & FACIL_BUS_STOP)   buff.AppendAsUtfChar(StringControlCode.SCC_BUS);
	if (x & FACIL_DOCK)       buff.AppendAsUtfChar(StringControlCode.SCC_SHIP);
	if (x & FACIL_AIRPORT)    buff.AppendAsUtfChar(StringControlCode.SCC_PLANE);
}

public static void GetSpecialTownNameString(StringBuilder buff, int ind, uint seed)
{
	GenerateTownNameString(buff, ind, seed);
}

private static readonly string[] _silly_company_names = {
	"Bloggs Brothers",
	"Tiny Transport Ltd.",
	"Express Travel",
	"Comfy-Coach & Co.",
	"Crush & Bump Ltd.",
	"Broken & Late Ltd.",
	"Sam Speedy & Son",
	"Supersonic Travel",
	"Mike's Motors",
	"Lightning International",
	"Pannik & Loozit Ltd.",
	"Inter-City Transport",
	"Getout & Pushit Ltd."
};

        private static readonly string[] _surname_list = {
	"Adams",
	"Allan",
	"Baker",
	"Bigwig",
	"Black",
	"Bloggs",
	"Brown",
	"Campbell",
	"Gordon",
	"Hamilton",
	"Hawthorn",
	"Higgins",
	"Green",
	"Gribble",
	"Jones",
	"McAlpine",
	"MacDonald",
	"McIntosh",
	"Muir",
	"Murphy",
	"Nelson",
	"O'Donnell",
	"Parker",
	"Phillips",
	"Pilkington",
	"Quigley",
	"Sharkey",
	"Thomson",
	"Watkins"
};

        public static readonly string[] _silly_surname_list = {
	"Grumpy",
	"Dozy",
	"Speedy",
	"Nosey",
	"Dribble",
	"Mushroom",
	"Cabbage",
	"Sniffle",
	"Fishy",
	"Swindle",
	"Sneaky",
	"Nutkins"
};

        public static readonly char[] _initial_name_letters = {
	'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
	'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W',
};

static void GenAndCoName(StringBuilder buff, uint arg)
{
    char[] list;
	uint num;

	if (_settings_game.game_creation.landscape == LT_TOYLAND) {
		list = _silly_surname_list;
	} else {
		list = _surname_list;
	}

	buff.Append(list[list.Length * BitMath.GB(arg, 16, 8) >> 8]);
	buff.Append(" & Co.");
}

static void GenPresidentName(StringBuilder buff, uint x)
{
	buff.Append(_initial_name_letters[_initial_name_letters.Length] * BitMath.GB(x, 0, 8) >> 8]);
	buff.Append(". ");

	var i = (_initial_name_letters.Length + 35) * BitMath.GB(x, 8, 8) >> 8;
	if (i < _initial_name_letters.Length) {
		buff.Append(_initial_name_letters[i]);
	    buff.Append(". ");
	}

    char[] list;
    if (_settings_game.game_creation.landscape == LT_TOYLAND) {
		list = _silly_surname_list;
	} else {
		list = _surname_list;
	}

	buff.Append(list[list.Length * BitMath.GB(x, 16, 8) >> 8]);
}

static void GetSpecialNameString(StringBuilder buff, int ind, StringParameters args)
{
	switch (ind) {
		case 1: // not used
			buff.Append(_silly_company_names[Math.Min(args.GetInt32() & 0xFFFF, _silly_company_names.Length - 1)]);
            break;

		case 2: // used for Foobar & Co company names
			GenAndCoName(buff, args.GetInt32());
		    break;

		case 3: // President name
			GenPresidentName(buff, args.GetInt32());
		    break;
	}

	/* town name? */
	if (MathFuncs.IsInsideMM(ind - 6, 0, SPECSTR_TOWNNAME_LAST - SPECSTR_TOWNNAME_START + 1)) {
		buff = GetSpecialTownNameString(buff, ind - 6, args.GetInt32());
		buff.Append(" Transport");
        break;
	}

	/* language name? */
	if (MathFuncs.IsInsideMM(ind, (SPECSTR_LANGUAGE_START - 0x70E4), (SPECSTR_LANGUAGE_END - 0x70E4) + 1)) {
		int i = ind - (SPECSTR_LANGUAGE_START - 0x70E4);
		buff.Append(_languages[i] == _current_language ? _current_language.own_name : _languages[i].name);
	    break;
	}

	/* resolution size? */
	if (MathFuncs.IsInsideMM(ind, (SPECSTR_RESOLUTION_START - 0x70E4), (SPECSTR_RESOLUTION_END - 0x70E4) + 1)) {
		int i = ind - (SPECSTR_RESOLUTION_START - 0x70E4);
		buff.Append($"{_resolutions[i].width}x{_resolutions[i].height}");
	    break;
	}

	throw new NotReachedException();
}

extern void SortNetworkLanguages();


public int StringIDSorter(StringID a, StringID b)
{
	var stra = GetString(a);
    var strb = GetString(b);

	return String.Compare(stra, strb, StringComparison.CurrentCultureIgnoreCase);
}

/**
 * Get the language with the given NewGRF language ID.
 * @param newgrflangid NewGRF languages ID to check.
 * @return The language's metadata, or null if it is not known.
 */
public LanguageMetadata GetLanguage(byte newgrflangid)
{
	foreach (var lang in _languages) {
		if (newgrflangid == lang.newgrflangid) return lang;
	}

	return null;
}


/**
 * Gets a list of languages from the given directory.
 * @param path  the base directory to search in
 */
static void GetLanguageList(string path)
{
	var dir = new DirectoryInfo(path);
	if (dir.Exists)
	{
		foreach (var file in dir.GetFiles("*.lng"))
        {
		
			var lmd = new LanguageMetadata();
            lmd.file = file.FullName;
            
			/* Check whether the file is of the correct version */
			if (LanguagePackHeader.ReadHeader(lmd.file, lmd) == false) {
				Log.Debug($"{lmd.file} is not a valid language file");
			} else if (GetLanguage(lmd.newgrflangid) != null) {
				Log.Debug($"{lmd.file}'s language ID is already known");
			} else {
				_languages.Add(lmd);
			}
		}
	}
}

/**
 * Make a list of the available language packs. Put the data in
 * #_languages list.
 */
void InitializeLanguagePacks()
{
    foreach (var sp in FileIO.FOR_ALL_SEARCHPATHS)
    { 
		var path = FileIO.FioAppendDirectory(sp, Subdirectory.LANG_DIR);
		GetLanguageList(path);
	}
	if (_languages.Any() == false) usererror("No available language packs (invalid versions?)");

	/* Acquire the locale of the current system */
	var lang = GetCurrentLocale("LC_MESSAGES");
	if (lang == null) lang = "en_GB";

    /// Matching the language in the configuration file or the current locale
    LanguageMetadata chosen_language   = null;
    /// Using pt_PT for pt_BR locale when pt_BR is not available
    LanguageMetadata language_fallback = null;
    /// Fallback when no locale-matching language has been found
    LanguageMetadata en_GB_fallback    = _languages.First(); 

	/* Find a proper language. */
	foreach (var lng in _languages) {
		/* We are trying to find a default language. The priority is by
		 * configuration file, local environment and last, if nothing found,
		 * English. */
	    var lang_file = Path.GetFileName(lng.file);
		if (lang_file == _config_language_file) {
			chosen_language = lng;
			break;
		}

		if (lng.isocode == "en_GB") {en_GB_fallback    = lng;}
		if (string.Compare(lng.isocode,0, lang,0, 5) == 0) {chosen_language   = lng;}
	    if (string.Compare(lng.isocode, 0, lang, 0, 2) == 0) {language_fallback = lng;}
	}

	/* We haven't found the language in the config nor the one in the locale.
	 * Now we set it to one of the fallback languages */
	if (chosen_language == null) {
		chosen_language = language_fallback ?? en_GB_fallback;
	}

	if (!ReadLanguagePack(chosen_language)) usererror($"Can't read language pack '{chosen_language.file}'");
}

/**
 * Get the ISO language code of the currently loaded language.
 * @return the ISO code.
 */
string GetCurrentLanguageIsoCode()
{
	return _langpack.isocode;
}


/**
* Read a particular language.
* @param lang The metadata about the language.
* @return Whether the loading went okay or not.
*/
        bool ReadLanguagePack(LanguageMetadata lang)
        {
            /* Current language pack */
            var file = new FileInfo(lang.file);
            if (file.Exists == false) return false;

            uint count = 0;
            for (uint i = 0; i < Language.TAB_COUNT; i++)
            {
                ushort num = lang.offsets[i];
                if (num > Language.TAB_SIZE)
                {
                    return false;
                }

                _langtab_start[i] = count;
                _langtab_num[i] = num;
                count += num;
    }
    /* Allocate offsets */
            var langpack_offs = new string[count];

    using (var stream = file.OpenRead())
            {
                if (stream.Seek(LanguagePackHeader.HeaderByteSize, SeekOrigin.Begin) < LanguagePackHeader.HeaderByteSize)
                {
                    return false;
                }
                using (var reader = new BinaryReader(stream))
                {
                    /* Fill offsets */
                    for (uint i = 0; i < count; i++)
                    {
                        langpack_offs[i] = reader.ReadString();
                    }
                }
            }


            _langpack = lang;
            _langpack_offs = langpack_offs;

            _current_language = lang;
            _current_text_dir = (TextDirection)_current_language.text_dir;
            _config_language_file = Path.GetFileName(_current_language.file);
            SetCurrentGrfLangID(_current_language.newgrflangid);

            /* Delete previous collator. */
            if (_current_collator != null)
            {
                _current_collator = null;
            }

            /* Create a collator instance for our current locale. */
            UErrorCode status = U_ZERO_ERROR;
            _current_collator = Collator.createInstance(Locale(_current_language.isocode), status);
            /* Sort number substrings by their numerical value. */
            if (_current_collator != null) _current_collator.setAttribute(UCOL_NUMERIC_COLLATION, UCOL_ON, status);
            /* Avoid using the collator if it is not correctly set. */
            if (U_FAILURE(status))
            {
                _current_collator = null;
            }

            /* Some lists need to be sorted again after a language change. */
            ReconsiderGameScriptLanguage();
            InitializeSortedCargoSpecs();
            SortIndustryTypes();
            BuildIndustriesLegend();
            SortNetworkLanguages();
            BuildContentTypeStringList();
            InvalidateWindowClassesData(WC_BUILD_VEHICLE);      // Build vehicle window.
            InvalidateWindowClassesData(WC_TRAINS_LIST);        // Train group window.
            InvalidateWindowClassesData(WC_ROADVEH_LIST);       // Road vehicle group window.
            InvalidateWindowClassesData(WC_SHIPS_LIST);         // Ship group window.
            InvalidateWindowClassesData(WC_AIRCRAFT_LIST);      // Aircraft group window.
            InvalidateWindowClassesData(WC_INDUSTRY_DIRECTORY); // Industry directory window.
            InvalidateWindowClassesData(WC_STATION_LIST);       // Station list window.

            return true;
        }

        private static LanguagePackGlyphSearcher pack_searcher;

/**
 * Check whether the currently loaded language pack
 * uses characters that the currently loaded font
 * does not support. If this is the case an error
 * message will be shown in English. The error
 * message will not be localized because that would
 * mean it might use characters that are not in the
 * font, which is the whole reason this check has
 * been added.
 * @param base_font Whether to look at the base font as well.
 * @param searcher  The methods to use to search for strings to check.
 *                  If null the loaded language pack searcher is used.
 */
void CheckForMissingGlyphs(bool base_font, MissingGlyphSearcher searcher)
{	
	if (searcher == null) searcher = pack_searcher;
	bool bad_font = !base_font || searcher.FindMissingGlyphs(null);
	if (bad_font) {
		/* We found an unprintable character... lets try whether we can find
		 * a fallback font that can print the characters in the current language. */
		FreeTypeSettings backup;
		memcpy(&backup, &_freetype, sizeof(backup));

		bad_font = !SetFallbackFont(&_freetype, _langpack.isocode, _langpack.winlangid, searcher);

		memcpy(&_freetype, &backup, sizeof(backup));

		if (bad_font && base_font) {
			/* Our fallback font does miss characters too, so keep the
			 * user chosen font as that is more likely to be any good than
			 * the wild guess we made */
			InitFreeType(searcher.Monospace());
		}
	}

	if (bad_font) {
		/* All attempts have failed. Display an error. As we do not want the string to be translated by
		 * the translators, we 'force' it into the binary and 'load' it via a BindCString. To do this
		 * properly we have to set the colour of the string, otherwise we end up with a lot of artifacts.
		 * The colour 'character' might change in the future, so for safety we just Utf8 Encode it into
		 * the string, which takes exactly three characters, so it replaces the "XXX" with the colour marker. */
		static char *err_str = stredup("XXXThe current font is missing some of the characters used in the texts for this language. Read the readme to see how to solve this.");
		Utf8Encode(err_str, StringControlCode.SCC_YELLOW);
		SetDParamStr(0, err_str);
		ShowErrorMessage(STR_JUST_RAW_STRING, INVALID_STRING_ID, WL_WARNING);

		/* Reset the font width */
		LoadStringWidthTable(searcher.Monospace());
		return;
	}

	/* Update the font with cache */
	LoadStringWidthTable(searcher.Monospace());

}
}
}