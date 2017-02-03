/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file station_type.h Types related to stations. */

using System;
using System.Collections.Generic;
using Nopenttd.Core;

namespace Nopenttd
{

    public struct StationID
    {
        public ushort Id { get; set; }

        public StationID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(StationID id)
        {
            return id.Id;
        }

        public static implicit operator StationID(ushort id)
        {
            return new StationID(id);
        }
    }

    public struct RoadStopID
    {
        public ushort Id { get; set; }

        public RoadStopID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(RoadStopID id)
        {
            return id.Id;
        }

        public static implicit operator RoadStopID(ushort id)
        {
            return new RoadStopID(id);
        }
    }

    public class StationConstants
    {
        public static readonly StationID NEW_STATION = 0xFFFE;
        public static readonly StationID INVALID_STATION = 0xFFFF;

        /// The maximum length of a station name in characters including '\0'
        public const uint MAX_LENGTH_STATION_NAME_CHARS = 32;
    }

    //typedef SmallStack<StationID, StationID, INVALID_STATION, 8, 0xFFFD> StationIDStack;

    /** Station types */

    public enum StationType
    {
        STATION_RAIL,
        STATION_AIRPORT,
        STATION_TRUCK,
        STATION_BUS,
        STATION_OILRIG,
        STATION_DOCK,
        STATION_BUOY,
        STATION_WAYPOINT,
    }

/** Types of RoadStops */

    public enum RoadStopType
    {
        /// A standard stop for buses
        ROADSTOP_BUS,

        /// A standard stop for trucks
        ROADSTOP_TRUCK,
    }

/** The facilities a station might be having */

    [Flags]
    public enum StationFacility
    {
        /// The station has no facilities at all
        FACIL_NONE = 0,

        /// Station with train station 
        FACIL_TRAIN = 1 << 0,

        /// Station with truck stops 
        FACIL_TRUCK_STOP = 1 << 1,

        /// Station with bus stops 
        FACIL_BUS_STOP = 1 << 2,

        /// Station with an airport 
        FACIL_AIRPORT = 1 << 3,

        /// Station with a dock 
        FACIL_DOCK = 1 << 4,

        /// Station is a waypoint 
        FACIL_WAYPOINT = 1 << 7,
    }

    //typedef SimpleTinyEnumT<StationFacility, byte> StationFacilityByte;

/** The vehicles that may have visited a station */

    [Flags]
    public enum StationHadVehicleOfType
    {
        /// Station has seen no vehicles
        HVOT_NONE = 0,

        /// Station has seen a train
        HVOT_TRAIN = 1 << 1,

        /// Station has seen a bus
        HVOT_BUS = 1 << 2,

        /// Station has seen a truck
        HVOT_TRUCK = 1 << 3,

        /// Station has seen an aircraft
        HVOT_AIRCRAFT = 1 << 4,

        /// Station has seen a ship
        HVOT_SHIP = 1 << 5,

        /// Station is a waypoint (NewGRF only!)
        HVOT_WAYPOINT = 1 << 6,
    }

//typedef SimpleTinyEnumT<StationHadVehicleOfType, byte> StationHadVehicleOfTypeByte;

/** The different catchment areas used */

    public enum CatchmentArea
    {
        /// Catchment when the station has no facilities
        CA_NONE = 0,

        /// Catchment for bus stops with "modified catchment" enabled 
        CA_BUS = 3,

        /// Catchment for truck stops with "modified catchment" enabled 
        CA_TRUCK = 3,

        /// Catchment for train stations with "modified catchment" enabled 
        CA_TRAIN = 4,

        /// Catchment for docks with "modified catchment" enabled 
        CA_DOCK = 5,

        /// Catchment for all stations with "modified catchment" disabled
        CA_UNMODIFIED = 4,

        /// Maximum catchment for airports with "modified catchment" enabled
        MAX_CATCHMENT = 10,
    }



/** List of station IDs */

    public class StationIDList : List<StationID>
    {
    }

/** List of stations */
//public class StationList : SmallVector<Station>
//typedef SmallVector<Station *, 2> StationList;

/**
 * Structure contains cached list of stations nearby. The list
 * is created upon first call to GetStations()
 */

    //public class StationFinder //: TileArea
    //{
    //    /// List of stations nearby
    //    private StationList stations;

    //    private TileArea tileArea;

    //    /**
    //     * Constructs StationFinder
    //     * @param area the area to search from
    //     */
    //    //public StationFinder(ref TileArea area) : TileArea(area) {}
    //    //public const StationList *GetStations();
    //}
}