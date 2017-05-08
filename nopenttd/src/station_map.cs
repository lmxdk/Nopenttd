/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file station_map.h Maps accessors for stations. */

using System.Diagnostics;
using Nopenttd;
using Nopenttd.Core;
using Nopenttd.Tiles;

namespace Nopenttd
{

    /// Index of station graphics. @see _station_display_datas
    public struct StationGfx
    {
        public byte Id { get; set; }

        public StationGfx(byte id)
        {
            Id = id;
        }

        public static implicit operator byte(StationGfx id)
        {
            return id.Id;
        }

        public static implicit operator StationGfx(byte id)
        {
            return new StationGfx(id);
        }
    }


    public static class StationMap
    {
/**
 * Get StationID from a tile
 * @param t Tile to query station ID from
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return Station ID of the station at \a t
 */
        public static StationID GetStationIndex(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));
            return (StationID) Map._m[t].m2;
        }

        /// The offset for the water parts.
        public const int GFX_DOCK_BASE_WATER_PART = 4;

        /// The offset for the drive through parts.
        public const int GFX_TRUCK_BUS_DRIVETHROUGH_OFFSET = 4;

/**
 * Get the station type of this tile
 * @param t the tile to query
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return the station type
 */
        public static StationType GetStationType(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));
            return (StationType) BitMath.GB(Map._me[t].m6, 3, 3);
        }

/**
 * Get the road stop type of this tile
 * @param t the tile to query
 * @pre GetStationType(t) == STATION_TRUCK || GetStationType(t) == STATION_BUS
 * @return the road stop type
 */
        public static RoadStopType GetRoadStopType(this TileIndex t)
        {
            Debug.Assert(GetStationType(t) == StationType.STATION_TRUCK ||
                         GetStationType(t) == StationType.STATION_BUS);
            return GetStationType(t) == StationType.STATION_TRUCK
                ? RoadStopType.ROADSTOP_TRUCK
                : RoadStopType.ROADSTOP_BUS;
        }

/**
 * Get the station graphics of this tile
 * @param t the tile to query
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return the station graphics
 */
        public static StationGfx GetStationGfx(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));
            return Map._m[t].m5;
        }

/**
 * Set the station graphics of this tile
 * @param t the tile to update
 * @param gfx the new graphics
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 */
        public static void SetStationGfx(this TileIndex t, StationGfx gfx)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));
            Map._m[t].m5 = gfx;
        }

/**
 * Is this station tile a rail station?
 * @param t the tile to get the information from
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return true if and only if the tile is a rail station
 */
        public static bool IsRailStation(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_RAIL;
        }

/**
 * Is this tile a station tile and a rail station?
 * @param t the tile to get the information from
 * @return true if and only if the tile is a rail station
 */
        public static bool IsRailStationTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && IsRailStation(t);
        }

/**
 * Is this station tile a rail waypoint?
 * @param t the tile to get the information from
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return true if and only if the tile is a rail waypoint
 */
        public static bool IsRailWaypoint(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_WAYPOINT;
        }

/**
 * Is this tile a station tile and a rail waypoint?
 * @param t the tile to get the information from
 * @return true if and only if the tile is a rail waypoint
 */
        public static bool IsRailWaypointTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && IsRailWaypoint(t);
        }

/**
 * Has this station tile a rail? In other words, is this station
 * tile a rail station or rail waypoint?
 * @param t the tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return true if and only if the tile has rail
 */
        public static bool HasStationRail(this TileIndex t)
        {
            return IsRailStation(t) || IsRailWaypoint(t);
        }

/**
 * Has this station tile a rail? In other words, is this station
 * tile a rail station or rail waypoint?
 * @param t the tile to check
 * @return true if and only if the tile is a station tile and has rail
 */
        public static bool HasStationTileRail(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && HasStationRail(t);
        }

/**
 * Is this station tile an airport?
 * @param t the tile to get the information from
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return true if and only if the tile is an airport
 */
        public static bool IsAirport(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_AIRPORT;
        }

/**
 * Is this tile a station tile and an airport tile?
 * @param t the tile to get the information from
 * @return true if and only if the tile is an airport
 */
        public static bool IsAirportTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && IsAirport(t);
        }

        bool IsHangar(this TileIndex t);

