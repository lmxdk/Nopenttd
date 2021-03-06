/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file rail_map.h Hides the direct accesses to the map array with map accessors */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.Tiles;

namespace Nopenttd
{


/** Different types of Rail-related tiles */
    public enum RailTileType
    {
        /// Normal rail tile without signals
        RAIL_TILE_NORMAL = 0,

        /// Normal rail tile with signals
        RAIL_TILE_SIGNALS = 1,

        /// Depot (one entrance)
        RAIL_TILE_DEPOT = 3,
    };

    public static class RailTileTypeExtensions
    {
/**
 * Returns the RailTileType (normal with or without signals,
 * waypoint or depot).
 * @param t the tile to get the information from
 * @pre IsTileType(t, MP_RAILWAY)
 * @return the RailTileType
 */
        public static RailTileType GetRailTileType(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_RAILWAY));
            return (RailTileType) BitMath.GB(Map._m[t].m5, 6, 2);
        }

/**
 * Returns whether this is plain rails, with or without signals. Iow, if this
 * tiles RailTileType is RAIL_TILE_NORMAL or RAIL_TILE_SIGNALS.
 * @param t the tile to get the information from
 * @pre IsTileType(t, MP_RAILWAY)
 * @return true if and only if the tile is normal rail (with or without signals)
 */
        public static bool IsPlainRail(this TileIndex t)
        {
            RailTileType rtt = GetRailTileType(t);
            return rtt == RailTileType.RAIL_TILE_NORMAL || rtt == RailTileType.RAIL_TILE_SIGNALS;
        }

/**
 * Checks whether the tile is a rail tile or rail tile with signals.
 * @param t the tile to get the information from
 * @return true if and only if the tile is normal rail (with or without signals)
 */
        public static bool IsPlainRailTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_RAILWAY) && IsPlainRail(t);
        }


/**
 * Checks if a rail tile has signals.
 * @param t the tile to get the information from
 * @pre IsTileType(t, MP_RAILWAY)
 * @return true if and only if the tile has signals
 */
        public static bool HasSignals(this TileIndex t)
        {
            return GetRailTileType(t) == RailTileType.RAIL_TILE_SIGNALS;
        }

/**
 * Add/remove the 'has signal' bit from the RailTileType
 * @param tile the tile to add/remove the signals to/from
 * @param signals whether the rail tile should have signals or not
 * @pre IsPlainRailTile(tile)
 */
        public static void SetHasSignals(this TileIndex tile, bool signals)
        {
            Debug.Assert(IsPlainRailTile(tile));
            Map._m[tile].m5 = BitMath.SB(Map._m[tile].m5, 6, 1, signals);
        }

/**
 * Is this rail tile a rail depot?
 * @param t the tile to get the information from
 * @pre IsTileType(t, MP_RAILWAY)
 * @return true if and only if the tile is a rail depot
 */
        public static bool IsRailDepot(this TileIndex t)
        {
            return GetRailTileType(t) == RailTileType.RAIL_TILE_DEPOT;
        }

/**
 * Is this tile rail tile and a rail depot?
 * @param t the tile to get the information from
 * @return true if and only if the tile is a rail depot
 */
        public static bool IsRailDepotTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_RAILWAY) && IsRailDepot(t);
        }

/**
 * Gets the rail type of the given tile
 * @param t the tile to get the rail type from
 * @return the rail type of the tile
 */
        public static RailType GetRailType(this TileIndex t)
        {
            return (RailType) BitMath.GB(Map._m[t].m3, 0, 4);
        }

/**
 * Sets the rail type of the given tile
 * @param t the tile to set the rail type of
 * @param r the new rail type for the tile
 */
        public static void SetRailType(this TileIndex t, RailType r)
        {
            BitMath.SB(Map._m[t].m3, 0, 4, r);
        }


/**
 * Gets the track bits of the given tile
 * @param tile the tile to get the track bits from
 * @return the track bits of the tile
 */
        public static TrackBits GetTrackBits(TileIndex tile)
        {
            Debug.Assert(IsPlainRailTile(tile));
            return (TrackBits) BitMath.GB(Map._m[tile].m5, 0, 6);
        }

