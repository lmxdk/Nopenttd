/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file track_type.h All types related to tracks */

/**
 * These are used to specify a single track.
 * Can be translated to a trackbit with TrackToTrackbit
 */

using System;

namespace Nopenttd
{
    enum Track
    {
        /// Used for iterations
        TRACK_BEGIN = 0,

        /// Track along the x-axis (north-east to south-west)
        TRACK_X = 0,

        /// Track along the y-axis (north-west to south-east)
        TRACK_Y = 1,

        /// Track in the upper corner of the tile (north)
        TRACK_UPPER = 2,

        /// Track in the lower corner of the tile (south)
        TRACK_LOWER = 3,

        /// Track in the left corner of the tile (west)
        TRACK_LEFT = 4,

        /// Track in the right corner of the tile (east)
        TRACK_RIGHT = 5,

        /// Used for iterations
        TRACK_END = 6,

        /// Flag for an invalid track
        INVALID_TRACK = 0xFF,
    }

/** Allow incrementing of Track variables */
//DECLARE_POSTFIX_INCREMENT(Track)
    ///** Define basic enum properties */
//template <> struct EnumPropsT<Track> : MakeEnumPropsT<Track, byte, TRACK_BEGIN, TRACK_END, INVALID_TRACK, 3> {};
//typedef TinyEnumT<Track> TrackByte;


/** Bitfield corresponding to Track */
    [Flags]
    enum TrackBits
    {
        /// No track
        TRACK_BIT_NONE = 0,

        /// X-axis track
        TRACK_BIT_X = 1 << Track.TRACK_X,

        /// Y-axis track
        TRACK_BIT_Y = 1 << Track.TRACK_Y,

        /// Upper track
        TRACK_BIT_UPPER = 1 << Track.TRACK_UPPER,

        /// Lower track
        TRACK_BIT_LOWER = 1 << Track.TRACK_LOWER,

        /// Left track
        TRACK_BIT_LEFT = 1 << Track.TRACK_LEFT,

        /// Right track
        TRACK_BIT_RIGHT = 1 << Track.TRACK_RIGHT,

        /// X-Y-axis cross
        TRACK_BIT_CROSS = TRACK_BIT_X | TRACK_BIT_Y,

        /// Upper and lower track
        TRACK_BIT_HORZ = TRACK_BIT_UPPER | TRACK_BIT_LOWER,

        /// Left and right track
        TRACK_BIT_VERT = TRACK_BIT_LEFT | TRACK_BIT_RIGHT,

        /// "Arrow" to the north-east
        TRACK_BIT_3WAY_NE = TRACK_BIT_X | TRACK_BIT_UPPER | TRACK_BIT_RIGHT,

        /// "Arrow" to the south-east
        TRACK_BIT_3WAY_SE = TRACK_BIT_Y | TRACK_BIT_LOWER | TRACK_BIT_RIGHT,

        /// "Arrow" to the south-west
        TRACK_BIT_3WAY_SW = TRACK_BIT_X | TRACK_BIT_LOWER | TRACK_BIT_LEFT,

        /// "Arrow" to the north-west
        TRACK_BIT_3WAY_NW = TRACK_BIT_Y | TRACK_BIT_UPPER | TRACK_BIT_LEFT,

        /// All possible tracks
        TRACK_BIT_ALL = TRACK_BIT_CROSS | TRACK_BIT_HORZ | TRACK_BIT_VERT,

        /// Bitmask for the first 6 bits
        TRACK_BIT_MASK = (int) 0x3FU,

        /// Bitflag for a wormhole (used for tunnels)
        TRACK_BIT_WORMHOLE = (int) 0x40U,

        /// Bitflag for a depot
        TRACK_BIT_DEPOT = (int) 0x80U,

        /// Flag for an invalid trackbits value
        INVALID_TRACK_BIT = (int) 0xFF,
    }

//typedef SimpleTinyEnumT<TrackBits, byte> TrackBitsByte;

/**
 * Enumeration for tracks and directions.
 *
 * These are a combination of tracks and directions. Values are 0-5 in one
 * direction (corresponding to the Track enum) and 8-13 in the other direction.
 * 6, 7, 14 and 15 are used to encode the reversing of road vehicles. Those
 * reversing track dirs are not considered to be 'valid' except in a small
 * corner in the road vehicle controller.
 */

