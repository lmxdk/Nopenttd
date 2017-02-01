/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file road_type.h Enums and other types related to roads. */

using System;

namespace Nopenttd
{


/**
 * The different roadtypes we support
 *
 * @note currently only ROADTYPE_ROAD and ROADTYPE_TRAM are supported.
 */

    public enum RoadType
    {
        /// Used for iterations
        ROADTYPE_BEGIN = 0,

        /// Basic road type 
        ROADTYPE_ROAD = 0,

        /// Trams 
        ROADTYPE_TRAM = 1,

        /// Used for iterations 
        ROADTYPE_END = 2,

        /// flag for invalid roadtype 
        INVALID_ROADTYPE = 0xFF,
    }

//DECLARE_POSTFIX_INCREMENT(RoadType)
//template <> struct EnumPropsT<RoadType> : MakeEnumPropsT<RoadType, byte, ROADTYPE_BEGIN, ROADTYPE_END, INVALID_ROADTYPE, 2> {};

/**
 * The different roadtypes we support, but then a bitmask of them
 * @note currently only roadtypes with ROADTYPE_ROAD and ROADTYPE_TRAM are supported.
 */

    [Flags]
    public enum RoadTypes
    {
        /// No roadtypes
        ROADTYPES_NONE = 0,

        /// Road 
        ROADTYPES_ROAD = 1 << RoadType.ROADTYPE_ROAD,

        /// Trams 
        ROADTYPES_TRAM = 1 << RoadType.ROADTYPE_TRAM,

        /// Road + trams 
        ROADTYPES_ALL = ROADTYPES_ROAD | ROADTYPES_TRAM,

        /// Used for iterations? 
        ROADTYPES_END,

        /// Invalid roadtypes 
        INVALID_ROADTYPES = 0xFF,
    }

//DECLARE_ENUM_AS_BIT_SET(RoadTypes)
//template <> struct EnumPropsT<RoadTypes> : MakeEnumPropsT<RoadTypes, byte, ROADTYPES_NONE, ROADTYPES_END, INVALID_ROADTYPES, 2> {};
//typedef SimpleTinyEnumT<RoadTypes, byte> RoadTypesByte;


/**
 * Enumeration for the road parts on a tile.
 *
 * This enumeration defines the possible road parts which
 * can be build on a tile.
 */

    [Flags]
    public enum RoadBits
    {
        /// No road-part is build
        ROAD_NONE = 0,

        /// North-west part  
        ROAD_NW = 1,

        /// South-west part  
        ROAD_SW = 2,

        /// South-east part  
        ROAD_SE = 4,

        /// North-east part  
        ROAD_NE = 8,

        /// Full road along the x-axis (south-west + north-east)  
        ROAD_X = ROAD_SW | ROAD_NE,

        /// Full road along the y-axis (north-west + south-east)  
        ROAD_Y = ROAD_NW | ROAD_SE,

        /// Road at the two northern edges
        ROAD_N = ROAD_NE | ROAD_NW,

        /// Road at the two eastern edges  
        ROAD_E = ROAD_NE | ROAD_SE,

        /// Road at the two southern edges  
        ROAD_S = ROAD_SE | ROAD_SW,

        /// Road at the two western edges  
        ROAD_W = ROAD_NW | ROAD_SW,

        /// Full 4-way crossing
        ROAD_ALL = ROAD_X | ROAD_Y,

        /// Out-of-range roadbits, used for iterations
        ROAD_END = ROAD_ALL + 1
    }

//DECLARE_ENUM_AS_BIT_SET(RoadBits)
//template <> struct EnumPropsT<RoadBits> : MakeEnumPropsT<RoadBits, byte, ROAD_NONE, ROAD_END, ROAD_NONE, 4> {};
}