/**
 * Sets the track bits of the given tile
 * @param t the tile to set the track bits of
 * @param b the new track bits for the tile
 */
        public static void SetTrackBits(this TileIndex t, TrackBits b)
        {
            Debug.Assert(IsPlainRailTile(t));
            BitMath.SB(Map._m[t].m5, 0, 6, b);
        }

/**
 * Returns whether the given track is present on the given tile.
 * @param tile  the tile to check the track presence of
 * @param track the track to search for on the tile
 * @pre IsPlainRailTile(tile)
 * @return true if and only if the given track exists on the tile
 */
        public static bool HasTrack(TileIndex tile, Track track)
        {
            return BitMath.HasBit(GetTrackBits(tile), track);
        }

/**
 * Returns the direction the depot is facing to
 * @param t the tile to get the depot facing from
 * @pre IsRailDepotTile(t)
 * @return the direction the depot is facing
 */
        public static DiagDirection GetRailDepotDirection(this TileIndex t)
        {
            return (DiagDirection) BitMath.GB(Map._m[t].m5, 0, 2);
        }

/**
 * Returns the track of a depot, ignoring direction
 * @pre IsRailDepotTile(t)
 * @param t the tile to get the depot track from
 * @return the track of the depot
 */
        public static Track GetRailDepotTrack(this TileIndex t)
        {
            return DiagDirToDiagTrack(GetRailDepotDirection(t));
        }


/**
 * Returns the reserved track bits of the tile
 * @pre IsPlainRailTile(t)
 * @param t the tile to query
 * @return the track bits
 */
        public static TrackBits GetRailReservationTrackBits(this TileIndex t)
        {
            Debug.Assert(IsPlainRailTile(t));
            byte track_b = BitMath.GB(Map._m[t].m2, 8, 3);
            Track track = (Track) (track_b - 1); // map array saves Track+1
            if (track_b == 0) return TrackBits.TRACK_BIT_NONE;
            return (TrackBits) (TrackToTrackBits(track) | (BitMath.HasBit(Map._m[t].m2, 11)
                                    ? TrackToTrackBits(TrackToOppositeTrack(track))
                                    : 0));
        }

/**
 * Sets the reserved track bits of the tile
 * @pre IsPlainRailTile(t) && !TracksOverlap(b)
 * @param t the tile to change
 * @param b the track bits
 */
        public static void SetTrackReservation(this TileIndex t, TrackBits b)
        {
            Debug.Assert(IsPlainRailTile(t));
            Debug.Assert(b != TrackBits.INVALID_TRACK_BIT);
            Debug.Assert(!TracksOverlap(b));
            Track track = RemoveFirstTrack(&b);
            BitMath.SB(Map._m[t].m2, 8, 3, track == Track.INVALID_TRACK ? 0 : track + 1);
            BitMath.SB(Map._m[t].m2, 11, 1, (byte) (b != TrackBits.TRACK_BIT_NONE));
        }

/**
 * Try to reserve a specific track on a tile
 * @pre IsPlainRailTile(t) && HasTrack(tile, t)
 * @param tile the tile
 * @param t the rack to reserve
 * @return true if successful
 */
        public static bool TryReserveTrack(TileIndex tile, Track t)
        {
            Debug.Assert(HasTrack(tile, t));
            TrackBits bits = TrackToTrackBits(t);
            TrackBits res = GetRailReservationTrackBits(tile);
            if ((res & bits) != TrackBits.TRACK_BIT_NONE) return false; // already reserved
            res |= bits;
            if (TracksOverlap(res)) return false; // crossing reservation present
            SetTrackReservation(tile, res);
            return true;
        }

/**
 * Lift the reservation of a specific track on a tile
 * @pre IsPlainRailTile(t) && HasTrack(tile, t)
 * @param tile the tile
 * @param t the track to free
 */
        public static void UnreserveTrack(TileIndex tile, Track t)
        {
            Debug.Assert(HasTrack(tile, t));
            TrackBits res = GetRailReservationTrackBits(tile);
            res &= ~TrackToTrackBits(t);
            SetTrackReservation(tile, res);
        }

