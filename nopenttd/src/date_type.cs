/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file date_type.h Types related to the dates in OpenTTD. */

using System.Diagnostics;

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

        /**
         * Converts a Date to a Year, Month & Day.
         * @param date the date to convert from
         * @param ymd  the year, month and day to write to
         */
        public YearMonthDay ConvertDateToYMD()
        {
            /* Year determination in multiple steps to account for leap
             * years. First do the large steps, then the smaller ones.
             */

            /* There are 97 leap years in 400 years */
            Year yr = 400 * (Value / (DateConstants.DAYS_IN_YEAR * 400 + 97));
            int rem = Value % (DateConstants.DAYS_IN_YEAR * 400 + 97);
            ushort x;

            if (rem >= DateConstants.DAYS_IN_YEAR * 100 + 25)
            {
                /* There are 25 leap years in the first 100 years after
                 * every 400th year, as every 400th year is a leap year */
                yr += 100;
                rem -= DateConstants.DAYS_IN_YEAR * 100 + 25;

                /* There are 24 leap years in the next couple of 100 years */
                yr += 100 * (rem / (DateConstants.DAYS_IN_YEAR * 100 + 24));
                rem = (rem % (DateConstants.DAYS_IN_YEAR * 100 + 24));
            }

            if (!yr.IsLeapYear() && rem >= DateConstants.DAYS_IN_YEAR * 4)
            {
                /* The first 4 year of the century are not always a leap year */
                yr += 4;
                rem -= DateConstants.DAYS_IN_YEAR * 4;
            }

            /* There is 1 leap year every 4 years */
            yr += 4 * (rem / (DateConstants.DAYS_IN_YEAR * 4 + 1));
            rem = rem % (DateConstants.DAYS_IN_YEAR * 4 + 1);

            /* The last (max 3) years to account for; the first one
             * can be, but is not necessarily a leap year */
            while (rem >= (yr.IsLeapYear() ? DateConstants.DAYS_IN_LEAP_YEAR : DateConstants.DAYS_IN_YEAR))
            {
                rem -= yr.IsLeapYear() ? DateConstants.DAYS_IN_LEAP_YEAR : DateConstants.DAYS_IN_YEAR;
                yr++;
            }

            /* Skip the 29th of February in non-leap years */
            if (!yr.IsLeapYear() && rem >= (int)DaysTillMonth.ACCUM_MAR - 1) rem++;

            var ymd = new YearMonthDay();
            ymd.year = yr;

            x = DateConstants._month_date_from_year_day[rem];
            ymd.month = (byte)(x >> 5);
            ymd.day = (byte)(x & 0x1F);
            return ymd;
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

                                            /// Current year, starting at 0
        public static Year _cur_year;       /// Current month (0..11) 
        public static Month _cur_month;     /// Current date in days (day counter) 
        public static Date _date;           /// Fractional part of the day. 
        public static DateFract _date_fract;/// Ever incrementing (and sometimes wrapping) tick counter for setting off various events 
        public static ushort _tick_counter;  

        /**
         * Set the date.
         * @param date  New date
         * @param fract The number of ticks that have passed on this date.
         */
        public static void SetDate(Date date, DateFract fract)
        {
            Debug.Assert(fract < DAY_TICKS);

            _date = date;
            _date_fract = fract;
            var ymd = date.ConvertDateToYMD();
            _cur_year = ymd.year;
            _cur_month = ymd.month;
        }


        public static readonly ushort[] _month_date_from_year_day = {
            (( 0 << 5) | 1),(( 0 << 5) | 2),(( 0 << 5) | 3),(( 0 << 5) | 4),(( 0 << 5) | 5),(( 0 << 5) | 6),(( 0 << 5) | 7),(( 0 << 5) | 8),(( 0 << 5) | 9),(( 0 << 5) | 10),(( 0 << 5) | 11),(( 0 << 5) | 12),(( 0 << 5) | 13),(( 0 << 5) | 14),(( 0 << 5) | 15),(( 0 << 5) | 16),(( 0 << 5) | 17),(( 0 << 5) | 18),(( 0 << 5) | 19),(( 0 << 5) | 20),(( 0 << 5) | 21),(( 0 << 5) | 22),(( 0 << 5) | 23),(( 0 << 5) | 24),(( 0 << 5) | 25),(( 0 << 5) | 26),(( 0 << 5) | 27),(( 0 << 5) | 28),(( 0 << 5) | 29),(( 0 << 5) | 30),(( 0 << 5) | 31),
            (( 1 << 5) | 1),(( 1 << 5) | 2),(( 1 << 5) | 3),(( 1 << 5) | 4),(( 1 << 5) | 5),(( 1 << 5) | 6),(( 1 << 5) | 7),(( 1 << 5) | 8),(( 1 << 5) | 9),(( 1 << 5) | 10),(( 1 << 5) | 11),(( 1 << 5) | 12),(( 1 << 5) | 13),(( 1 << 5) | 14),(( 1 << 5) | 15),(( 1 << 5) | 16),(( 1 << 5) | 17),(( 1 << 5) | 18),(( 1 << 5) | 19),(( 1 << 5) | 20),(( 1 << 5) | 21),(( 1 << 5) | 22),(( 1 << 5) | 23),(( 1 << 5) | 24),(( 1 << 5) | 25),(( 1 << 5) | 26),(( 1 << 5) | 27),(( 1 << 5) | 28),(( 1 << 5) | 29),
            (( 2 << 5) | 1),(( 2 << 5) | 2),(( 2 << 5) | 3),(( 2 << 5) | 4),(( 2 << 5) | 5),(( 2 << 5) | 6),(( 2 << 5) | 7),(( 2 << 5) | 8),(( 2 << 5) | 9),(( 2 << 5) | 10),(( 2 << 5) | 11),(( 2 << 5) | 12),(( 2 << 5) | 13),(( 2 << 5) | 14),(( 2 << 5) | 15),(( 2 << 5) | 16),(( 2 << 5) | 17),(( 2 << 5) | 18),(( 2 << 5) | 19),(( 2 << 5) | 20),(( 2 << 5) | 21),(( 2 << 5) | 22),(( 2 << 5) | 23),(( 2 << 5) | 24),(( 2 << 5) | 25),(( 2 << 5) | 26),(( 2 << 5) | 27),(( 2 << 5) | 28),(( 2 << 5) | 29),(( 2 << 5) | 30),(( 2 << 5) | 31),
            (( 3 << 5) | 1),(( 3 << 5) | 2),(( 3 << 5) | 3),(( 3 << 5) | 4),(( 3 << 5) | 5),(( 3 << 5) | 6),(( 3 << 5) | 7),(( 3 << 5) | 8),(( 3 << 5) | 9),(( 3 << 5) | 10),(( 3 << 5) | 11),(( 3 << 5) | 12),(( 3 << 5) | 13),(( 3 << 5) | 14),(( 3 << 5) | 15),(( 3 << 5) | 16),(( 3 << 5) | 17),(( 3 << 5) | 18),(( 3 << 5) | 19),(( 3 << 5) | 20),(( 3 << 5) | 21),(( 3 << 5) | 22),(( 3 << 5) | 23),(( 3 << 5) | 24),(( 3 << 5) | 25),(( 3 << 5) | 26),(( 3 << 5) | 27),(( 3 << 5) | 28),(( 3 << 5) | 29),(( 3 << 5) | 30),
            (( 4 << 5) | 1),(( 4 << 5) | 2),(( 4 << 5) | 3),(( 4 << 5) | 4),(( 4 << 5) | 5),(( 4 << 5) | 6),(( 4 << 5) | 7),(( 4 << 5) | 8),(( 4 << 5) | 9),(( 4 << 5) | 10),(( 4 << 5) | 11),(( 4 << 5) | 12),(( 4 << 5) | 13),(( 4 << 5) | 14),(( 4 << 5) | 15),(( 4 << 5) | 16),(( 4 << 5) | 17),(( 4 << 5) | 18),(( 4 << 5) | 19),(( 4 << 5) | 20),(( 4 << 5) | 21),(( 4 << 5) | 22),(( 4 << 5) | 23),(( 4 << 5) | 24),(( 4 << 5) | 25),(( 4 << 5) | 26),(( 4 << 5) | 27),(( 4 << 5) | 28),(( 4 << 5) | 29),(( 4 << 5) | 30),(( 4 << 5) | 31),
            (( 5 << 5) | 1),(( 5 << 5) | 2),(( 5 << 5) | 3),(( 5 << 5) | 4),(( 5 << 5) | 5),(( 5 << 5) | 6),(( 5 << 5) | 7),(( 5 << 5) | 8),(( 5 << 5) | 9),(( 5 << 5) | 10),(( 5 << 5) | 11),(( 5 << 5) | 12),(( 5 << 5) | 13),(( 5 << 5) | 14),(( 5 << 5) | 15),(( 5 << 5) | 16),(( 5 << 5) | 17),(( 5 << 5) | 18),(( 5 << 5) | 19),(( 5 << 5) | 20),(( 5 << 5) | 21),(( 5 << 5) | 22),(( 5 << 5) | 23),(( 5 << 5) | 24),(( 5 << 5) | 25),(( 5 << 5) | 26),(( 5 << 5) | 27),(( 5 << 5) | 28),(( 5 << 5) | 29),(( 5 << 5) | 30),
            (( 6 << 5) | 1),(( 6 << 5) | 2),(( 6 << 5) | 3),(( 6 << 5) | 4),(( 6 << 5) | 5),(( 6 << 5) | 6),(( 6 << 5) | 7),(( 6 << 5) | 8),(( 6 << 5) | 9),(( 6 << 5) | 10),(( 6 << 5) | 11),(( 6 << 5) | 12),(( 6 << 5) | 13),(( 6 << 5) | 14),(( 6 << 5) | 15),(( 6 << 5) | 16),(( 6 << 5) | 17),(( 6 << 5) | 18),(( 6 << 5) | 19),(( 6 << 5) | 20),(( 6 << 5) | 21),(( 6 << 5) | 22),(( 6 << 5) | 23),(( 6 << 5) | 24),(( 6 << 5) | 25),(( 6 << 5) | 26),(( 6 << 5) | 27),(( 6 << 5) | 28),(( 6 << 5) | 29),(( 6 << 5) | 30),(( 6 << 5) | 31),
            (( 7 << 5) | 1),(( 7 << 5) | 2),(( 7 << 5) | 3),(( 7 << 5) | 4),(( 7 << 5) | 5),(( 7 << 5) | 6),(( 7 << 5) | 7),(( 7 << 5) | 8),(( 7 << 5) | 9),(( 7 << 5) | 10),(( 7 << 5) | 11),(( 7 << 5) | 12),(( 7 << 5) | 13),(( 7 << 5) | 14),(( 7 << 5) | 15),(( 7 << 5) | 16),(( 7 << 5) | 17),(( 7 << 5) | 18),(( 7 << 5) | 19),(( 7 << 5) | 20),(( 7 << 5) | 21),(( 7 << 5) | 22),(( 7 << 5) | 23),(( 7 << 5) | 24),(( 7 << 5) | 25),(( 7 << 5) | 26),(( 7 << 5) | 27),(( 7 << 5) | 28),(( 7 << 5) | 29),(( 7 << 5) | 30),(( 7 << 5) | 31),
            (( 8 << 5) | 1),(( 8 << 5) | 2),(( 8 << 5) | 3),(( 8 << 5) | 4),(( 8 << 5) | 5),(( 8 << 5) | 6),(( 8 << 5) | 7),(( 8 << 5) | 8),(( 8 << 5) | 9),(( 8 << 5) | 10),(( 8 << 5) | 11),(( 8 << 5) | 12),(( 8 << 5) | 13),(( 8 << 5) | 14),(( 8 << 5) | 15),(( 8 << 5) | 16),(( 8 << 5) | 17),(( 8 << 5) | 18),(( 8 << 5) | 19),(( 8 << 5) | 20),(( 8 << 5) | 21),(( 8 << 5) | 22),(( 8 << 5) | 23),(( 8 << 5) | 24),(( 8 << 5) | 25),(( 8 << 5) | 26),(( 8 << 5) | 27),(( 8 << 5) | 28),(( 8 << 5) | 29),(( 8 << 5) | 30),
            (( 9 << 5) | 1),(( 9 << 5) | 2),(( 9 << 5) | 3),(( 9 << 5) | 4),(( 9 << 5) | 5),(( 9 << 5) | 6),(( 9 << 5) | 7),(( 9 << 5) | 8),(( 9 << 5) | 9),(( 9 << 5) | 10),(( 9 << 5) | 11),(( 9 << 5) | 12),(( 9 << 5) | 13),(( 9 << 5) | 14),(( 9 << 5) | 15),(( 9 << 5) | 16),(( 9 << 5) | 17),(( 9 << 5) | 18),(( 9 << 5) | 19),(( 9 << 5) | 20),(( 9 << 5) | 21),(( 9 << 5) | 22),(( 9 << 5) | 23),(( 9 << 5) | 24),(( 9 << 5) | 25),(( 9 << 5) | 26),(( 9 << 5) | 27),(( 9 << 5) | 28),(( 9 << 5) | 29),(( 9 << 5) | 30),(( 9 << 5) | 31),
            ((10 << 5) | 1),((10 << 5) | 2),((10 << 5) | 3),((10 << 5) | 4),((10 << 5) | 5),((10 << 5) | 6),((10 << 5) | 7),((10 << 5) | 8),((10 << 5) | 9),((10 << 5) | 10),((10 << 5) | 11),((10 << 5) | 12),((10 << 5) | 13),((10 << 5) | 14),((10 << 5) | 15),((10 << 5) | 16),((10 << 5) | 17),((10 << 5) | 18),((10 << 5) | 19),((10 << 5) | 20),((10 << 5) | 21),((10 << 5) | 22),((10 << 5) | 23),((10 << 5) | 24),((10 << 5) | 25),((10 << 5) | 26),((10 << 5) | 27),((10 << 5) | 28),((10 << 5) | 29),((10 << 5) | 30),
            ((11 << 5) | 1),((11 << 5) | 2),((11 << 5) | 3),((11 << 5) | 4),((11 << 5) | 5),((11 << 5) | 6),((11 << 5) | 7),((11 << 5) | 8),((11 << 5) | 9),((11 << 5) | 10),((11 << 5) | 11),((11 << 5) | 12),((11 << 5) | 13),((11 << 5) | 14),((11 << 5) | 15),((11 << 5) | 16),((11 << 5) | 17),((11 << 5) | 18),((11 << 5) | 19),((11 << 5) | 20),((11 << 5) | 21),((11 << 5) | 22),((11 << 5) | 23),((11 << 5) | 24),((11 << 5) | 25),((11 << 5) | 26),((11 << 5) | 27),((11 << 5) | 28),((11 << 5) | 29),((11 << 5) | 30),((11 << 5) | 31)};

        /** Number of days to pass from the first day in the year before reaching the first of a month. */
        public static readonly ushort[] _accum_days_for_month = {
            (ushort)DaysTillMonth.ACCUM_JAN, (ushort)DaysTillMonth.ACCUM_FEB, (ushort)DaysTillMonth.ACCUM_MAR, (ushort)DaysTillMonth.ACCUM_APR,
            (ushort)DaysTillMonth.ACCUM_MAY, (ushort)DaysTillMonth.ACCUM_JUN, (ushort)DaysTillMonth.ACCUM_JUL, (ushort)DaysTillMonth.ACCUM_AUG,
            (ushort)DaysTillMonth.ACCUM_SEP, (ushort)DaysTillMonth.ACCUM_OCT, (ushort)DaysTillMonth.ACCUM_NOV, (ushort)DaysTillMonth.ACCUM_DEC,
        };


        /** Available settings for autosave intervals. */
        public static readonly Month[] _autosave_months = {
              /// never
            0,/// every month  
            1,/// every 3 months  
            3,/// every 6 months  
            6,/// every 12 months  
            12
        };


        ///**
        // * Runs various procedures that have to be done yearly
        // */
        //static void OnNewYear()
        //{
        //    CompaniesYearlyLoop();
        //    VehiclesYearlyLoop();
        //    TownsYearlyLoop();
        //    InvalidateWindowClassesData(WC_BUILD_STATION);
        //    if (_network_server) NetworkServerYearlyLoop();

        //    if (_cur_year == _settings_client.gui.semaphore_build_before) ResetSignalVariant();

        //    /* check if we reached end of the game */
        //    if (_cur_year == ORIGINAL_END_YEAR)
        //    {
        //        ShowEndGameChart();
        //        /* check if we reached the maximum year, decrement dates by a year */
        //    }
        //    else if (_cur_year == MAX_YEAR + 1)
        //    {
        //        Vehicle* v;
        //        int days_this_year;

        //        _cur_year--;
        //        days_this_year = IsLeapYear(_cur_year) ? DAYS_IN_LEAP_YEAR : DAYS_IN_YEAR;
        //        _date -= days_this_year;
        //        FOR_ALL_VEHICLES(v) v->date_of_last_service -= days_this_year;

        //        LinkGraph* lg;
        //        FOR_ALL_LINK_GRAPHS(lg) lg->ShiftDates(-days_this_year);

        //        /* Because the _date wraps here, and text-messages expire by game-days, we have to clean out
        //         *  all of them if the date is set back, else those messages will hang for ever */
        //        NetworkInitChatMessage();
        //    }

        //    if (_settings_client.gui.auto_euro) CheckSwitchToEuro();
        //}

        ///**
        // * Runs various procedures that have to be done monthly
        // */
        //static void OnNewMonth()
        //{
        //    if (_settings_client.gui.autosave != 0 && (_cur_month % _autosave_months[_settings_client.gui.autosave]) == 0)
        //    {
        //        _do_autosave = true;
        //        SetWindowDirty(WC_STATUS_BAR, 0);
        //    }

        //    SetWindowClassesDirty(WC_CHEATS);
        //    CompaniesMonthlyLoop();
        //    EnginesMonthlyLoop();
        //    TownsMonthlyLoop();
        //    IndustryMonthlyLoop();
        //    SubsidyMonthlyLoop();
        //    StationMonthlyLoop();
        //    if (_network_server) NetworkServerMonthlyLoop();
        //}

        ///**
        // * Runs various procedures that have to be done daily
        // */
        //static void OnNewDay()
        //{
        //    if (_network_server) NetworkServerDailyLoop();

        //    DisasterDailyLoop();
        //    IndustryDailyLoop();

        //    SetWindowWidgetDirty(WC_STATUS_BAR, 0, 0);
        //    EnginesDailyLoop();

        //    /* Refresh after possible snowline change */
        //    SetWindowClassesDirty(WC_TOWN_VIEW);
        //}

        ///**
        // * Increases the tick counter, increases date  and possibly calls
        // * procedures that have to be called daily, monthly or yearly.
        // */
        //void IncreaseDate()
        //{
        //    /* increase day, and check if a new day is there? */
        //    _tick_counter++;

        //    if (_game_mode == GM_MENU) return;

        //    _date_fract++;
        //    if (_date_fract < DAY_TICKS) return;
        //    _date_fract = 0;

        //    /* increase day counter */
        //    _date++;

        //    YearMonthDay ymd;
        //    ConvertDateToYMD(_date, &ymd);

        //    /* check if we entered a new month? */
        //    bool new_month = ymd.month != _cur_month;

        //    /* check if we entered a new year? */
        //    bool new_year = ymd.year != _cur_year;

        //    /* update internal variables before calling the daily/monthly/yearly loops */
        //    _cur_month = ymd.month;
        //    _cur_year = ymd.year;

        //    /* yes, call various daily loops */
        //    OnNewDay();

        //    /* yes, call various monthly loops */
        //    if (new_month) OnNewMonth();

        //    /* yes, call various yearly loops */
        //    if (new_year) OnNewYear();
        //}

    }

    public enum DaysTillMonth
    {
        ACCUM_JAN = 0,
        ACCUM_FEB = ACCUM_JAN + 31,
        ACCUM_MAR = ACCUM_FEB + 29,
        ACCUM_APR = ACCUM_MAR + 31,
        ACCUM_MAY = ACCUM_APR + 30,
        ACCUM_JUN = ACCUM_MAY + 31,
        ACCUM_JUL = ACCUM_JUN + 30,
        ACCUM_AUG = ACCUM_JUL + 31,
        ACCUM_SEP = ACCUM_AUG + 31,
        ACCUM_OCT = ACCUM_SEP + 30,
        ACCUM_NOV = ACCUM_OCT + 31,
        ACCUM_DEC = ACCUM_NOV + 30,
    };


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

        /**
         * Converts a tuple of Year, Month and Day to a Date.
         * @param year  is a number between 0..MAX_YEAR
         * @param month is a number between 0..11
         * @param day   is a number between 1..31
         */
        public static Date ConvertYMDToDate(Year year, Month month, Day day)
        {
            /* Day-offset in a leap year */
            int days = DateConstants._accum_days_for_month[month] + day - 1;

            /* Account for the missing of the 29th of February in non-leap years */
            if (!IsLeapYear(year) && days >= (int)DaysTillMonth.ACCUM_MAR) days--;

            return DAYS_TILL(year) + days;
        }


        /**
         * Checks whether the given year is a leap year or not.
         * @param yr The year to check.
         * @return True if \c yr is a leap year, otherwise false.
         */
        //inline
        public static bool IsLeapYear(this Year yr)
        {
            return yr % 4 == 0 && (yr % 100 != 0 || yr % 400 == 0);
        }
    }

/**
 * Data structure to convert between Date and triplet (year, month, and day).
 * @see ConvertDateToYMD(), ConvertYMDToDate()
 */

    public struct YearMonthDay
    {
        /// Year (0...)
        public Year year;

        /// Month (0..11)
        public Month month;

        /// Day (1..31)
        public Day day;


    }

}