/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file road_map.h Map accessors for roads. */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.src.Core.Exceptions;
using Nopenttd.Tiles;

namespace Nopenttd
{


    /** Which directions are disallowed ? */
    enum DisallowedRoadDirections
    {
        /// None of the directions are disallowed
        DRD_NONE,

        /// All southbound traffic is disallowed
        DRD_SOUTHBOUND,

        /// All northbound traffic is disallowed
        DRD_NORTHBOUND,

        /// All directions are disallowed
        DRD_BOTH,

        /// Sentinel
        DRD_END,
    };
    //DECLARE_ENUM_AS_BIT_SET(DisallowedRoadDirections)
/** Helper information for extract tool. */
    //template<> struct EnumPropsT<DisallowedRoadDirections> : MakeEnumPropsT<DisallowedRoadDirections, byte, DRD_NONE, DRD_END, DRD_END, 2> { };



    /** The different types of road tiles. */
    public enum RoadTileType
    {
        /// Normal road
        ROAD_TILE_NORMAL,

        /// Level crossing
        ROAD_TILE_CROSSING,

        /// Depot (one entrance)
        ROAD_TILE_DEPOT,
    };



    /** The possible road side decorations. */
    public enum Roadside
    {
        /// Road on barren land
        ROADSIDE_BARREN = 0,

        /// Road on grass 
        ROADSIDE_GRASS = 1,

        /// Road with paved sidewalks 
        ROADSIDE_PAVED = 2,

        /// Road with street lights on paved sidewalks 
        ROADSIDE_STREET_LIGHTS = 3,

        /// Road with trees on paved sidewalks 
        ROADSIDE_TREES = 5,

        /// Road on grass with road works 
        ROADSIDE_GRASS_ROAD_WORKS = 6,

        /// Road with sidewalks and road works 
        ROADSIDE_PAVED_ROAD_WORKS = 7,
    };


    public static class RoadMap
    {
/**
 * Get the type of the road tile.
 * @param t Tile to query.
 * @pre IsTileType(t, MP_ROAD)
 * @return The road tile type.
 */
        public static RoadTileType GetRoadTileType(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_ROAD));
            return (RoadTileType) BitMath.GB(Map._m[t].m5, 6, 2);
        }

/**
 * Return whether a tile is a normal road.
 * @param t Tile to query.
 * @pre IsTileType(t, MP_ROAD)
 * @return True if normal road.
 */
        public static bool IsNormalRoad(this TileIndex t)
        {
            return GetRoadTileType(t) == RoadTileType.ROAD_TILE_NORMAL;
        }

/**
 * Return whether a tile is a normal road tile.
 * @param t Tile to query.
 * @return True if normal road tile.
 */
        public static bool IsNormalRoadTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_ROAD) && IsNormalRoad(t);
        }

/**
 * Return whether a tile is a level crossing.
 * @param t Tile to query.
 * @pre IsTileType(t, MP_ROAD)
 * @return True if level crossing.
 */
        public static bool IsLevelCrossing(this TileIndex t)
        {
            return GetRoadTileType(t) == RoadTileType.ROAD_TILE_CROSSING;
        }

/**
 * Return whether a tile is a level crossing tile.
 * @param t Tile to query.
 * @return True if level crossing tile.
 */
        public static bool IsLevelCrossingTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_ROAD) && IsLevelCrossing(t);
        }

/**
 * Return whether a tile is a road depot.
 * @param t Tile to query.
 * @pre IsTileType(t, MP_ROAD)
 * @return True if road depot.
 */
        public static bool IsRoadDepot(this TileIndex t)
        {
            return GetRoadTileType(t) == RoadTileType.ROAD_TILE_DEPOT;
        }

/**
 * Return whether a tile is a road depot tile.
 * @param t Tile to query.
 * @return True if road depot tile.
 */
        public static bool IsRoadDepotTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_ROAD) && IsRoadDepot(t);
        }

