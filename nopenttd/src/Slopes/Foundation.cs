/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/**
 * @file slope_type.h Definitions of a slope.
 * This file defines the enumeration and helper functions for handling
 * the slope info of a tile.
 */

namespace Nopenttd.Slopes
{


    /**
     * Enumeration for Foundations.
     */

    public enum Foundation
    {
        /// The tile has no foundation, the slope remains unchanged.
        FOUNDATION_NONE,

        /// The tile is leveled up to a flat slope. 
        FOUNDATION_LEVELED,

        /// The tile has an along X-axis inclined foundation. 
        FOUNDATION_INCLINED_X,

        /// The tile has an along Y-axis inclined foundation. 
        FOUNDATION_INCLINED_Y,

        /// The tile has a steep slope. The lowest corner is raised by a foundation to allow building railroad on the lower halftile. 
        FOUNDATION_STEEP_LOWER,

        /* Halftile foundations */

        /// The tile has a steep slope. The lowest corner is raised by a foundation and the upper halftile is leveled.
        FOUNDATION_STEEP_BOTH,

        /// Level west halftile non-continuously. 
        FOUNDATION_HALFTILE_W,

        /// Level south halftile non-continuously. 
        FOUNDATION_HALFTILE_S,

        /// Level east halftile non-continuously. 
        FOUNDATION_HALFTILE_E,

        /// Level north halftile non-continuously. 
        FOUNDATION_HALFTILE_N,

        /* Special anti-zig-zag foundations for single horizontal/vertical track */

        /// Foundation for TRACK_BIT_LEFT, but not a leveled foundation.
        FOUNDATION_RAIL_W,

        /// Foundation for TRACK_BIT_LOWER, but not a leveled foundation.
        FOUNDATION_RAIL_S,

        /// Foundation for TRACK_BIT_RIGHT, but not a leveled foundation.
        FOUNDATION_RAIL_E,

        /// Foundation for TRACK_BIT_UPPER, but not a leveled foundation.
        FOUNDATION_RAIL_N,

        /// Used inside "rail_cmd.cpp" to indicate invalid slope/track combination.
        FOUNDATION_INVALID = 0xFF,
    };

}