

    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file dock_widget.h Types related to the dock widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #BuildDocksDepotWindow class. */
    public enum BuildDockDepotWidgets
    {
// Background of the window.
        WID_BDD_BACKGROUND, // X-direction button.
        WID_BDD_X, // Y-direction button.
        WID_BDD_Y,
    }

/** Widgets of the #BuildDocksToolbarWindow class. */
    public enum DockToolbarWidgets
    {
// Build canal button.
        WID_DT_CANAL, // Build lock button.
        WID_DT_LOCK, // Demolish aka dynamite button.
        WID_DT_DEMOLISH, // Build depot button.
        WID_DT_DEPOT, // Build station button.
        WID_DT_STATION, // Build buoy button.
        WID_DT_BUOY, // Build river button (in scenario editor).
        WID_DT_RIVER, // Build aqueduct button.
        WID_DT_BUILD_AQUEDUCT,

// Used to initialize a variable.
        WID_DT_INVALID,
    }

}