/**
 * Get the present road bits for a specific road type.
 * @param t  The tile to query.
 * @param rt Road type.
 * @pre IsNormalRoad(t)
 * @return The present road bits for the road type.
 */
        public static RoadBits GetRoadBits(this TileIndex t, RoadType rt)
        {
            Debug.Assert(IsNormalRoad(t));
            switch (rt)
            {
                default: throw new NotReachedException();
                case RoadType.ROADTYPE_ROAD: return (RoadBits) BitMath.GB(Map._m[t].m5, 0, 4);
                case RoadType.ROADTYPE_TRAM: return (RoadBits) BitMath.GB(Map._m[t].m3, 0, 4);
            }
        }

/**
 * Get all RoadBits set on a tile except from the given RoadType
 *
 * @param t The tile from which we want to get the RoadBits
 * @param rt The RoadType which we exclude from the querry
 * @return all set RoadBits of the tile which are not from the given RoadType
 */
        public static RoadBits GetOtherRoadBits(this TileIndex t, RoadType rt)
        {
            return GetRoadBits(t, rt == RoadType.ROADTYPE_ROAD ? RoadType.ROADTYPE_TRAM : RoadType.ROADTYPE_ROAD);
        }

/**
 * Get all set RoadBits on the given tile
 *
 * @param tile The tile from which we want to get the RoadBits
 * @return all set RoadBits of the tile
 */
        public static RoadBits GetAllRoadBits(TileIndex tile)
        {
            return GetRoadBits(tile, RoadType.ROADTYPE_ROAD) | GetRoadBits(tile, RoadType.ROADTYPE_TRAM);
        }

/**
 * Set the present road bits for a specific road type.
 * @param t  The tile to change.
 * @param r  The new road bits.
 * @param rt Road type.
 * @pre IsNormalRoad(t)
 */
        public static void SetRoadBits(this TileIndex t, RoadBits r, RoadType rt)
        {
            Debug.Assert(IsNormalRoad(t)); // XXX incomplete
            switch (rt)
            {
                default: throw new NotReachedException();
                case RoadType.ROADTYPE_ROAD:
                    Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 0, 4, r);
                    break;
                case RoadType.ROADTYPE_TRAM:
                    Map._m[t].m3 = BitMath.SB(Map._m[t].m3, 0, 4, r);
                    break;
            }
        }

/**
 * Get the present road types of a tile.
 * @param t The tile to query.
 * @return Present road types.
 */
        public static RoadTypes GetRoadTypes(this TileIndex t)
        {
            return (RoadTypes) BitMath.GB(Map._me[t].m7, 6, 2);
        }

/**
 * Set the present road types of a tile.
 * @param t  The tile to change.
 * @param rt The new road types.
 */
        public static void SetRoadTypes(this TileIndex t, RoadTypes rt)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_ROAD) || TileMap.IsTileType(t, TileType.MP_STATION) ||
                         TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            Map._me[t].m7 = BitMath.SB(Map._me[t].m7, 6, 2, rt);
        }

/**
 * Check if a tile has a specific road type.
 * @param t  The tile to check.
 * @param rt Road type to check.
 * @return True if the tile has the specified road type.
 */
        public static bool HasTileRoadType(this TileIndex t, RoadType rt)
        {
            return BitMath.HasBit(GetRoadTypes(t), rt);
        }

/**
 * Get the owner of a specific road type.
 * @param t  The tile to query.
 * @param rt The road type to get the owner of.
 * @return Owner of the given road type.
 */
        public static Owner GetRoadOwner(this TileIndex t, RoadType rt)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_ROAD) || TileMap.IsTileType(t, TileType.MP_STATION) ||
                         TileMap.IsTileType(t, TileType.MP_TUNNELBRIDGE));
            switch (rt)
            {
                default: throw new NotReachedException();
                case RoadType.ROADTYPE_ROAD:
                    return (Owner) BitMath.GB(IsNormalRoadTile(t) ? Map._m[t].m1 : Map._me[t].m7, 0, 5);
                case RoadType.ROADTYPE_TRAM:
                {
                    /* Trams don't need OWNER_TOWN, and remapping OWNER_NONE
                     * to OWNER_TOWN makes it use one bit less */
                    Owner o = (Owner) BitMath.GB(Map._m[t].m3, 4, 4);
                    return o == Owner.OWNER_TOWN ? Owner.OWNER_NONE : o;
                }
            }
        }

