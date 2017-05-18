    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file waypoint_widget.h Types related to the waypoint widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #WaypointWindow class. */
    public enum WaypointWidgets
    {
// Caption of window.
        WID_W_CAPTION, // The viewport on this waypoint.
        WID_W_VIEWPORT, // Center the main view on this waypoint.
        WID_W_CENTER_VIEW, // Rename this waypoint.
        WID_W_RENAME, // Show the vehicles visiting this waypoint.
        WID_W_SHOW_VEHICLES,
    }
}