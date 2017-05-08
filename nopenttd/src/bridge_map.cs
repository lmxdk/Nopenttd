/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file bridge_map.h Map accessor functions for bridges. */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.Slopes;
using Nopenttd.Tiles;

namespace Nopenttd
{
    public static class BridgeMap
    {

/**
 * Checks if this is a bridge, instead of a tunnel
 * @param t The tile to analyze
 * @pre IsTileType(t, MP_TUNNELBRIDGE)
 * @return true if the structure is a bridge one
 */
        public static bool IsBridge(TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            return BitMath.HasBit(Map._m[t].m5, 7);
        }

/**
 * checks if there is a bridge on this tile
 * @param t The tile to analyze
 * @return true if a bridge is present
 */
        public static bool IsBridgeTile(TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE) && IsBridge(t);
        }

        /**
         * checks if a bridge is set above the ground of this tile
         * @param t The tile to analyze
         * @return true if a bridge is detected above
         */
        public static bool IsBridgeAbove(TileIndex t)
        {
            return BitMath.GB(Map._m[t].type, 2, 2) != 0;
        }

        /**
         * Determines the type of bridge on a tile
         * @param t The tile to analyze
         * @pre IsBridgeTile(t)
         * @return The bridge type
         */
        public static BridgeType GetBridgeType(TileIndex t)
        {
            Debug.Assert(IsBridgeTile(t));
            return BitMath.GB(Map._me[t].m6, 2, 4);
        }

        /**
         * Get the axis of the bridge that goes over the tile. Not the axis or the ramp.
         * @param t The tile to analyze
         * @pre IsBridgeAbove(t)
         * @return the above mentioned axis
         */
        public static Axis GetBridgeAxis(TileIndex t)
        {
            Debug.Assert(IsBridgeAbove(t));
            return (Axis) (BitMath.GB(Map._m[t].type, 2, 2) - 1);
        }


        /**
         * Finds the end of a bridge in the specified direction starting at a middle tile
         * @param tile the bridge tile to find the bridge ramp for
         * @param dir  the direction to search in
         */
        public static TileIndex GetBridgeEnd(TileIndex tile, DiagDirection dir)
        {
            TileIndexDiff delta = Map.TileOffsByDiagDir(dir);

            dir = dir.ReverseDiagDir();
            do
            {
                tile += (uint)delta.Difference;
            } while (!IsBridgeTile(tile) || GetTunnelBridgeDirection(tile) != dir);

            return tile;
        }


        /**
         * Finds the northern end of a bridge starting at a middle tile
         * @param t the bridge tile to find the bridge ramp for
         */
        public static TileIndex GetNorthernBridgeEnd(TileIndex t)
        {
            return GetBridgeEnd(t, GetBridgeAxis(t).AxisToDiagDir().ReverseDiagDir());
        }


        /**
         * Finds the southern end of a bridge starting at a middle tile
         * @param t the bridge tile to find the bridge ramp for
         */
        public static TileIndex GetSouthernBridgeEnd(TileIndex t)
        {
            return GetBridgeEnd(t, GetBridgeAxis(t).AxisToDiagDir());
        }


        /**
         * Starting at one bridge end finds the other bridge end
         * @param t the bridge ramp tile to find the other bridge ramp for
         */
        public static TileIndex GetOtherBridgeEnd(TileIndex tile)
        {
            Debug.Assert(IsBridgeTile(tile));
            return GetBridgeEnd(tile, GetTunnelBridgeDirection(tile));
        }

        /**
         * Get the height ('z') of a bridge.
         * @param tile the bridge ramp tile to get the bridge height from
         * @return the height of the bridge.
         */
        int GetBridgeHeight(TileIndex t)
        {
            int h;
            Slope tileh = TileMap.GetTileSlope(t, ref h);
            Foundation f = GetBridgeFoundation(tileh, DiagDirToAxis(GetTunnelBridgeDirection(t)));

            /* one height level extra for the ramp */
            return h + 1 + ApplyFoundationToSlope(f, ref tileh);
        }


