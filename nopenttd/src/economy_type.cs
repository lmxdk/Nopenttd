/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file economy_type.h Types related to the economy. */

namespace Nopenttd
{

    //typedef OverflowSafeInt64 Money;
    public struct Money
    {
        public long Id { get; set; }

        public Money(long id)
        {
            Id = id;
        }

        public static implicit operator long(Money id)
        {
            return id.Id;
        }

        public static implicit operator Money(long id)
        {
            return new Money(id);
        }
    }




/** Data of the economy. */

    struct Economy
    {
        /// NOSAVE: Maximum possible loan
        Money max_loan;

        /// Economy fluctuation status
        short fluct;

        /// Interest
        byte interest_rate;

        /// inflation amount
        byte infl_amount;

        /// inflation rate for payment rates
        byte infl_amount_pr;

        /// Bits 31-16 are number of industry to be performed, 15-0 are fractional collected daily
        uint industry_daily_change_counter;

        /// The value which will increment industry_daily_change_counter. Computed value. NOSAVE
        uint industry_daily_increment;

        /// Cumulated inflation of prices since game start; 16 bit fractional part
        ulong inflation_prices;

        /// Cumulated inflation of cargo paypent since game start; 16 bit fractional part
        ulong inflation_payment;

        /* Old stuff for savegame conversion only */

        /// Old: Unrounded max loan
        Money old_max_loan_unround;

        /// Old: Fraction of the unrounded max loan
        ushort old_max_loan_unround_fract;
    };

/** Score categories in the detailed performance rating. */

    enum ScoreID
    {
        SCORE_BEGIN = 0,
        SCORE_VEHICLES = 0,
        SCORE_STATIONS = 1,
        SCORE_MIN_PROFIT = 2,
        SCORE_MIN_INCOME = 3,
        SCORE_MAX_INCOME = 4,
        SCORE_DELIVERED = 5,
        SCORE_CARGO = 6,
        SCORE_MONEY = 7,
        SCORE_LOAN = 8,

        /// This must always be the last entry
        SCORE_TOTAL = 9,

        /// How many scores are there..  
        SCORE_END = 10,

        /// The max score that can be in the performance history
        SCORE_MAX = 1000,
        /* the scores together of score_info is allowed to be more! */
    }

//DECLARE_POSTFIX_INCREMENT(ScoreID)

/** Data structure for storing how the score is computed for a single score id. */

    public struct ScoreInfo
    {
        /// How much you need to get the perfect score
        public int needed;

        /// How much score it will give
        public int score;
    };

/**
 * Enumeration of all base prices for use with #Prices.
 * The prices are ordered as they are expected by NewGRF cost multipliers, so don't shuffle them.
 */

    public enum Price
    {
        PR_BEGIN = 0,
        PR_STATION_VALUE = 0,
        PR_BUILD_RAIL,
        PR_BUILD_ROAD,
        PR_BUILD_SIGNALS,
        PR_BUILD_BRIDGE,
        PR_BUILD_DEPOT_TRAIN,
        PR_BUILD_DEPOT_ROAD,
        PR_BUILD_DEPOT_SHIP,
        PR_BUILD_TUNNEL,
        PR_BUILD_STATION_RAIL,
        PR_BUILD_STATION_RAIL_LENGTH,
        PR_BUILD_STATION_AIRPORT,
        PR_BUILD_STATION_BUS,
        PR_BUILD_STATION_TRUCK,
        PR_BUILD_STATION_DOCK,
        PR_BUILD_VEHICLE_TRAIN,
        PR_BUILD_VEHICLE_WAGON,
        PR_BUILD_VEHICLE_AIRCRAFT,
        PR_BUILD_VEHICLE_ROAD,
        PR_BUILD_VEHICLE_SHIP,
        PR_BUILD_TREES,
        PR_TERRAFORM,
        PR_CLEAR_GRASS,
        PR_CLEAR_ROUGH,
        PR_CLEAR_ROCKS,
        PR_CLEAR_FIELDS,
        PR_CLEAR_TREES,
        PR_CLEAR_RAIL,
        PR_CLEAR_SIGNALS,
        PR_CLEAR_BRIDGE,
        PR_CLEAR_DEPOT_TRAIN,
        PR_CLEAR_DEPOT_ROAD,
        PR_CLEAR_DEPOT_SHIP,
        PR_CLEAR_TUNNEL,
        PR_CLEAR_WATER,
        PR_CLEAR_STATION_RAIL,
        PR_CLEAR_STATION_AIRPORT,
        PR_CLEAR_STATION_BUS,
        PR_CLEAR_STATION_TRUCK,
        PR_CLEAR_STATION_DOCK,
        PR_CLEAR_HOUSE,
        PR_CLEAR_ROAD,
        PR_RUNNING_TRAIN_STEAM,
        PR_RUNNING_TRAIN_DIESEL,
        PR_RUNNING_TRAIN_ELECTRIC,
        PR_RUNNING_AIRCRAFT,
        PR_RUNNING_ROADVEH,
        PR_RUNNING_SHIP,
        PR_BUILD_INDUSTRY,
        PR_CLEAR_INDUSTRY,
        PR_BUILD_OBJECT,
        PR_CLEAR_OBJECT,
        PR_BUILD_WAYPOINT_RAIL,
        PR_CLEAR_WAYPOINT_RAIL,
        PR_BUILD_WAYPOINT_BUOY,
        PR_CLEAR_WAYPOINT_BUOY,
        PR_TOWN_ACTION,
        PR_BUILD_FOUNDATION,
        PR_BUILD_INDUSTRY_RAW,
        PR_BUILD_TOWN,
        PR_BUILD_CANAL,
        PR_CLEAR_CANAL,
        PR_BUILD_AQUEDUCT,
        PR_CLEAR_AQUEDUCT,
        PR_BUILD_LOCK,
        PR_CLEAR_LOCK,
        PR_INFRASTRUCTURE_RAIL,
        PR_INFRASTRUCTURE_ROAD,
        PR_INFRASTRUCTURE_WATER,
        PR_INFRASTRUCTURE_STATION,
        PR_INFRASTRUCTURE_AIRPORT,