/**
 * Set the owner of a specific road type.
 * @param t  The tile to change.
 * @param rt The road type to change the owner of.
 * @param o  New owner of the given road type.
 */
        public static void SetRoadOwner(this TileIndex t, RoadType rt, Owner o)
        {
            switch (rt)
            {
                default: throw new NotReachedException();
                case RoadType.ROADTYPE_ROAD:
                {
                    if (IsNormalRoadTile(t))
                    {
                        Map._m[t].m1 = BitMath.SB(Map._m[t].m1, 0, 5, o);
                    }
                    else
                    {
                        Map._me[t].m7 = BitMath.SB(Map._me[t].m7, 0, 5, o);
                    }
                    break;
                }
                case RoadType.ROADTYPE_TRAM:
                    Map._m[t].m3 = BitMath.SB(Map._m[t].m3, 4, 4, o == Owner.OWNER_NONE ? Owner.OWNER_TOWN : o);
                    break;
            }
        }

/**
 * Check if a specific road type is owned by an owner.
 * @param t  The tile to query.
 * @param rt The road type to compare the owner of.
 * @param o  Owner to compare with.
 * @pre HasTileRoadType(t, rt)
 * @return True if the road type is owned by the given owner.
 */
        public static bool IsRoadOwner(this TileIndex t, RoadType rt, Owner o)
        {
            Debug.Assert(HasTileRoadType(t, rt));
            return (GetRoadOwner(t, rt) == o);
        }

/**
 * Checks if given tile has town owned road
 * @param t tile to check
 * @pre IsTileType(t, MP_ROAD)
 * @return true iff tile has road and the road is owned by a town
 */
        public static bool HasTownOwnedRoad(this TileIndex t)
        {
            return HasTileRoadType(t, RoadType.ROADTYPE_ROAD) &&
                   IsRoadOwner(t, RoadType.ROADTYPE_ROAD, Owner.OWNER_TOWN);
        }

/**
 * Gets the disallowed directions
 * @param t the tile to get the directions from
 * @return the disallowed directions
 */
        public static DisallowedRoadDirections GetDisallowedRoadDirections(this TileIndex t)
        {
            Debug.Assert(IsNormalRoad(t));
            return (DisallowedRoadDirections) BitMath.GB(Map._m[t].m5, 4, 2);
        }

/**
 * Sets the disallowed directions
 * @param t   the tile to set the directions for
 * @param drd the disallowed directions
 */
        public static void SetDisallowedRoadDirections(this TileIndex t, DisallowedRoadDirections drd)
        {
            Debug.Assert(IsNormalRoad(t));
            Debug.Assert(drd < DisallowedRoadDirections.DRD_END);
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 4, 2, drd);
        }

/**
 * Get the road axis of a level crossing.
 * @param t The tile to query.
 * @pre IsLevelCrossing(t)
 * @return The axis of the road.
 */
        public static Axis GetCrossingRoadAxis(this TileIndex t)
        {
            Debug.Assert(IsLevelCrossing(t));
            return (Axis) BitMath.GB(Map._m[t].m5, 0, 1);
        }

/**
 * Get the rail axis of a level crossing.
 * @param t The tile to query.
 * @pre IsLevelCrossing(t)
 * @return The axis of the rail.
 */
        public static Axis GetCrossingRailAxis(this TileIndex t)
        {
            Debug.Assert(IsLevelCrossing(t));
            return OtherAxis((Axis) GetCrossingRoadAxis(t));
        }