        /**
         * Get the height ('z') of a bridge in pixels.
         * @param tile the bridge ramp tile to get the bridge height from
         * @return the height of the bridge in pixels
         */
        public static int GetBridgePixelHeight(TileIndex tile)
        {
            return (int)(GetBridgeHeight(tile) * TileConstants.TILE_HEIGHT);
        }

/**
 * Remove the bridge over the given axis.
 * @param t the tile to remove the bridge from
 * @param a the axis of the bridge to remove
 */
        public static void ClearSingleBridgeMiddle(TileIndex t, Axis a)
        {
            Map._m[t].type = (byte)BitMath.ClrBit(Map._m[t].type, (byte)(2 + (byte)a));
        }

/**
 * Removes bridges from the given, that is bridges along the X and Y axis.
 * @param t the tile to remove the bridge from
 */
        public static void ClearBridgeMiddle(TileIndex t)
        {
            ClearSingleBridgeMiddle(t, Axis.AXIS_X);
            ClearSingleBridgeMiddle(t, Axis.AXIS_Y);
        }

/**
 * Set that there is a bridge over the given axis.
 * @param t the tile to add the bridge to
 * @param a the axis of the bridge to add
 */
        public static void SetBridgeMiddle(TileIndex t, Axis a)
        {
            Map._m[t].type = (byte)BitMath.SetBit(Map._m[t].type, (byte)(2 + (byte)a));
        }

/**
 * Generic part to make a bridge ramp for both roads and rails.
 * @param t          the tile to make a bridge ramp
 * @param o          the new owner of the bridge ramp
 * @param bridgetype the type of bridge this bridge ramp belongs to
 * @param d          the direction this ramp must be facing
 * @param tt         the transport type of the bridge
 * @param rt         the road or rail type
 * @note this function should not be called directly.
 */
        public static void MakeBridgeRamp(TileIndex t, Owner o, BridgeType bridgetype, DiagDirection d,
            TransportType tt, RailType rt)
        {
            TileMap.SetTileType(t, TileType.MP_TUNNELBRIDGE);
            TileMap.SetTileOwner(t, o);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = (byte)rt;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = (byte)(1 << 7 | (byte)tt << 2 | (byte)d);
            BitMath.SB(ref Map._me[t].m6, 2, 4, bridgetype);
            Map._me[t].m7 = 0;
        }

/**
 * Make a bridge ramp for roads.
 * @param t          the tile to make a bridge ramp
 * @param o          the new owner of the bridge ramp
 * @param owner_road the new owner of the road on the bridge
 * @param owner_tram the new owner of the tram on the bridge
 * @param bridgetype the type of bridge this bridge ramp belongs to
 * @param d          the direction this ramp must be facing
 * @param r          the road type of the bridge
 */
        public static void MakeRoadBridgeRamp(TileIndex t, Owner o, Owner owner_road, Owner owner_tram,
            BridgeType bridgetype, DiagDirection d, RoadTypes r)
        {
            MakeBridgeRamp(t, o, bridgetype, d, TransportType.TRANSPORT_ROAD, 0);
            SetRoadOwner(t, RoadType.ROADTYPE_ROAD, owner_road);
            if (owner_tram != Owner.OWNER_TOWN) SetRoadOwner(t, RoadType.ROADTYPE_TRAM, owner_tram);
            SetRoadTypes(t, r);
        }

/**
 * Make a bridge ramp for rails.
 * @param t          the tile to make a bridge ramp
 * @param o          the new owner of the bridge ramp
 * @param bridgetype the type of bridge this bridge ramp belongs to
 * @param d          the direction this ramp must be facing
 * @param r          the rail type of the bridge
 */
        public static void MakeRailBridgeRamp(TileIndex t, Owner o, BridgeType bridgetype, DiagDirection d, RailType r)
        {
            MakeBridgeRamp(t, o, bridgetype, d, TransportType.TRANSPORT_RAIL, r);
        }

/**
 * Make a bridge ramp for aqueducts.
 * @param t          the tile to make a bridge ramp
 * @param o          the new owner of the bridge ramp
 * @param d          the direction this ramp must be facing
 */
        public static void MakeAqueductBridgeRamp(TileIndex t, Owner o, DiagDirection d)
        {
            MakeBridgeRamp(t, o, 0, d, TransportType.TRANSPORT_WATER, 0);
        }
    }
}