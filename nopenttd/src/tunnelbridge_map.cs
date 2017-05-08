/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tunnelbridge_map.h Functions that have tunnels and bridges in common */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.Tiles;

namespace Nopenttd
{

    public static class TunnelBridgeMap
    {

/**
 * Get the direction pointing to the other end.
 *
 * Tunnel: Get the direction facing into the tunnel
 * Bridge: Get the direction pointing onto the bridge
 * @param t The tile to analyze
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE)
 * @return the above mentioned direction
 */
        public static DiagDirection GetTunnelBridgeDirection(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            return (DiagDirection) BitMath.GB(Map._m[t].m5, 0, 2);
        }

/**
 * Tunnel: Get the transport type of the tunnel (road or rail)
 * Bridge: Get the transport type of the bridge's ramp
 * @param t The tile to analyze
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE)
 * @return the transport type in the tunnel/bridge
 */
        public static TransportType GetTunnelBridgeTransportType(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            return (TransportType) BitMath.GB(Map._m[t].m5, 2, 2);
        }

/**
 * Tunnel: Is this tunnel entrance in a snowy or desert area?
 * Bridge: Does the bridge ramp lie in a snow or desert area?
 * @param t The tile to analyze
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE)
 * @return true if and only if the tile is in a snowy/desert area
 */
        public static bool HasTunnelBridgeSnowOrDesert(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            return BitMath.HasBit(Map._me[t].m7, 5);
        }

/**
 * Tunnel: Places this tunnel entrance in a snowy or desert area, or takes it out of there.
 * Bridge: Sets whether the bridge ramp lies in a snow or desert area.
 * @param t the tunnel entrance / bridge ramp tile
 * @param snow_or_desert is the entrance/ramp in snow or desert (true), when
 *                       not in snow and not in desert false
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE)
 */
        public static void SetTunnelBridgeSnowOrDesert(this TileIndex t, bool snow_or_desert)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            Map._me[t].m7 = BitMath.SB(Map._me[t].m7, 5, 1, snow_or_desert);
        }

/**
 * Determines type of the wormhole and returns its other end
 * @param t one end
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE)
 * @return other end
 */
        public static TileIndex GetOtherTunnelBridgeEnd(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            return t.IsTunnel() ? t.GetOtherTunnelEnd() : GetOtherBridgeEnd(t);
        }


/**
 * Get the reservation state of the rail tunnel/bridge
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE) && GetTunnelBridgeTransportType(t) == TRANSPORT_RAIL
 * @param t the tile
 * @return reservation state
 */
        public static bool HasTunnelBridgeReservation(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            Debug.Assert(GetTunnelBridgeTransportType(t) == TransportType.TRANSPORT_RAIL);
            return BitMath.HasBit(Map._m[t].m5, 4);
        }

/**
 * Set the reservation state of the rail tunnel/bridge
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE) && GetTunnelBridgeTransportType(t) == TRANSPORT_RAIL
 * @param t the tile
 * @param b the reservation state
 */
        public static void SetTunnelBridgeReservation(this TileIndex t, bool b)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            Debug.Assert(GetTunnelBridgeTransportType(t) == TransportType.TRANSPORT_RAIL);
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 4, 1, b ? 1 : 0);
        }

/**
 * Get the reserved track bits for a rail tunnel/bridge
 * @pre TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE) && GetTunnelBridgeTransportType(t) == TRANSPORT_RAIL
 * @param t the tile
 * @return reserved track bits
 */
        public static TrackBits GetTunnelBridgeReservationTrackBits(this TileIndex t)
        {
            return HasTunnelBridgeReservation(t)
                ? DiagDirToDiagTrackBits(GetTunnelBridgeDirection(t))
                : TrackBits.TRACK_BIT_NONE;
        }

    }
}