/**
 * Get the road bits of a level crossing.
 * @param tile The tile to query.
 * @return The present road bits.
 */
        public static RoadBits GetCrossingRoadBits(TileIndex tile)
        {
            return GetCrossingRoadAxis(tile) == Axis.AXIS_X ? RoadBits.ROAD_X : RoadBits.ROAD_Y;
        }

/**
 * Get the rail track of a level crossing.
 * @param tile The tile to query.
 * @return The rail track.
 */
        public static Track GetCrossingRailTrack(TileIndex tile)
        {
            return AxisToTrack(GetCrossingRailAxis(tile));
        }

/**
 * Get the rail track bits of a level crossing.
 * @param tile The tile to query.
 * @return The rail track bits.
 */
        public static TrackBits GetCrossingRailBits(TileIndex tile)
        {
            return AxisToTrackBits(GetCrossingRailAxis(tile));
        }


/**
 * Get the reservation state of the rail crossing
 * @param t the crossing tile
 * @return reservation state
 * @pre IsLevelCrossingTile(t)
 */
        public static bool HasCrossingReservation(this TileIndex t)
        {
            Debug.Assert(IsLevelCrossingTile(t));
            return BitMath.HasBit(Map._m[t].m5, 4);
        }

/**
 * Set the reservation state of the rail crossing
 * @note Works for both waypoints and rail depots
 * @param t the crossing tile
 * @param b the reservation state
 * @pre IsLevelCrossingTile(t)
 */
        public static void SetCrossingReservation(this TileIndex t, bool b)
        {
            Debug.Assert(IsLevelCrossingTile(t));
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 4, 1, b ? 1 : 0);
        }

/**
 * Get the reserved track bits for a rail crossing
 * @param t the tile
 * @pre IsLevelCrossingTile(t)
 * @return reserved track bits
 */
        public static TrackBits GetCrossingReservationTrackBits(this TileIndex t)
        {
            return HasCrossingReservation(t) ? GetCrossingRailBits(t) : TrackBits.TRACK_BIT_NONE;
        }

/**
 * Check if the level crossing is barred.
 * @param t The tile to query.
 * @pre IsLevelCrossing(t)
 * @return True if the level crossing is barred.
 */
        public static bool IsCrossingBarred(this TileIndex t)
        {
            Debug.Assert(IsLevelCrossing(t));
            return BitMath.HasBit(Map._m[t].m5, 5);
        }

/**
 * Set the bar state of a level crossing.
 * @param t The tile to modify.
 * @param barred True if the crossing should be barred, false otherwise.
 * @pre IsLevelCrossing(t)
 */
        public static void SetCrossingBarred(this TileIndex t, bool barred)
        {
            Debug.Assert(IsLevelCrossing(t));
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 5, 1, barred ? 1 : 0);
        }

/**
 * Unbar a level crossing.
 * @param t The tile to change.
 */
        public static void UnbarCrossing(this TileIndex t)
        {
            SetCrossingBarred(t, false);
        }

/**
 * Bar a level crossing.
 * @param t The tile to change.
 */
        public static void BarCrossing(this TileIndex t)
        {
            SetCrossingBarred(t, true);
        }

/** Check if a road tile has snow/desert. */
/**
 * Check if a road tile has snow/desert.
 * @param t The tile to query.
 * @return True if the tile has snow/desert.
 */
        public static bool IsOnSnowOrDesert(this TileIndex t) //IsOnDesert IsOnSnow
        {
            return BitMath.HasBit(Map._me[t].m7, 5);
        }

/** Toggle the snow/desert state of a road tile. */
/**
 * Toggle the snow/desert state of a road tile.
 * @param t The tile to change.
 */
        public static void ToggleSnowOrDesert(this TileIndex t) //ToggleDesert ToggleSnow
        {
            Map._me[t].m7 = BitMath.ToggleBit(Map._me[t].m7, 5);
        }

/**
 * Get the decorations of a road.
 * @param tile The tile to query.
 * @return The road decoration of the tile.
 */
        public static Roadside GetRoadside(TileIndex tile)
        {
            return (Roadside) BitMath.GB(Map._me[tile].m6, 3, 3);
        }

