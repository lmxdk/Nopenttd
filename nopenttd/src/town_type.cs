/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file town_type.h Types related to towns. */

namespace Nopenttd
{

    public struct TownId
    {
        public ushort Id { get; set; }

        public TownId(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(TownId id)
        {
            return id.Id;
        }

        public static implicit operator TownId(ushort id)
        {
            return new TownId(id);
        }
    }

/** Supported initial town sizes */

    public enum TownSize
    {
        /// Small town.
        TSZ_SMALL,

        /// Medium town. 
        TSZ_MEDIUM,

        /// Large town. 
        TSZ_LARGE,

        /// Random size, bigger than small, smaller than large. 
        TSZ_RANDOM,

        /// Number of available town sizes.
        TSZ_END,
    };

    //template <> struct EnumPropsT<TownSize> : MakeEnumPropsT<TownSize, byte, TSZ_SMALL, TSZ_END, TSZ_END, 2> {};

    public enum Ratings
    {
        /* These refer to the maximums, so Appalling is -1000 to -400
         * MAXIMUM RATINGS BOUNDARIES */
        RATING_MINIMUM = -1000,
        RATING_APPALLING = -400,
        RATING_VERYPOOR = -200,
        RATING_POOR = 0,
        RATING_MEDIOCRE = 200,
        RATING_GOOD = 400,
        RATING_VERYGOOD = 600,
        RATING_EXCELLENT = 800,

        /// OUTSTANDING
        RATING_OUTSTANDING = 1000,

        RATING_MAXIMUM = RATING_OUTSTANDING,

        /// initial rating
        RATING_INITIAL = 500,

        /* RATINGS AFFECTING NUMBERS */
        RATING_TREE_DOWN_STEP = -35,
        RATING_TREE_MINIMUM = RATING_MINIMUM,
        RATING_TREE_UP_STEP = 7,
        RATING_TREE_MAXIMUM = 220,

        /// when a town grows, all companies have rating increased a bit ...
        RATING_GROWTH_UP_STEP = 5,

        /// ... up to RATING_MEDIOCRE
        RATING_GROWTH_MAXIMUM = RATING_MEDIOCRE,

        /// when a town grows, company gains reputation for all well serviced stations ...
        RATING_STATION_UP_STEP = 12,

        /// ... but loses for bad serviced stations
        RATING_STATION_DOWN_STEP = -15,

        /// penalty for removing town owned tunnel or bridge
        RATING_TUNNEL_BRIDGE_DOWN_STEP = -250,

        /// minimum rating after removing tunnel or bridge
        RATING_TUNNEL_BRIDGE_MINIMUM = 0,

        /// rating needed, "Permissive" difficulty settings
        RATING_TUNNEL_BRIDGE_NEEDED_PERMISSIVE = 144,

        /// "Neutral"
        RATING_TUNNEL_BRIDGE_NEEDED_NEUTRAL = 208,

        /// "Hostile"
        RATING_TUNNEL_BRIDGE_NEEDED_HOSTILE = 400,

        /// removing a roadpiece in the middle
        RATING_ROAD_DOWN_STEP_INNER = -50,

        /// removing a roadpiece at the edge
        RATING_ROAD_DOWN_STEP_EDGE = -18,

        /// minimum rating after removing town owned road
        RATING_ROAD_MINIMUM = -100,

        /// rating needed, "Permissive" difficulty settings
        RATING_ROAD_NEEDED_PERMISSIVE = 16,

        /// "Neutral"
        RATING_ROAD_NEEDED_NEUTRAL = 64,

        /// "Hostile"
        RATING_ROAD_NEEDED_HOSTILE = 112,

        RATING_HOUSE_MINIMUM = RATING_MINIMUM,

        RATING_BRIBE_UP_STEP = 200,
        RATING_BRIBE_MAXIMUM = 800,
        // XXX SHOULD BE SOMETHING LOWER?
        RATING_BRIBE_DOWN_TO = -50
    }

/**
 * Town Layouts
 */

    public enum TownLayout
    {
        TL_BEGIN = 0,

        /// Original algorithm (min. 1 distance between roads)
        TL_ORIGINAL = 0,

        /// Extended original algorithm (min. 2 distance between roads)    
        TL_BETTER_ROADS,

        /// Geometric 2x2 grid algorithm    
        TL_2X2_GRID,

        /// Geometric 3x3 grid algorithm    
        TL_3X3_GRID,

        /// Random town layout
        TL_RANDOM,

        /// Number of town layouts
        NUM_TLS,
    }

//template <> struct EnumPropsT<TownLayout> : MakeEnumPropsT<TownLayout, byte, TL_BEGIN, NUM_TLS, NUM_TLS, 3> {};
/** It needs to be 8bits, because we save and load it as such */
//typedef SimpleTinyEnumT<TownLayout, byte> TownLayoutByte; // typedefing-enumification of TownLayout

/** Town founding setting values */

    public enum TownFounding
    {
        /// Used for iterations and limit testing
        TF_BEGIN = 0,

        /// Forbidden
        TF_FORBIDDEN = 0,

        /// Allowed
        TF_ALLOWED,

        /// Allowed, with custom town layout
        TF_CUSTOM_LAYOUT,

        /// Used for iterations and limit testing
        TF_END,
    };

    /** It needs to be 8bits, because we save and load it as such */
    //typedef SimpleTinyEnumT<TownFounding, byte> TownFoundingByte;

    public static class TownConstants
    {
        ///The maximum length of a town name in characters including '\0'
        public const uint MAX_LENGTH_TOWN_NAME_CHARS = 32;
    }

/** Store the maximum and actually transported cargo amount for the current and the last month. */
//template <typename Tstorage>
    public struct TransportedCargoStat<Tstorage>
    {
        /// Maximum amount last month
        Tstorage old_max;

        /// Maximum amount this month 
        Tstorage new_max;

        /// Actually transported last month 
        Tstorage old_act;

        /// Actually transported this month 
        Tstorage new_act;

        //public TransportedCargoStat()
        //{
        //    old_max = default(Tstorage);
        //    new_max = default(Tstorage);
        //    old_act = default(Tstorage);
        //    new_act = default(Tstorage);
        //}

        /** Update stats for a new month. */

        void NewMonth()
        {
            this.old_max = this.new_max;
            this.new_max = default(Tstorage);//0
            this.old_act = this.new_act;
            this.new_act = default(Tstorage);//0;
        }
    }
}