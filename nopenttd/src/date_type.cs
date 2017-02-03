/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file date_type.h Types related to the dates in OpenTTD. */

namespace Nopenttd
{

    /// The type to store our dates in
    public struct Date
    {
        public int Value { get; set; }

        public Date(int value)
        {
            Value = value;
        }

        public static implicit operator int(Date value)
        {
            return value.Value;
        }

        public static implicit operator Date(int value)
        {
            return new Date(value);
        }
    }

    /// The fraction of a date we're in, i.e. the number of ticks since the last date changeover
    public struct DateFract
    {
        public uint Value { get; set; }

        public DateFract(uint value)
        {
            Value = value;
        }

        public static implicit operator uint(DateFract value)
        {
            return value.Value;
        }

        public static implicit operator DateFract(uint value)
        {
            return new DateFract(value);
        }
    }

    /// The type to store ticks in
    public struct Ticks
    {
        public int Value { get; set; }

        public Ticks(int value)
        {
            Value = value;
        }

        public static implicit operator int(Ticks value)
        {
            return value.Value;
        }

        public static implicit operator Ticks(int value)
        {
            return new Ticks(value);
        }
    }

    /// Type for the year, note: 0 based, i.e. starts at the year 0.
    public struct Year
    {
        public int Value { get; set; }

        public Year(int value)
        {
            Value = value;
        }

        public static implicit operator int(Year value)
        {
            return value.Value;
        }

        public static implicit operator Year(int value)
        {
            return new Year(value);
        }
    }

    /// Type for the month, note: 0 based, i.e. 0 = January, 11 = December.
    public struct Month
    {
        public byte Value { get; set; }

        public Month(byte value)
        {
            Value = value;
        }

        public static implicit operator byte(Month value)
        {
            return value.Value;
        }

        public static implicit operator Month(byte value)
        {
            return new Month(value);
        }
    }

    /// Type for the day of the month, note: 1 based, first day of a month is 1.
    public struct Day
    {
        public byte Value { get; set; }

        public Day(byte value)
        {
            Value = value;
        }

        public static implicit operator byte(Day value)
        {
            return value.Value;
        }

        public static implicit operator Day(byte value)
        {
            return new Day(value);
        }
    }

    public class DateConstants
    {
        /**
         * 1 day is 74 ticks; _date_fract used to be uint16 and incremented by 885. On
         *                    an overflow the new day begun and 65535 / 885 = 74.
         * 1 tick is approximately 30 ms.
         * 1 day is thus about 2 seconds (74 * 30 = 2220) on a machine that can run OpenTTD normally
         */

        /// ticks per day
        public const int DAY_TICKS = 74;

        /// days per year
        public const int DAYS_IN_YEAR = 365;

        /// sometimes, you need one day more...
        public const int DAYS_IN_LEAP_YEAR = 366;

        /// cycle duration for updating station rating
        public const int STATION_RATING_TICKS = 185;

        /// cycle duration for updating station acceptance
        public const int STATION_ACCEPTANCE_TICKS = 250;

        /// cycle duration for cleaning dead links
        public const int STATION_LINKGRAPH_TICKS = 504;

        /// cycle duration for aging cargo
        public const int CARGO_AGING_TICKS = 185;

        /// cycle duration for industry production
        public const int INDUSTRY_PRODUCE_TICKS = 256;

        /// cycle duration for towns trying to grow. (this originates from the size of the town array in TTD
        public const int TOWN_GROWTH_TICKS = 70;

        /// cycle duration for lumber mill's extra action
        public const int INDUSTRY_CUT_TREE_TICKS = INDUSTRY_PRODUCE_TICKS * 2;


        /*
         * ORIGINAL_BASE_YEAR, ORIGINAL_MAX_YEAR and DAYS_TILL_ORIGINAL_BASE_YEAR are
         * primarily used for loading newgrf and savegame data and returning some
         * newgrf (callback) functions that were in the original (TTD) inherited
         * format, where '_date == 0' meant that it was 1920-01-01.
         */

        /** The minimum starting year/base year of the original TTD */
        public static readonly Year ORIGINAL_BASE_YEAR = 1920;
        /** The original ending year */
        public static readonly Year ORIGINAL_END_YEAR = 2051;
        /** The maximum year of the original TTD */
        public static readonly Year ORIGINAL_MAX_YEAR = 2090;
        /** The absolute minimum & maximum years in OTTD */
        public static readonly Year MIN_YEAR = 0;

        /** The default starting year */
        public static readonly Year DEF_START_YEAR = 1950;

        /**
         * MAX_YEAR, nicely rounded value of the number of years that can
         * be encoded in a single 32 bits date, about 2^31 / 366 years.
         */
        public static readonly Year MAX_YEAR = 5000000;

        /// Representation of an invalid year
        public static readonly Year INVALID_YEAR = -1;

        /// Representation of an invalid date
        public static readonly Date INVALID_DATE = -1;

        /// Representation of an invalid number of ticks
        public static readonly Ticks INVALID_TICKS = -1;


    }

    public static class DateExtensions
    {
        /**
         * Calculate the number of leap years till a given year.
         *
         * Each passed leap year adds one day to the 'day count'.
         *
         * A special case for the year 0 as no year has been passed,
         * but '(year - 1) / 4' does not yield '-1' to counteract the
         * '+1' at the end of the formula as divisions round to zero.
         *
         * @param year the year to get the leap years till.
         * @return the number of leap years.
         */

        public static int LEAP_YEARS_TILL(this Year year)
        {
            return year == 0 ? 0 : (year - 1) / 4 - (year - 1) / 100 + (year - 1) / 400 + 1;
        }

        /**
         * Calculate the date of the first day of a given year.
         * @param year the year to get the first day of.
         * @return the date.
         */

        public static Date DAYS_TILL(this Year year)
        {
            return DateConstants.DAYS_IN_YEAR * year + year.LEAP_YEARS_TILL();
        }

        /**
         * The offset in days from the '_date == 0' till
         * 'ConvertYMDToDate(ORIGINAL_BASE_YEAR, 0, 1)'
         */

        public static Date DAYS_TILL_ORIGINAL_BASE_YEAR()
        {
            return DAYS_TILL(DateConstants.ORIGINAL_BASE_YEAR);
        }

        /** The number of days till the last day */

        public static int MAX_DAY()
        {
            return DAYS_TILL(DateConstants.MAX_YEAR + 1) - 1;
        }
    }

/**
 * Data structure to convert between Date and triplet (year, month, and day).
 * @see ConvertDateToYMD(), ConvertYMDToDate()
 */

    public struct YearMonthDay
    {
        /// Year (0...)
        Year year;

        /// Month (0..11)
        Month month;

        /// Day (1..31)
        Day day;
    }

}