/**
 * Is the station at \a t a truck stop?
 * @param t Tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return \c true if station is a truck stop, \c false otherwise
 */
        public static bool IsTruckStop(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_TRUCK;
        }

/**
 * Is the station at \a t a bus stop?
 * @param t Tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return \c true if station is a bus stop, \c false otherwise
 */
        public static bool IsBusStop(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_BUS;
        }

/**
 * Is the station at \a t a road station?
 * @param t Tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return \c true if station at the tile is a bus top or a truck stop, \c false otherwise
 */
        public static bool IsRoadStop(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));
            return IsTruckStop(t) || IsBusStop(t);
        }

/**
 * Is tile \a t a road stop station?
 * @param t Tile to check
 * @return \c true if the tile is a station tile and a road stop
 */
        public static bool IsRoadStopTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && IsRoadStop(t);
        }

/**
 * Is tile \a t a standard (non-drive through) road stop station?
 * @param t Tile to check
 * @return \c true if the tile is a station tile and a standard road stop
 */
        public static bool IsStandardRoadStopTile(this TileIndex t)
        {
            return IsRoadStopTile(t) && GetStationGfx(t) < GFX_TRUCK_BUS_DRIVETHROUGH_OFFSET;
        }

/**
 * Is tile \a t a drive through road stop station?
 * @param t Tile to check
 * @return \c true if the tile is a station tile and a drive through road stop
 */
        public static bool IsDriveThroughStopTile(this TileIndex t)
        {
            return IsRoadStopTile(t) && GetStationGfx(t) >= GFX_TRUCK_BUS_DRIVETHROUGH_OFFSET;
        }

/**
 * Get the station graphics of this airport tile
 * @param t the tile to query
 * @pre IsAirport(t)
 * @return the station graphics
 */
        public static StationGfx GetAirportGfx(this TileIndex t)
        {
            Debug.Assert(IsAirport(t));
            return GetTranslatedAirportTileID(GetStationGfx(t));
        }

/**
 * Gets the direction the road stop entrance points towards.
 * @param t the tile of the road stop
 * @pre IsRoadStopTile(t)
 * @return the direction of the entrance
 */
        public static DiagDirection GetRoadStopDir(this TileIndex t)
        {
            StationGfx gfx = GetStationGfx(t);
            Debug.Assert(IsRoadStopTile(t));
            if (gfx < GFX_TRUCK_BUS_DRIVETHROUGH_OFFSET)
            {
                return (DiagDirection) (gfx);
            }
            else
            {
                return (DiagDirection) (gfx - GFX_TRUCK_BUS_DRIVETHROUGH_OFFSET);
            }
        }

/**
 * Is tile \a t part of an oilrig?
 * @param t Tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return \c true if the tile is an oilrig tile
 */
        public static bool IsOilRig(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_OILRIG;
        }

/**
 * Is tile \a t a dock tile?
 * @param t Tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return \c true if the tile is a dock
 */
        public static bool IsDock(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_DOCK;
        }

/**
 * Is tile \a t a dock tile?
 * @param t Tile to check
 * @return \c true if the tile is a dock
 */
        public static bool IsDockTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && GetStationType(t) == StationType.STATION_DOCK;
        }

/**
 * Is tile \a t a buoy tile?
 * @param t Tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return \c true if the tile is a buoy
 */
        public static bool IsBuoy(this TileIndex t)
        {
            return GetStationType(t) == StationType.STATION_BUOY;
        }

/**
 * Is tile \a t a buoy tile?
 * @param t Tile to check
 * @return \c true if the tile is a buoy
 */
        public static bool IsBuoyTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && IsBuoy(t);
        }

/**
 * Is tile \a t an hangar tile?
 * @param t Tile to check
 * @return \c true if the tile is an hangar
 */
        public static bool IsHangarTile(this TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_STATION) && IsHangar(t);
        }

/**
 * Get the rail direction of a rail station.
 * @param t Tile to query
 * @pre HasStationRail(t)
 * @return The direction of the rails on tile \a t.
 */
        public static Axis GetRailStationAxis(this TileIndex t)
        {
            Debug.Assert(HasStationRail(t));
            return BitMath.HasBit(GetStationGfx(t), 0) ? Axis.AXIS_Y : Axis.AXIS_X;
        }