/**
 * Get the reservation state of the depot
 * @pre IsRailDepot(t)
 * @param t the depot tile
 * @return reservation state
 */
        public static bool HasDepotReservation(this TileIndex t)
        {
            Debug.Assert(IsRailDepot(t));
            return BitMath.HasBit(Map._m[t].m5, 4);
        }

/**
 * Set the reservation state of the depot
 * @pre IsRailDepot(t)
 * @param t the depot tile
 * @param b the reservation state
 */
        public static void SetDepotReservation(this TileIndex t, bool b)
        {
            Debug.Assert(IsRailDepot(t));
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 4, 1, b ? 1 : 0);
        }

/**
 * Get the reserved track bits for a depot
 * @pre IsRailDepot(t)
 * @param t the tile
 * @return reserved track bits
 */
        public static TrackBits GetDepotReservationTrackBits(this TileIndex t)
        {
            return HasDepotReservation(t) ? TrackToTrackBits(GetRailDepotTrack(t)) : TrackBits.TRACK_BIT_NONE;
        }


        public static bool IsPbsSignal(SignalType s)
        {
            return s == SignalType.SIGTYPE_PBS || s == SignalType.SIGTYPE_PBS_ONEWAY;
        }

        public static SignalType GetSignalType(this TileIndex t, Track track)
        {
            Debug.Assert(GetRailTileType(t) == RailTileType.RAIL_TILE_SIGNALS);
            byte pos = (byte) ((track == Track.TRACK_LOWER || track == Track.TRACK_RIGHT) ? 4 : 0);
            return (SignalType) BitMath.GB(Map._m[t].m2, pos, 3);
        }

        public static void SetSignalType(this TileIndex t, Track track, SignalType s)
        {
            Debug.Assert(GetRailTileType(t) == RailTileType.RAIL_TILE_SIGNALS);
            byte pos = (byte) ((track == Track.TRACK_LOWER || track == Track.TRACK_RIGHT) ? 4 : 0);
            Map._m[t].m2 = BitMath.SB(Map._m[t].m2, pos, 3, s);
            if (track == Track.INVALID_TRACK) BitMath.SB(Map._m[t].m2, 4, 3, s);
        }

        public static bool IsPresignalEntry(this TileIndex t, Track track)
        {
            var type = GetSignalType(t, track);
            return type == SignalType.SIGTYPE_ENTRY || type == SignalType.SIGTYPE_COMBO;
        }

        public static bool IsPresignalExit(this TileIndex t, Track track)
        {
            var type = GetSignalType(t, track);
            return type == SignalType.SIGTYPE_EXIT || type == SignalType.SIGTYPE_COMBO;
        }

/** One-way signals can't be passed the 'wrong' way. */
        public static bool IsOnewaySignal(this TileIndex t, Track track)
        {
            return GetSignalType(t, track) != SignalType.SIGTYPE_PBS;
        }

        public static void CycleSignalSide(this TileIndex t, Track track)
        {
            byte sig;
            byte pos = (track == Track.TRACK_LOWER || track == Track.TRACK_RIGHT) ? 4 : 6;

            sig = BitMath.GB(Map._m[t].m3, pos, 2);
            if (--sig == 0) sig = IsPbsSignal(GetSignalType(t, track)) ? 2 : 3;
            BitMath.SB(Map._m[t].m3, pos, 2, sig);
        }

        public static SignalVariant GetSignalVariant(this TileIndex t, Track track)
        {
            byte pos = (byte) ((track == Track.TRACK_LOWER || track == Track.TRACK_RIGHT) ? 7 : 3);
            return (SignalVariant) BitMath.GB(Map._m[t].m2, pos, 1);
        }

        public static void SetSignalVariant(this TileIndex t, Track track, SignalVariant v)
        {
            byte pos = (byte) ((track == Track.TRACK_LOWER || track == Track.TRACK_RIGHT) ? 7 : 3);
            BitMath.SB(Map._m[t].m2, pos, 1, v);
            if (track == Track.INVALID_TRACK) BitMath.SB(Map._m[t].m2, 7, 1, v);
        }

