/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file build_vehicle_widget.h Types related to the build_vehicle widgets. */

namespace Nopenttd.Widgets
{

/** Widgets of the #BuildVehicleWindow class. */
    public enum BuildVehicleWidgets
    {
// Caption of window.
        WID_BV_CAPTION, // Sort direction.
        WID_BV_SORT_ASCENDING_DESCENDING, // Criteria of sorting dropdown.
        WID_BV_SORT_DROPDOWN, // Cargo filter dropdown.
        WID_BV_CARGO_FILTER_DROPDOWN, // Toggle whether to display the hidden vehicles.
        WID_BV_SHOW_HIDDEN_ENGINES, // List of vehicles.
        WID_BV_LIST, // Scrollbar of list.
        WID_BV_SCROLLBAR, // Button panel.
        WID_BV_PANEL, // Build panel.
        WID_BV_BUILD, // Button to hide or show the selected engine.
        WID_BV_SHOW_HIDE, // Build button.
        WID_BV_BUILD_SEL, // Rename button.
        WID_BV_RENAME,
    }

}
