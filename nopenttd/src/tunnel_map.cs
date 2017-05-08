/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tunnel_map.h Map accessors for tunnels. */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.Tiles;

namespace Nopenttd
{
    public static class TunnelMap
    {
/**
 * Is this a tunnel (entrance)?
 * @param t the tile that might be a tunnel
 * @pre TileMap.IsTileType(t, MP_TUNNELBRIDGE)	
 * @return true if and only if this tile is a tunnel (entrance)
 */
        public static bool IsTunnel(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            return !BitMath.HasBit(Map._m[t].m5, 7);
        }

/**
 * Is this a tunnel (entrance)?
 * @param t the tile that might be a tunnel
 * @return true if and only if this tile is a tunnel (entrance)
 */
        public static bool IsTunnelTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE) && IsTunnel(t);
        }

/**
 * Makes a road tunnel entrance
 * @param t the entrance of the tunnel
 * @param o the owner of the entrance
 * @param d the direction facing out of the tunnel
 * @param r the road type used in the tunnel
 */
        public static void MakeRoadTunnel(this TileIndex t, Owner o, DiagDirection d, RoadTypes r)
        {
            TileMap.SetTileType(t, TileType.MP_TUNNELBRIDGE);
            TileMap.SetTileOwner(t, o);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = (byte)((int)TransportType.TRANSPORT_ROAD << 2 | (int)d);
            Map._me[t].m6 = BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
            t.SetRoadOwner(RoadType.ROADTYPE_ROAD, o);
            if (o != Owner.OWNER_TOWN) t.SetRoadOwner(RoadType.ROADTYPE_TRAM, o);
            t.SetRoadTypes(r);
        }

/**
 * Makes a rail tunnel entrance
 * @param t the entrance of the tunnel
 * @param o the owner of the entrance
 * @param d the direction facing out of the tunnel
 * @param r the rail type used in the tunnel
 */
        public static void MakeRailTunnel(this TileIndex t, Owner o, DiagDirection d, RailType r)
        {
            TileMap.SetTileType(t, TileType.MP_TUNNELBRIDGE);
            TileMap.SetTileOwner(t, o);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = (byte) r;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = (byte)((int)TransportType.TRANSPORT_RAIL << 2 | d);
            Map._me[t].m6 = BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }
    

    /**
     * Gets the other end of the tunnel. Where a vehicle would reappear when it
     * enters at the given tile.
     * @param tile the tile to search from.
     * @return the tile of the other end of the tunnel.
     */
    public static TileIndex GetOtherTunnelEnd(this TileIndex tile)
    {
        DiagDirection dir = GetTunnelBridgeDirection(tile);
        TileIndexDiff delta = TileOffsByDiagDir(dir);
        int z = TileMap.GetTileZ(tile);

        dir = ReverseDiagDir(dir);
        do
        {
            tile += delta;
        } while (
            !IsTunnelTile(tile) ||
            GetTunnelBridgeDirection(tile) != dir ||
            TileMap.GetTileZ(tile) != z
        );

        return tile;
    }


    /**
     * Is there a tunnel in the way in the given direction?
     * @param tile the tile to search from.
     * @param z    the 'z' to search on.
     * @param dir  the direction to start searching to.
     * @return true if and only if there is a tunnel.
     */
public static bool IsTunnelInWayDir(this TileIndex tile, int z, DiagDirection dir)
    {
        TileIndexDiff delta = TileOffsByDiagDir(dir);
        int height;

        do
        {
            tile -= delta;
            if (!TileMap.IsValidTile(tile)) return false;
            height = TileMap.GetTileZ(tile);
        } while (z < height);

        return z == height && IsTunnelTile(tile) && GetTunnelBridgeDirection(tile) == dir;
    }

    /**
     * Is there a tunnel in the way in any direction?
     * @param tile the tile to search from.
     * @param z the 'z' to search on.
     * @return true if and only if there is a tunnel.
     */
    public static bool IsTunnelInWay(this TileIndex tile, int z)
    {
        return IsTunnelInWayDir(tile, z, (Map.TileX(tile) > (Map.MapMaxX() / 2)) ? DiagDirection.DIAGDIR_NE : DiagDirection.DIAGDIR_SW) ||
                IsTunnelInWayDir(tile, z, (Map.TileY(tile) > (Map.MapMaxY() / 2)) ? DiagDirection.DIAGDIR_NW : DiagDirection.DIAGDIR_SE);
    }
    }

}