/**
 * Set the states of the signals (Along/AgainstTrackDir)
 * @param tile  the tile to set the states for
 * @param state the new state
 */
        public static void SetSignalStates(TileIndex tile, uint state)
        {
            BitMath.SB(Map._m[tile].m4, 4, 4, state);
        }

/**
 * Set the states of the signals (Along/AgainstTrackDir)
 * @param tile  the tile to set the states for
 * @return the state of the signals
 */
        public static uint GetSignalStates(TileIndex tile)
        {
            return BitMath.GB(Map._m[tile].m4, 4, 4);
        }

/**
 * Get the state of a single signal
 * @param t         the tile to get the signal state for
 * @param signalbit the signal
 * @return the state of the signal
 */
        public static SignalState GetSingleSignalState(this TileIndex t, byte signalbit)
        {
            return (SignalState) BitMath.HasBit(GetSignalStates(t), signalbit);
        }

/**
 * Set whether the given signals are present (Along/AgainstTrackDir)
 * @param tile    the tile to set the present signals for
 * @param signals the signals that have to be present
 */
        public static void SetPresentSignals(TileIndex tile, uint signals)
        {
            BitMath.SB(Map._m[tile].m3, 4, 4, signals);
        }

/**
 * Get whether the given signals are present (Along/AgainstTrackDir)
 * @param tile the tile to get the present signals for
 * @return the signals that are present
 */
        public static uint GetPresentSignals(TileIndex tile)
        {
            return BitMath.GB(Map._m[tile].m3, 4, 4);
        }

/**
 * Checks whether the given signals is present
 * @param t         the tile to check on
 * @param signalbit the signal
 * @return true if and only if the signal is present
 */
        public static bool IsSignalPresent(this TileIndex t, byte signalbit)
        {
            return BitMath.HasBit(GetPresentSignals(t), signalbit);
        }

/**
 * Checks for the presence of signals (either way) on the given track on the
 * given rail tile.
 */
        public static bool HasSignalOnTrack(TileIndex tile, Track track)
        {
            Debug.Assert(IsValidTrack(track));
            return GetRailTileType(tile) == RailTileType.RAIL_TILE_SIGNALS &&
                   (GetPresentSignals(tile) & SignalOnTrack(track)) != 0;
        }

/**
 * Checks for the presence of signals along the given trackdir on the given
 * rail tile.
 *
 * Along meaning if you are currently driving on the given trackdir, this is
 * the signal that is facing us (for which we stop when it's red).
 */
        public static bool HasSignalOnTrackdir(TileIndex tile, Trackdir trackdir)
        {
            Debug.Assert(IsValidTrackdir(trackdir));
            return GetRailTileType(tile) == RailTileType.RAIL_TILE_SIGNALS &&
                   GetPresentSignals(tile) & SignalAlongTrackdir(trackdir);
        }

/**
 * Gets the state of the signal along the given trackdir.
 *
 * Along meaning if you are currently driving on the given trackdir, this is
 * the signal that is facing us (for which we stop when it's red).
 */
        public static SignalState GetSignalStateByTrackdir(TileIndex tile, Trackdir trackdir)
        {
            Debug.Assert(IsValidTrackdir(trackdir));
            Debug.Assert(HasSignalOnTrack(tile, TrackdirToTrack(trackdir)));
            return GetSignalStates(tile) & SignalAlongTrackdir(trackdir)
                ? SignalState.SIGNAL_STATE_GREEN
                : SignalState.SIGNAL_STATE_RED;
        }

/**
 * Sets the state of the signal along the given trackdir.
 */
        public static void SetSignalStateByTrackdir(TileIndex tile, Trackdir trackdir, SignalState state)
        {
            if (state == SignalState.SIGNAL_STATE_GREEN)
            {
                // set 1
                SetSignalStates(tile, GetSignalStates(tile) | SignalAlongTrackdir(trackdir));
            }
            else
            {
                SetSignalStates(tile, GetSignalStates(tile) & ~SignalAlongTrackdir(trackdir));
            }
        }

