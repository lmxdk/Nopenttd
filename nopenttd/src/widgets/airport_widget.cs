	/* $Id$ */

	/*
	* This file is part of OpenTTD.
	* OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
	* OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
	* See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
	*/

	/** @file airport_widget.h Types related to the airport widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #BuildAirToolbarWindow class. */
    public enum AirportToolbarWidgets
    {
// Build airport button.
        WID_AT_AIRPORT, // Demolish button.
        WID_AT_DEMOLISH,
    }

    /** Widgets of the #BuildAirportWindow class. */
    public enum AirportPickerWidgets
    {
// Dropdown of airport classes.
        WID_AP_CLASS_DROPDOWN, // List of airports.
        WID_AP_AIRPORT_LIST, // Scrollbar of the list.
        WID_AP_SCROLLBAR, // Current number of the layout.
        WID_AP_LAYOUT_NUM, // Decrease the layout number.
        WID_AP_LAYOUT_DECREASE, // Increase the layout number.
        WID_AP_LAYOUT_INCREASE, // A visual display of the airport currently selected.
        WID_AP_AIRPORT_SPRITE, // Additional text about the airport.
        WID_AP_EXTRA_TEXT, // Panel at the bottom.
        WID_AP_BOTTOMPANEL, // Label if you want to see the coverage.
        WID_AP_COVERAGE_LABEL, // Don't show the coverage button.
        WID_AP_BTN_DONTHILIGHT, // Show the coverage button.
        WID_AP_BTN_DOHILIGHT,
    }
}