/**
 * Get the rail track of a rail station tile.
 * @param t Tile to query
 * @pre HasStationRail(t)
 * @return The rail track of the rails on tile \a t.
 */
        public static Track GetRailStationTrack(this TileIndex t)
        {
            return AxisToTrack(GetRailStationAxis(t));
        }

/**
 * Get the trackbits of a rail station tile.
 * @param t Tile to query
 * @pre HasStationRail(t)
 * @return The trackbits of the rails on tile \a t.
 */
        public static TrackBits GetRailStationTrackBits(this TileIndex t)
        {
            return AxisToTrackBits(GetRailStationAxis(t));
        }

/**
 * Check if a tile is a valid continuation to a railstation tile.
 * The tile \a test_tile is a valid continuation to \a station_tile, if all of the following are true:
 * \li \a test_tile is a rail station tile
 * \li the railtype of \a test_tile is compatible with the railtype of \a station_tile
 * \li the tracks on \a test_tile and \a station_tile are in the same direction
 * \li both tiles belong to the same station
 * \li \a test_tile is not blocked (@see IsStationTileBlocked)
 * @param test_tile Tile to test
 * @param station_tile Station tile to compare with
 * @pre IsRailStationTile(station_tile)
 * @return true if the two tiles are compatible
 */
        public static bool IsCompatibleTrainStationTile(TileIndex test_tile, TileIndex station_tile)
        {
            Debug.Assert(IsRailStationTile(station_tile));
            return IsRailStationTile(test_tile) &&
                   test_tile.GetRailType().IsCompatibleRail(station_tile.GetRailType()) &&
                   GetRailStationAxis(test_tile) == GetRailStationAxis(station_tile) &&
                   GetStationIndex(test_tile) == GetStationIndex(station_tile) &&
                   !IsStationTileBlocked(test_tile);
        }

/**
 * Get the reservation state of the rail station
 * @pre HasStationRail(t)
 * @param t the station tile
 * @return reservation state
 */
        public static bool HasStationReservation(this TileIndex t)
        {
            Debug.Assert(HasStationRail(t));
            return BitMath.HasBit(Map._me[t].m6, 2);
        }

/**
 * Set the reservation state of the rail station
 * @pre HasStationRail(t)
 * @param t the station tile
 * @param b the reservation state
 */
        public static void SetRailStationReservation(this TileIndex t, bool b)
        {
            Debug.Assert(HasStationRail(t));
            Map._me[t].m6 = BitMath.SB(Map._me[t].m6, 2, 1, b ? 1 : 0);
        }

/**
 * Get the reserved track bits for a waypoint
 * @pre HasStationRail(t)
 * @param t the tile
 * @return reserved track bits
 */
        public static TrackBits GetStationReservationTrackBits(this TileIndex t)
        {
            return HasStationReservation(t) ? GetRailStationTrackBits(t) : TrackBits.TRACK_BIT_NONE;
        }

/**
 * Get the direction of a dock.
 * @param t Tile to query
 * @pre IsDock(t)
 * @pre \a t is the land part of the dock
 * @return The direction of the dock on tile \a t.
 */
        public static DiagDirection GetDockDirection(this TileIndex t)
        {
            StationGfx gfx = GetStationGfx(t);
            Debug.Assert(IsDock(t) && gfx < GFX_DOCK_BASE_WATER_PART);
            return (DiagDirection) (gfx);
        }

        private static readonly TileIndexDiffC buoy_offset = new TileIndexDiffC(0, 0);
        private static readonly TileIndexDiffC oilrig_offset = new TileIndexDiffC(2, 0);

        private static readonly TileIndexDiffC[] dock_offset = //[DIAGDIR_END] = 
        {
            new TileIndexDiffC(-2, 0),
            new TileIndexDiffC(0, 2),
            new TileIndexDiffC(2, 0),
            new TileIndexDiffC(0, -2),
        };

        /**
         * Get the tileoffset from this tile a ship should target to get to this dock.
         * @param t Tile to query
         * @pre TileMap.IsTileType(t, TileType.MP_STATION)
         * @pre IsBuoy(t) || IsOilRig(t) || IsDock(t)
         * @return The offset from this tile that should be used as destination for ships.
         */
        public static TileIndexDiffC GetDockOffset(this TileIndex t)
        {

            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));

            if (IsBuoy(t)) return buoy_offset;
            if (IsOilRig(t)) return oilrig_offset;

            Debug.Assert(IsDock(t));

            return dock_offset[t.GetDockDirection()];
        }

