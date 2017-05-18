
    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file rail_widget.h Types related to the rail widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #BuildRailToolbarWindow class. */
    public enum RailToolbarWidgets
    {
        /* Name starts with RA instead of R, because of collision with RoadToolbarWidgets */ // Caption of the window.
        WID_RAT_CAPTION, // Build rail along the game view Y axis.
        WID_RAT_BUILD_NS, // Build rail along the game grid X axis.
        WID_RAT_BUILD_X, // Build rail along the game view X axis.
        WID_RAT_BUILD_EW, // Build rail along the game grid Y axis.
        WID_RAT_BUILD_Y, // Autorail tool.
        WID_RAT_AUTORAIL, // Destroy something with dynamite!
        WID_RAT_DEMOLISH, // Build a depot.
        WID_RAT_BUILD_DEPOT, // Build a waypoint.
        WID_RAT_BUILD_WAYPOINT, // Build a station.
        WID_RAT_BUILD_STATION, // Build signals.
        WID_RAT_BUILD_SIGNALS, // Build a bridge.
        WID_RAT_BUILD_BRIDGE, // Build a tunnel.
        WID_RAT_BUILD_TUNNEL, // Bulldozer to remove rail.
        WID_RAT_REMOVE, // Convert other rail to this type.
        WID_RAT_CONVERT_RAIL,
    }

/** Widgets of the #BuildRailStationWindow class. */
    public enum BuildRailStationWidgets
    {
        /* Name starts with BRA instead of BR, because of collision with BuildRoadStationWidgets */ // Button to select '/' view.
        WID_BRAS_PLATFORM_DIR_X, // Button to select '\' view.
        WID_BRAS_PLATFORM_DIR_Y,

// Button to select stations with a single platform.
        WID_BRAS_PLATFORM_NUM_1, // Button to select stations with 2 platforms.
        WID_BRAS_PLATFORM_NUM_2, // Button to select stations with 3 platforms.
        WID_BRAS_PLATFORM_NUM_3, // Button to select stations with 4 platforms.
        WID_BRAS_PLATFORM_NUM_4, // Button to select stations with 5 platforms.
        WID_BRAS_PLATFORM_NUM_5, // Button to select stations with 6 platforms.
        WID_BRAS_PLATFORM_NUM_6, // Button to select stations with 7 platforms.
        WID_BRAS_PLATFORM_NUM_7,

// Button to select single tile length station platforms.
        WID_BRAS_PLATFORM_LEN_1, // Button to select 2 tiles length station platforms.
        WID_BRAS_PLATFORM_LEN_2, // Button to select 3 tiles length station platforms.
        WID_BRAS_PLATFORM_LEN_3, // Button to select 4 tiles length station platforms.
        WID_BRAS_PLATFORM_LEN_4, // Button to select 5 tiles length station platforms.
        WID_BRAS_PLATFORM_LEN_5, // Button to select 6 tiles length station platforms.
        WID_BRAS_PLATFORM_LEN_6, // Button to select 7 tiles length station platforms.
        WID_BRAS_PLATFORM_LEN_7,

// Button to enable drag and drop type station placement.
        WID_BRAS_PLATFORM_DRAG_N_DROP,

// Button for turning coverage highlighting off.
        WID_BRAS_HIGHLIGHT_OFF, // Button for turning coverage highlighting on.
        WID_BRAS_HIGHLIGHT_ON, // Empty space for the coverage texts.
        WID_BRAS_COVERAGE_TEXTS,

// Matrix widget displaying the available stations.
        WID_BRAS_MATRIX, // Panel used at each cell of the matrix.
        WID_BRAS_IMAGE, // Scrollbar of the matrix widget.
        WID_BRAS_MATRIX_SCROLL,

// Selection for default-size button for newstation.
        WID_BRAS_SHOW_NEWST_DEFSIZE, // Selection for newstation class selection list.
        WID_BRAS_SHOW_NEWST_ADDITIONS, // Selection for newstation image matrix.
        WID_BRAS_SHOW_NEWST_MATRIX, // Selection for panel and resize at bottom right for newstation.
        WID_BRAS_SHOW_NEWST_RESIZE, // Display of selected station type.
        WID_BRAS_SHOW_NEWST_TYPE, // List with available newstation classes.
        WID_BRAS_NEWST_LIST, // Scrollbar of the #WID_BRAS_NEWST_LIST.
        WID_BRAS_NEWST_SCROLL,

// Helper for determining the chosen platform width.
        WID_BRAS_PLATFORM_NUM_BEGIN = WID_BRAS_PLATFORM_NUM_1 - 1, // Helper for determining the chosen platform length.
        WID_BRAS_PLATFORM_LEN_BEGIN = WID_BRAS_PLATFORM_LEN_1 - 1,
    }

/** Widgets of the #BuildSignalWindow class. */
    public enum BuildSignalWidgets
    {
// Build a semaphore normal block signal
        WID_BS_SEMAPHORE_NORM, // Build a semaphore entry block signal
        WID_BS_SEMAPHORE_ENTRY, // Build a semaphore exit block signal
        WID_BS_SEMAPHORE_EXIT, // Build a semaphore combo block signal
        WID_BS_SEMAPHORE_COMBO, // Build a semaphore path signal.
        WID_BS_SEMAPHORE_PBS, // Build a semaphore one way path signal.
        WID_BS_SEMAPHORE_PBS_OWAY, // Build an electric normal block signal
        WID_BS_ELECTRIC_NORM, // Build an electric entry block signal
        WID_BS_ELECTRIC_ENTRY, // Build an electric exit block signal
        WID_BS_ELECTRIC_EXIT, // Build an electric combo block signal
        WID_BS_ELECTRIC_COMBO, // Build an electric path signal.
        WID_BS_ELECTRIC_PBS, // Build an electric one way path signal.
        WID_BS_ELECTRIC_PBS_OWAY, // Convert the signal.
        WID_BS_CONVERT, // The current signal density.
        WID_BS_DRAG_SIGNALS_DENSITY_LABEL, // Decrease the signal density.
        WID_BS_DRAG_SIGNALS_DENSITY_DECREASE, // Increase the signal density.
        WID_BS_DRAG_SIGNALS_DENSITY_INCREASE,
    }

/** Widgets of the #BuildRailDepotWindow class. */
    public enum BuildRailDepotWidgets
    {
        /* Name starts with BRA instead of BR, because of collision with BuildRoadDepotWidgets */ // Build a depot with the entrance in the north east.
        WID_BRAD_DEPOT_NE, // Build a depot with the entrance in the south east.
        WID_BRAD_DEPOT_SE, // Build a depot with the entrance in the south west.
        WID_BRAD_DEPOT_SW, // Build a depot with the entrance in the north west.
        WID_BRAD_DEPOT_NW,
    }

/** Widgets of the #BuildRailWaypointWindow class. */
    public enum BuildRailWaypointWidgets
    {
// Matrix with waypoints.
        WID_BRW_WAYPOINT_MATRIX, // A single waypoint.
        WID_BRW_WAYPOINT, // Scrollbar for the matrix.
        WID_BRW_SCROLL,
    }
}