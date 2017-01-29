using System;

namespace Nopenttd.Slopes
{

    /**
     * Enumeration for the slope-type.
     *
     * This enumeration use the chars N,E,S,W corresponding the
     * direction north, east, south and west. The top corner of a tile
     * is the north-part of the tile. The whole slope is encoded with
     * 5 bits, 4 bits for each corner and 1 bit for a steep-flag.
     *
     * For halftile slopes an extra 3 bits are used to represent this
     * properly; 1 bit for a halftile-flag and 2 bits to encode which
     * extra side (corner) is leveled when the slope of the first 5
     * bits is applied. This means that there can only be one leveled
     * slope for steep slopes, which is logical because two leveled
     * slopes would mean that it is not a steep slope as halftile
     * slopes only span one height level.
     */

    [Flags]
    public enum Slope
    {
        /// a flat tile						
        SLOPE_FLAT = 0x00,

        /// the west corner of the tile is raised
        SLOPE_W = 0x01,

        /// the south corner of the tile is raised
        SLOPE_S = 0x02,

        /// the east corner of the tile is raised
        SLOPE_E = 0x04,

        /// the north corner of the tile is raised
        SLOPE_N = 0x08,

        /// indicates the slope is steep
        SLOPE_STEEP = 0x10,

        /// north and west corner are raised
        SLOPE_NW = SLOPE_N | SLOPE_W,

        /// south and west corner are raised
        SLOPE_SW = SLOPE_S | SLOPE_W,

        /// south and east corner are raised
        SLOPE_SE = SLOPE_S | SLOPE_E,

        /// north and east corner are raised
        SLOPE_NE = SLOPE_N | SLOPE_E,

        /// east and west corner are raised
        SLOPE_EW = SLOPE_E | SLOPE_W,

        /// north and south corner are raised
        SLOPE_NS = SLOPE_N | SLOPE_S,

        /// bit mask containing all 'simple' slopes
        SLOPE_ELEVATED = SLOPE_N | SLOPE_E | SLOPE_S | SLOPE_W,

        /// north, west and south corner are raised
        SLOPE_NWS = SLOPE_N | SLOPE_W | SLOPE_S,

        /// west, south and east corner are raised
        SLOPE_WSE = SLOPE_W | SLOPE_S | SLOPE_E,

        /// south, east and north corner are raised
        SLOPE_SEN = SLOPE_S | SLOPE_E | SLOPE_N,

        /// east, north and west corner are raised
        SLOPE_ENW = SLOPE_E | SLOPE_N | SLOPE_W,

        /// a steep slope falling to east (from west)
        SLOPE_STEEP_W = SLOPE_STEEP | SLOPE_NWS,

        /// a steep slope falling to north (from south)
        SLOPE_STEEP_S = SLOPE_STEEP | SLOPE_WSE,

        /// a steep slope falling to west (from east)
        SLOPE_STEEP_E = SLOPE_STEEP | SLOPE_SEN,

        /// a steep slope falling to south (from north)
        SLOPE_STEEP_N = SLOPE_STEEP | SLOPE_ENW,

        /// one halftile is leveled (non continuous slope)
        SLOPE_HALFTILE = 0x20,

        /// three bits used for halftile slopes
        SLOPE_HALFTILE_MASK = 0xE0,

        /// the west halftile is leveled (non continuous slope)
        SLOPE_HALFTILE_W = SLOPE_HALFTILE | (Corner.CORNER_W << 6),

        /// the south halftile is leveled (non continuous slope)
        SLOPE_HALFTILE_S = SLOPE_HALFTILE | (Corner.CORNER_S << 6),

        /// the east halftile is leveled (non continuous slope)
        SLOPE_HALFTILE_E = SLOPE_HALFTILE | (Corner.CORNER_E << 6),

        /// the north halftile is leveled (non continuous slope)
        SLOPE_HALFTILE_N = SLOPE_HALFTILE | (Corner.CORNER_N << 6),
    }
}