    enum Trackdir
    {
        /// Used for iterations
        TRACKDIR_BEGIN = 0,

        /// X-axis and direction to north-east    
        TRACKDIR_X_NE = 0,

        /// Y-axis and direction to south-east    
        TRACKDIR_Y_SE = 1,

        /// Upper track and direction to east    
        TRACKDIR_UPPER_E = 2,

        /// Lower track and direction to east    
        TRACKDIR_LOWER_E = 3,

        /// Left track and direction to south    
        TRACKDIR_LEFT_S = 4,

        /// Right track and direction to south    
        TRACKDIR_RIGHT_S = 5,

        /// (Road vehicle) reverse direction north-east    
        TRACKDIR_RVREV_NE = 6,

        /// (Road vehicle) reverse direction south-east    
        TRACKDIR_RVREV_SE = 7,

        /// X-axis and direction to south-west    
        TRACKDIR_X_SW = 8,

        /// Y-axis and direction to north-west    
        TRACKDIR_Y_NW = 9,

        /// Upper track and direction to west    
        TRACKDIR_UPPER_W = 10,

        /// Lower track and direction to west    
        TRACKDIR_LOWER_W = 11,

        /// Left track and direction to north    
        TRACKDIR_LEFT_N = 12,

        /// Right track and direction to north    
        TRACKDIR_RIGHT_N = 13,

        /// (Road vehicle) reverse direction south-west    
        TRACKDIR_RVREV_SW = 14,

        /// (Road vehicle) reverse direction north-west    
        TRACKDIR_RVREV_NW = 15,

        /// Used for iterations    
        TRACKDIR_END,

        /// Flag for an invalid trackdir    
        INVALID_TRACKDIR = 0xFF,
    };

/** Define basic enum properties */
//template <> struct EnumPropsT<Trackdir> : MakeEnumPropsT<Trackdir, byte, TRACKDIR_BEGIN, TRACKDIR_END, INVALID_TRACKDIR, 4> {};
//typedef TinyEnumT<Trackdir> TrackdirByte;

/**
 * Enumeration of bitmasks for the TrackDirs
 *
 * These are a combination of tracks and directions. Values are 0-5 in one
 * direction (corresponding to the Track enum) and 8-13 in the other direction.
 */

    [Flags]
    enum TrackdirBits
    {
        /// No track build
        TRACKDIR_BIT_NONE = 0x0000,

        /// Track x-axis, direction north-east
        TRACKDIR_BIT_X_NE = 0x0001,

        /// Track y-axis, direction south-east
        TRACKDIR_BIT_Y_SE = 0x0002,

        /// Track upper, direction east
        TRACKDIR_BIT_UPPER_E = 0x0004,

        /// Track lower, direction east
        TRACKDIR_BIT_LOWER_E = 0x0008,

        /// Track left, direction south
        TRACKDIR_BIT_LEFT_S = 0x0010,

        /// Track right, direction south
        TRACKDIR_BIT_RIGHT_S = 0x0020,
        /* Again, note the two missing values here. This enables trackdir -> track conversion by doing (trackdir & 0xFF) */

        /// Track x-axis, direction south-west
        TRACKDIR_BIT_X_SW = 0x0100,

        /// Track y-axis, direction north-west
        TRACKDIR_BIT_Y_NW = 0x0200,

        /// Track upper, direction west
        TRACKDIR_BIT_UPPER_W = 0x0400,

        /// Track lower, direction west
        TRACKDIR_BIT_LOWER_W = 0x0800,

        /// Track left, direction north
        TRACKDIR_BIT_LEFT_N = 0x1000,

        /// Track right, direction north
        TRACKDIR_BIT_RIGHT_N = 0x2000,

        /// Bitmask for bit-operations
        TRACKDIR_BIT_MASK = 0x3F3F,

        /// Flag for an invalid trackdirbit value
        INVALID_TRACKDIR_BIT = 0xFFFF,
    }

//typedef SimpleTinyEnumT<TrackdirBits, uint16> TrackdirBitsShort;

//typedef uint32 TrackStatus;

}