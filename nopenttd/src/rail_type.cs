/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file rail_type.h The different types of rail */

using System;

namespace Nopenttd
{
    public struct RailTypeLabel
    {
        public string Label { get; set; }

        public RailTypeLabel(string label)
        {
            Label = label;
        }

        public static implicit operator string(RailTypeLabel label)
        {
            return label.Label;
        }

        public static implicit operator RailTypeLabel(string label)
        {
            return new RailTypeLabel(label);
        }
    }

    public class RailConstaints
    {
        //RailTypeLabel was originally a uint32
        public static readonly RailTypeLabel RAILTYPE_RAIL_LABEL = "RAIL";
        public static readonly RailTypeLabel RAILTYPE_ELECTRIC_LABEL = "ELRL";
        public static readonly RailTypeLabel RAILTYPE_MONO_LABEL = "MONO";
        public static readonly RailTypeLabel RAILTYPE_MAGLEV_LABEL = "MGLV";
    }

/**
 * Enumeration for all possible railtypes.
 *
 * This enumeration defines all 4 possible railtypes.
 */

    public enum RailType
    {
        /// Used for iterations
        RAILTYPE_BEGIN = 0,

        /// Standard non-electric rails      
        RAILTYPE_RAIL = 0,

        /// Electric rails      
        RAILTYPE_ELECTRIC = 1,

        /// Monorail      
        RAILTYPE_MONO = 2,

        /// Maglev      
        RAILTYPE_MAGLEV = 3,

        /// Used for iterations      
        RAILTYPE_END = 16,

        /// Flag for invalid railtype      
        INVALID_RAILTYPE = 0xFF,

        /// Default railtype: first available
        DEF_RAILTYPE_FIRST = RAILTYPE_END,

        /// Default railtype: last available
        DEF_RAILTYPE_LAST,

        /// Default railtype: most used
        DEF_RAILTYPE_MOST_USED,
    }

/** Allow incrementing of Track variables */
//DECLARE_POSTFIX_INCREMENT(RailType)
    ///** Define basic enum properties */
//template <> struct EnumPropsT<RailType> : MakeEnumPropsT<RailType, byte, RAILTYPE_BEGIN, RAILTYPE_END, INVALID_RAILTYPE, 4> {};
//typedef TinyEnumT<RailType> RailTypeByte;

/**
 * The different roadtypes we support, but then a bitmask of them
 */
    [Flags]
    public enum RailTypes
    {
        /// No rail types
        RAILTYPES_NONE = 0,

        /// Non-electrified rails
        RAILTYPES_RAIL = 1 << RailType.RAILTYPE_RAIL,

        /// Electrified rails
        RAILTYPES_ELECTRIC = 1 << RailType.RAILTYPE_ELECTRIC,

        /// Monorail!
        RAILTYPES_MONO = 1 << RailType.RAILTYPE_MONO,

        /// Ever fast maglev
        RAILTYPES_MAGLEV = 1 << RailType.RAILTYPE_MAGLEV,

        /// Invalid railtypes
        INVALID_RAILTYPES = Int32.MaxValue, //was UINT_MAX,               
    }

//DECLARE_ENUM_AS_BIT_SET(RailTypes)

}