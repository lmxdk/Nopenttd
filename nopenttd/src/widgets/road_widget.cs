    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file road_widget.h Types related to the road widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #BuildRoadToolbarWindow class. */
    public enum RoadToolbarWidgets
    {
        /* Name starts with RO instead of R, because of collision with RailToolbarWidgets */ // Build road in x-direction.
        WID_ROT_ROAD_X, // Build road in y-direction.
        WID_ROT_ROAD_Y, // Autorail.
        WID_ROT_AUTOROAD, // Demolish.
        WID_ROT_DEMOLISH, // Build depot.
        WID_ROT_DEPOT, // Build bus station.
        WID_ROT_BUS_STATION, // Build truck station.
        WID_ROT_TRUCK_STATION, // Build one-way road.
        WID_ROT_ONE_WAY, // Build bridge.
        WID_ROT_BUILD_BRIDGE, // Build tunnel.
        WID_ROT_BUILD_TUNNEL, // Remove road.
        WID_ROT_REMOVE,
    }

/** Widgets of the #BuildRoadDepotWindow class. */
    public enum BuildRoadDepotWidgets
    {
        /* Name starts with BRO instead of BR, because of collision with BuildRailDepotWidgets */ // Caption of the window.
        WID_BROD_CAPTION, // Depot with NE entry.
        WID_BROD_DEPOT_NE, // Depot with SE entry.
        WID_BROD_DEPOT_SE, // Depot with SW entry.
        WID_BROD_DEPOT_SW, // Depot with NW entry.
        WID_BROD_DEPOT_NW,
    }

/** Widgets of the #BuildRoadStationWindow class. */
    public enum BuildRoadStationWidgets
    {
        /* Name starts with BRO instead of BR, because of collision with BuildRailStationWidgets */ // Caption of the window.
        WID_BROS_CAPTION, // Background of the window.
        WID_BROS_BACKGROUND, // Terminal station with NE entry.
        WID_BROS_STATION_NE, // Terminal station with SE entry.
        WID_BROS_STATION_SE, // Terminal station with SW entry.
        WID_BROS_STATION_SW, // Terminal station with NW entry.
        WID_BROS_STATION_NW, // Drive-through station in x-direction.
        WID_BROS_STATION_X, // Drive-through station in y-direction.
        WID_BROS_STATION_Y, // Turn off area highlight.
        WID_BROS_LT_OFF, // Turn on area highlight.
        WID_BROS_LT_ON, // Station acceptance info.
        WID_BROS_INFO,
    }

}