/**
 * Is there a custom rail station spec on this tile?
 * @param t Tile to query
 * @pre HasStationTileRail(t)
 * @return True if this station is part of a newgrf station.
 */
        public static bool IsCustomStationSpecIndex(this TileIndex t)
        {
            Debug.Assert(HasStationTileRail(t));
            return Map._m[t].m4 != 0;
        }

/**
 * Set the custom station spec for this tile.
 * @param t Tile to set the stationspec of.
 * @param specindex The new spec.
 * @pre HasStationTileRail(t)
 */
        public static void SetCustomStationSpecIndex(this TileIndex t, byte specindex)
        {
            Debug.Assert(HasStationTileRail(t));
            Map._m[t].m4 = specindex;
        }

/**
 * Get the custom station spec for this tile.
 * @param t Tile to query
 * @pre HasStationTileRail(t)
 * @return The custom station spec of this tile.
 */
        public static uint GetCustomStationSpecIndex(this TileIndex t)
        {
            Debug.Assert(HasStationTileRail(t));
            return Map._m[t].m4;
        }

/**
 * Set the random bits for a station tile.
 * @param t Tile to set random bits for.
 * @param random_bits The random bits.
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 */
        public static void SetStationTileRandomBits(this TileIndex t, byte random_bits)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));
            Map._m[t].m3 = BitMath.SB(Map._m[t].m3, 4, 4, random_bits);
        }

/**
 * Get the random bits of a station tile.
 * @param t Tile to query
 * @pre TileMap.IsTileType(t, TileType.MP_STATION)
 * @return The random bits for this station tile.
 */
        public static byte GetStationTileRandomBits(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_STATION));
            return BitMath.GB(Map._m[t].m3, 4, 4);
        }

/**
 * Make the given tile a station tile.
 * @param t the tile to make a station tile
 * @param o the owner of the station
 * @param sid the station to which this tile belongs
 * @param st the type this station tile
 * @param section the StationGfx to be used for this tile
 * @param wc The water class of the station
 */
        public static void MakeStation(this TileIndex t, Owner o, StationID sid, StationType st, byte section,
            WaterClass wc = WaterClass.WATER_CLASS_INVALID)
        {
            TileMap.SetTileType(t, TileType.MP_STATION);
            TileMap.SetTileOwner(t, o);
            WaterMap.SetWaterClass(t, wc);
            Map._m[t].m2 = sid;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = section;
            Map._me[t].m6 = BitMath.SB(Map._me[t].m6, 2, 1, 0);
            Map._me[t].m6 = BitMath.SB(Map._me[t].m6, 3, 3, st);
            Map._me[t].m7 = 0;
        }

/**
 * Make the given tile a rail station tile.
 * @param t the tile to make a rail station tile
 * @param o the owner of the station
 * @param sid the station to which this tile belongs
 * @param a the axis of this tile
 * @param section the StationGfx to be used for this tile
 * @param rt the railtype of this tile
 */
        public static void MakeRailStation(this TileIndex t, Owner o, StationID sid, Axis a, byte section, RailType rt)
        {
            MakeStation(t, o, sid, StationType.STATION_RAIL, section + a);
            t.SetRailType(rt);
            SetRailStationReservation(t, false);
        }

/**
 * Make the given tile a rail waypoint tile.
 * @param t the tile to make a rail waypoint
 * @param o the owner of the waypoint
 * @param sid the waypoint to which this tile belongs
 * @param a the axis of this tile
 * @param section the StationGfx to be used for this tile
 * @param rt the railtype of this tile
 */
        public static void MakeRailWaypoint(this TileIndex t, Owner o, StationID sid, Axis a, byte section, RailType rt)
        {
            MakeStation(t, o, sid, StationType.STATION_WAYPOINT, section + a);
            t.SetRailType(rt);
            SetRailStationReservation(t, false);
        }

