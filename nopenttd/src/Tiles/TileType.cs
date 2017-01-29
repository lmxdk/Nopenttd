/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see http://www.gnu.org/licenses/>.
 */

/** @file tile_type.h Types related to tiles. */

namespace Nopenttd.Tiles
{
/**
 * The different types of tiles.
 *
 * Each tile belongs to one type, according whatever is build on it.
 *
 * @note A railway with a crossing street is marked as MP_ROAD.
 */

    public enum TileType
    {
        MP_CLEAR,

        /// A tile without any structures, i.e. grass, rocks, farm fields etc.
        MP_RAILWAY,

        /// A railway
        MP_ROAD,

        /// A tile with road (or tram tracks)
        MP_HOUSE,

        /// A house by a town
        MP_TREES,

        /// Tile got trees
        MP_STATION,

        /// A tile of a station
        MP_WATER,

        /// Water tile
        MP_VOID,

        /// Invisible tiles at the SW and SE border
        MP_INDUSTRY,

        /// Part of an industry
        MP_TUNNELBRIDGE,

        /// Tunnel entry/exit and bridge heads
        MP_OBJECT /// Contains objects such as transmitters and owned land
    }
}