/**
 * Is a pbs signal present along the trackdir?
 * @param tile the tile to check
 * @param td the trackdir to check
 */
        public static bool HasPbsSignalOnTrackdir(TileIndex tile, Trackdir td)
        {
            return TileMap.IsTileType(tile, TileType.MP_RAILWAY) && HasSignalOnTrackdir(tile, td) &&
                   IsPbsSignal(GetSignalType(tile, TrackdirToTrack(td)));
        }

/**
 * Is a one-way signal blocking the trackdir? A one-way signal on the
 * trackdir against will block, but signals on both trackdirs won't.
 * @param tile the tile to check
 * @param td the trackdir to check
 */
        public static bool HasOnewaySignalBlockingTrackdir(TileIndex tile, Trackdir td)
        {
            return TileMap.IsTileType(tile, TileType.MP_RAILWAY) && HasSignalOnTrackdir(tile, ReverseTrackdir(td)) &&
                   !HasSignalOnTrackdir(tile, td) && IsOnewaySignal(tile, TrackdirToTrack(td));
        }


/** The ground 'under' the rail */
        public enum RailGroundType
        {
            /// Nothing (dirt)
            RAIL_GROUND_BARREN = 0,

            /// Grassy
            RAIL_GROUND_GRASS = 1,

            /// Grass with a fence at the NW edge
            RAIL_GROUND_FENCE_NW = 2,

            /// Grass with a fence at the SE edge
            RAIL_GROUND_FENCE_SE = 3,

            /// Grass with a fence at the NW and SE edges
            RAIL_GROUND_FENCE_SENW = 4,

            /// Grass with a fence at the NE edge
            RAIL_GROUND_FENCE_NE = 5,

            /// Grass with a fence at the SW edge
            RAIL_GROUND_FENCE_SW = 6,

            /// Grass with a fence at the NE and SW edges
            RAIL_GROUND_FENCE_NESW = 7,

            /// Grass with a fence at the eastern side
            RAIL_GROUND_FENCE_VERT1 = 8,

            /// Grass with a fence at the western side
            RAIL_GROUND_FENCE_VERT2 = 9,

            /// Grass with a fence at the southern side
            RAIL_GROUND_FENCE_HORIZ1 = 10,

            /// Grass with a fence at the northern side
            RAIL_GROUND_FENCE_HORIZ2 = 11,

            /// Icy or sandy
            RAIL_GROUND_ICE_DESERT = 12,

            /// Grass with a fence and shore or water on the free halftile
            RAIL_GROUND_WATER = 13,

            /// Snow only on higher part of slope (steep or one corner raised)
            RAIL_GROUND_HALF_SNOW = 14,
        };

        public static void SetRailGroundType(this TileIndex t, RailGroundType rgt)
        {
            Map._m[t].m4 = BitMath.SB(Map._m[t].m4, 0, 4, rgt);
        }

        public static RailGroundType GetRailGroundType(this TileIndex t)
        {
            return (RailGroundType) BitMath.GB(Map._m[t].m4, 0, 4);
        }

        public static bool IsSnowRailGround(this TileIndex t)
        {
            return GetRailGroundType(t) == RailGroundType.RAIL_GROUND_ICE_DESERT;
        }


        public static void MakeRailNormal(this TileIndex t, Owner o, TrackBits b, RailType r)
        {
            TileMap.SetTileType(t, TileType.MP_RAILWAY);
            TileMap.SetTileOwner(t, o);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = (byte) r;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = (byte)((int)RailTileType.RAIL_TILE_NORMAL << 6 | b);
            BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }


        public static void MakeRailDepot(this TileIndex t, Owner o, DepotID did, DiagDirection d, RailType r)
        {
            TileMap.SetTileType(t, TileType.MP_RAILWAY);
            TileMap.SetTileOwner(t, o);
            Map._m[t].m2 = did;
            Map._m[t].m3 = (byte) r;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = (byte)((int)RailTileType.RAIL_TILE_DEPOT << 6 | (int)d);
            BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }
    }
}
		