/**
 * Set the decorations of a road.
 * @param tile The tile to change.
 * @param s    The new road decoration of the tile.
 */
        public static void SetRoadside(TileIndex tile, Roadside s)
        {
            Map._me[tile].m6 = BitMath.SB(Map._me[tile].m6, 3, 3, s);
        }

/**
 * Check if a tile has road works.
 * @param t The tile to check.
 * @return True if the tile has road works in progress.
 */
        public static bool HasRoadWorks(this TileIndex t)
        {
            return GetRoadside(t) >= Roadside.ROADSIDE_GRASS_ROAD_WORKS;
        }

/**
 * Increase the progress counter of road works.
 * @param t The tile to modify.
 * @return True if the road works are in the last stage.
 */
        public static bool IncreaseRoadWorksCounter(this TileIndex t)
        {
            Map._me[t].m7 = BitMath.AB(Map._me[t].m7, 0, 4, 1);

            return BitMath.GB(Map._me[t].m7, 0, 4) == 15;
        }

/**
 * Start road works on a tile.
 * @param t The tile to start the work on.
 * @pre !HasRoadWorks(t)
 */
        public static void StartRoadWorks(this TileIndex t)
        {
            Debug.Assert(!HasRoadWorks(t));
            /* Remove any trees or lamps in case or roadwork */
            switch (GetRoadside(t))
            {
                case Roadside.ROADSIDE_BARREN:
                case Roadside.ROADSIDE_GRASS:
                    SetRoadside(t, Roadside.ROADSIDE_GRASS_ROAD_WORKS);
                    break;
                default:
                    SetRoadside(t, Roadside.ROADSIDE_PAVED_ROAD_WORKS);
                    break;
            }
        }

/**
 * Terminate road works on a tile.
 * @param t Tile to stop the road works on.
 * @pre HasRoadWorks(t)
 */
        public static void TerminateRoadWorks(this TileIndex t)
        {
            Debug.Assert(HasRoadWorks(t));
            SetRoadside(t, (Roadside) (GetRoadside(t) - Roadside.ROADSIDE_GRASS_ROAD_WORKS + Roadside.ROADSIDE_GRASS));
            /* Stop the counter */
            BitMath.SB(Map._me[t].m7, 0, 4, 0);
        }


/**
 * Get the direction of the exit of a road depot.
 * @param t The tile to query.
 * @return Diagonal direction of the depot exit.
 */
        public static DiagDirection GetRoadDepotDirection(this TileIndex t)
        {
            Debug.Assert(IsRoadDepot(t));
            return (DiagDirection) BitMath.GB(Map._m[t].m5, 0, 2);
        }

/**
 * Make a normal road tile.
 * @param t    Tile to make a normal road.
 * @param bits Road bits to set for all present road types.
 * @param rot  New present road types.
 * @param town Town ID if the road is a town-owned road.
 * @param road New owner of road.
 * @param tram New owner of tram tracks.
 */
        public static void MakeRoadNormal(this TileIndex t, RoadBits bits, RoadTypes rot, TownID town, Owner road,
            Owner tram)
        {
            TileMap.SetTileType(t, TileType.MP_ROAD);
            TileMap.SetTileOwner(t, road);
            Map._m[t].m2 = town;
            Map._m[t].m3 = (BitMath.HasBit(rot, RoadType.ROADTYPE_TRAM) ? bits : 0);
            Map._m[t].m4 = 0;
            Map._m[t].m5 = (BitMath.HasBit(rot, RoadType.ROADTYPE_ROAD) ? bits : 0) |
                           RoadTileType.ROAD_TILE_NORMAL << 6;
            BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = rot << 6;
            SetRoadOwner(t, RoadType.ROADTYPE_TRAM, tram);
        }