        PR_END,
        INVALID_PRICE = 0xFF
    }

    //DECLARE_POSTFIX_INCREMENT(Price)

    /// Prices of everything. @see Price
//    typedef Money[Price.PR_END] Prices; 
//typedef sbyte[Price.PR.END] PriceMultipliers;

/** Types of expenses. */
    public enum ExpensesType
    {
        /// Construction costs.
        EXPENSES_CONSTRUCTION = 0,

        /// New vehicles.
        EXPENSES_NEW_VEHICLES,

        /// Running costs trains.
        EXPENSES_TRAIN_RUN,

        /// Running costs road vehicles.
        EXPENSES_ROADVEH_RUN,

        /// Running costs aircrafts.
        EXPENSES_AIRCRAFT_RUN,

        /// Running costs ships.
        EXPENSES_SHIP_RUN,

        /// Property costs.
        EXPENSES_PROPERTY,

        /// Income from trains.
        EXPENSES_TRAIN_INC,

        /// Income from road vehicles.
        EXPENSES_ROADVEH_INC,

        /// Income from aircrafts.
        EXPENSES_AIRCRAFT_INC,

        /// Income from ships.
        EXPENSES_SHIP_INC,

        /// Interest payments over the loan.
        EXPENSES_LOAN_INT,

        /// Other expenses.
        EXPENSES_OTHER,

        /// Number of expense types.
        EXPENSES_END,

        /// Invalid expense type.
        INVALID_EXPENSES = 0xFF,
    };

/** Define basic enum properties for ExpensesType */
//template <> struct EnumPropsT<ExpensesType> : MakeEnumPropsT<ExpensesType, byte, EXPENSES_CONSTRUCTION, EXPENSES_END, INVALID_EXPENSES, 8> {};
//typedef TinyEnumT<ExpensesType> ExpensesTypeByte; /// typedefing-enumification of ExpensesType

/**
 * Categories of a price bases.
 */

    public enum PriceCategory
    {
        /// Not affected by difficulty settings
        PCAT_NONE,

        /// Price is affected by "vehicle running cost" difficulty setting
        PCAT_RUNNING,

        /// Price is affected by "construction cost" difficulty setting
        PCAT_CONSTRUCTION,
    }

/**
 * Describes properties of price bases.
 */

    public struct PriceBaseSpec
    {
        /// Default value at game start, before adding multipliers.
        Money start_price;

        /// Price is affected by certain difficulty settings.
        PriceCategory category;

        /// GRF Feature that decides whether price multipliers apply locally or globally, #GSF_END if none.
        uint grf_feature;

        /// Fallback price multiplier for new prices but old grfs.
        Price fallback_price;
    }

    public class EconomyConstants
    {
/** The "steps" in loan size, in British Pounds! */
        public const int LOAN_INTERVAL = 10000;

/**
 * Maximum inflation (including fractional part) without causing overflows in int64 price computations.
 * This allows for 32 bit base prices (21 are currently needed).
 * Considering the sign bit and 16 fractional bits, there are 15 bits left.
 * 170 years of 4% inflation result in a inflation of about 822, so 10 bits are actually enough.
 * Note that NewGRF multipliers share the 16 fractional bits.
 * @see MAX_PRICE_MODIFIER
 */
        public const ulong MAX_INFLATION = (1UL << (63 - 32)) - 1;

/**
 * Maximum NewGRF price modifiers.
 * Increasing base prices by factor 65536 should be enough.
 * @see MAX_INFLATION
 */
        public const int MIN_PRICE_MODIFIER = -8;
        public const int MAX_PRICE_MODIFIER = 16;
        public const int INVALID_PRICE_MODIFIER = MIN_PRICE_MODIFIER - 1;

/** Multiplier for how many regular track bits a tunnel/bridge counts. */
        public const uint TUNNELBRIDGE_TRACKBIT_FACTOR = 4;
/** Multiplier for how many regular track bits a level crossing counts. */
        public const uint LEVELCROSSING_TRACKBIT_FACTOR = 2;
/** Multiplier for how many regular tiles a lock counts. */
        public const uint LOCK_DEPOT_TILE_FACTOR = 2;

    }

    public struct CargoPaymentID
    {
        public uint Id { get; set; }

        public CargoPaymentID(uint id)
        {
            Id = id;
        }

        public static implicit operator uint(CargoPaymentID id)
        {
            return id.Id;
        }

        public static implicit operator CargoPaymentID(uint id)
        {
            return new CargoPaymentID(id);
        }
    }
}