/**
 * Make the given tile a roadstop tile.
 * @param t the tile to make a roadstop
 * @param o the owner of the roadstop
 * @param sid the station to which this tile belongs
 * @param rst the type of roadstop to make this tile
 * @param rt the roadtypes on this tile
 * @param d the direction of the roadstop
 */
        public static void MakeRoadStop(this TileIndex t, Owner o, StationID sid, RoadStopType rst, RoadTypes rt,
            DiagDirection d)
        {
            MakeStation(t, o, sid,
                (rst == RoadStopType.ROADSTOP_BUS ? StationType.STATION_BUS : StationType.STATION_TRUCK), d);
            t.SetRoadTypes(rt);
            t.SetRoadOwner(RoadType.ROADTYPE_ROAD, o);
            t.SetRoadOwner(RoadType.ROADTYPE_TRAM, o);
        }

/**
 * Make the given tile a drivethrough roadstop tile.
 * @param t the tile to make a roadstop
 * @param station the owner of the roadstop
 * @param road the owner of the road
 * @param tram the owner of the tram
 * @param sid the station to which this tile belongs
 * @param rst the type of roadstop to make this tile
 * @param rt the roadtypes on this tile
 * @param a the direction of the roadstop
 */
        public static void MakeDriveThroughRoadStop(this TileIndex t, Owner station, Owner road, Owner tram,
            StationID sid, RoadStopType rst, RoadTypes rt, Axis a)
        {
            MakeStation(t, station, sid,
                (rst == RoadStopType.ROADSTOP_BUS ? StationType.STATION_BUS : StationType.STATION_TRUCK),
                GFX_TRUCK_BUS_DRIVETHROUGH_OFFSET + a);
            t.SetRoadTypes(rt);
            t.SetRoadOwner(RoadType.ROADTYPE_ROAD, road);
            t.SetRoadOwner(RoadType.ROADTYPE_TRAM, tram);
        }

/**
 * Make the given tile an airport tile.
 * @param t the tile to make a airport
 * @param o the owner of the airport
 * @param sid the station to which this tile belongs
 * @param section the StationGfx to be used for this tile
 * @param wc the type of water on this tile
 */
        public static void MakeAirport(this TileIndex t, Owner o, StationID sid, byte section, WaterClass wc)
        {
            MakeStation(t, o, sid, StationType.STATION_AIRPORT, section, wc);
        }

/**
 * Make the given tile a buoy tile.
 * @param t the tile to make a buoy
 * @param sid the station to which this tile belongs
 * @param wc the type of water on this tile
 */
        public static void MakeBuoy(this TileIndex t, StationID sid, WaterClass wc)
        {
            /* Make the owner of the buoy tile the same as the current owner of the
             * water tile. In this way, we can reset the owner of the water to its
             * original state when the buoy gets removed. */
            MakeStation(t, TileMap.GetTileOwner(t), sid, StationType.STATION_BUOY, 0, wc);
        }

/**
 * Make the given tile a dock tile.
 * @param t the tile to make a dock
 * @param o the owner of the dock
 * @param sid the station to which this tile belongs
 * @param d the direction of the dock
 * @param wc the type of water on this tile
 */
        public static void MakeDock(this TileIndex t, Owner o, StationID sid, DiagDirection d, WaterClass wc)
        {
            MakeStation(t, o, sid, StationType.STATION_DOCK, d);
            MakeStation(t + Map.TileOffsByDiagDir(d), o, sid, StationType.STATION_DOCK,
                GFX_DOCK_BASE_WATER_PART + d.DiagDirToAxis(), wc);
        }

/**
 * Make the given tile an oilrig tile.
 * @param t the tile to make an oilrig
 * @param sid the station to which this tile belongs
 * @param wc the type of water on this tile
 */
        public static void MakeOilrig(this TileIndex t, StationID sid, WaterClass wc)
        {
            MakeStation(t, Owner.OWNER_NONE, sid, StationType.STATION_OILRIG, 0, wc);
        }

    }
}