/**
 * Make a level crossing.
 * @param t       Tile to make a level crossing.
 * @param road    New owner of road.
 * @param tram    New owner of tram tracks.
 * @param rail    New owner of the rail track.
 * @param roaddir Axis of the road.
 * @param rat     New rail type.
 * @param rot     New present road types.
 * @param town    Town ID if the road is a town-owned road.
 */
        public static void MakeRoadCrossing(this TileIndex t, Owner road, Owner tram, Owner rail, Axis roaddir,
            RailType rat, RoadTypes rot, uint town)
        {
            TileMap.SetTileType(t, TileType.MP_ROAD);
            TileMap.SetTileOwner(t, rail);
            Map._m[t].m2 = town;
            Map._m[t].m3 = rat;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = RoadTileType.ROAD_TILE_CROSSING << 6 | roaddir;
            BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = rot << 6 | road;
            SetRoadOwner(t, RoadType.ROADTYPE_TRAM, tram);
        }

/**
 * Make a road depot.
 * @param t     Tile to make a level crossing.
 * @param owner New owner of the depot.
 * @param did   New depot ID.
 * @param dir   Direction of the depot exit.
 * @param rt    Road type of the depot.
 */
        public static void MakeRoadDepot(this TileIndex t, Owner owner, DepotID did, DiagDirection dir, RoadType rt)
        {
            TileMap.SetTileType(t, TileType.MP_ROAD);
            TileMap.SetTileOwner(t, owner);
            Map._m[t].m2 = did;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = RoadTileType.ROAD_TILE_DEPOT << 6 | dir;
            BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = RoadTypeToRoadTypes(rt) << 6 | owner;
            SetRoadOwner(t, RoadType.ROADTYPE_TRAM, owner);
        }



        /**
         * Returns the RoadBits on an arbitrary tile
         * Special behaviour:
         * - road depots: entrance is treated as road piece
         * - road tunnels: entrance is treated as road piece
         * - bridge ramps: start of the ramp is treated as road piece
         * - bridge middle parts: bridge itself is ignored
         *
         * If straight_tunnel_bridge_entrance is set a ROAD_X or ROAD_Y
         * for bridge ramps and tunnel entrances is returned depending
         * on the orientation of the tunnel or bridge.
         * @param tile the tile to get the road bits for
         * @param rt   the road type to get the road bits form
         * @param straight_tunnel_bridge_entrance whether to return straight road bits for tunnels/bridges.
         * @return the road bits of the given tile
         */
        public static RoadBits GetAnyRoadBits(this TileIndex tile, RoadType rt, bool straight_tunnel_bridge_entrance)
        {
            if (!HasTileRoadType(tile, rt)) return RoadBits.ROAD_NONE;

            switch (TileMap.GetTileType(tile))
            {
                case TileType.MP_ROAD:
                    switch (GetRoadTileType(tile))
                    {
                        default:
                        case RoadTileType.ROAD_TILE_NORMAL: return GetRoadBits(tile, rt);
                        case RoadTileType.ROAD_TILE_CROSSING: return GetCrossingRoadBits(tile);
                        case RoadTileType.ROAD_TILE_DEPOT: return DiagDirToRoadBits(GetRoadDepotDirection(tile));
                    }

                case TileType.MP_STATION:
                    if (!IsRoadStopTile(tile)) return RoadBits.ROAD_NONE;
                    if (IsDriveThroughStopTile(tile)) return (GetRoadStopDir(tile) == DiagDirection.DIAGDIR_NE) ? RoadBits.ROAD_X : RoadBits.ROAD_Y;
                    return DiagDirToRoadBits(GetRoadStopDir(tile));

                case TileType.MP_TUNNELBRIDGE:
                    if (GetTunnelBridgeTransportType(tile) != TRANSPORT_ROAD) return RoadBits.ROAD_NONE;
                    return straight_tunnel_bridge_entrance ?
                        AxisToRoadBits(DiagDirToAxis(GetTunnelBridgeDirection(tile))) :
                        DiagDirToRoadBits(ReverseDiagDir(GetTunnelBridgeDirection(tile)));

                default: return RoadBits.ROAD_NONE;
            }
        }
    }
}
