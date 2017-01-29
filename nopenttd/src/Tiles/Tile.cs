/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file map_type.h Types related to maps. */

namespace Nopenttd.Tiles
{


/**
 * Data that is stored per tile. Also used TileExtended for this.
 * Look at docs/landscape.html for the exact meaning of the members.
 */

    public struct Tile
    {
        /// The type (bits 4..7), bridges (2..3), rainforest/desert (0..1)
        byte type;

        /// The height of the northern corner.  
        byte height;

        /// Primarily used for indices to towns, industries and stations  
        ushort m2;

        /// Primarily used for ownership information  
        byte m1;

        /// General purpose  
        byte m3;

        /// General purpose  
        byte m4;

        /// General purpose  
        byte m5;

        //assert_compile(sizeof(Tile) == 8);
    }
}