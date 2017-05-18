/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file depot_map.h Map related accessors for depots. */

using System.Diagnostics;
using Nopenttd.src.Core.Exceptions;
using Nopenttd.Tiles;

namespace Nopenttd
{

    public static class DepotMap
    {
/**
 * Check if a tile is a depot and it is a depot of the given type.
 */
        public static bool IsDepotTypeTile(this TileIndex tile, TransportType type)
        {
            switch (type)
            {
                default: throw new NotReachedException();
                case TransportType.TRANSPORT_RAIL: return tile.IsRailDepotTile();
                case TransportType.TRANSPORT_ROAD: return tile.IsRoadDepotTile();
                case TransportType.TRANSPORT_WATER: return tile.IsShipDepotTile();
                case TransportType.TRANSPORT_AIR: return tile.IsHangarTile();
            }
        }

/**
 * Is the given tile a tile with a depot on it?
 * @param tile the tile to check
 * @return true if and only if there is a depot on the tile.
 */
        public static bool IsDepotTile(this TileIndex tile)
        {
            return tile.IsRailDepotTile() || tile.IsRoadDepotTile() || tile.IsShipDepotTile() || tile.IsHangarTile();
        }

/**
 * Get the index of which depot is attached to the tile.
 * @param t the tile
 * @pre IsRailDepotTile(t) || IsRoadDepotTile(t) || IsShipDepotTile(t)
 * @return DepotID
 */
        public static DepotID GetDepotIndex(TileIndex t)
        {
            /* Hangars don't have a Depot class, thus store no DepotID. */
            Debug.Assert(t.IsRailDepotTile() || t.IsRoadDepotTile() || t.IsShipDepotTile());
            return Map._m[t].m2;
        }

/**
 * Get the type of vehicles that can use a depot
 * @param t The tile
 * @pre IsDepotTile(t)
 * @return the type of vehicles that can use the depot
 */
        public static VehicleType GetDepotVehicleType(TileIndex t)
        {
            switch (TileMap.GetTileType(t))
            {
                default: throw new NotReachedException();
                case TileType.MP_RAILWAY: return VehicleType.VEH_TRAIN;
                case TileType.MP_ROAD: return VehicleType.VEH_ROAD;
                case TileType.MP_WATER: return VehicleType.VEH_SHIP;
                case TileType.MP_STATION: return VehicleType.VEH_AIRCRAFT;
            }
        }
    }
}