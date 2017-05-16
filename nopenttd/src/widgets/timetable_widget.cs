
    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file timetable_widget.h Types related to the timetable widgets. */
namespace Nopenttd.Widgets
{
    /** Widgets of the #TimetableWindow class. */
    public enum VehicleTimetableWidgets
    {
// Caption of the window.
        WID_VT_CAPTION, // Order view.
        WID_VT_ORDER_VIEW, // Timetable panel.
        WID_VT_TIMETABLE_PANEL, // Panel with the expected/scheduled arrivals.
        WID_VT_ARRIVAL_DEPARTURE_PANEL, // Scrollbar for the panel.
        WID_VT_SCROLLBAR, // Summary panel.
        WID_VT_SUMMARY_PANEL, // Start date button.
        WID_VT_START_DATE, // Change time button.
        WID_VT_CHANGE_TIME, // Clear time button.
        WID_VT_CLEAR_TIME, // Reset lateness button.
        WID_VT_RESET_LATENESS, // Autofill button.
        WID_VT_AUTOFILL, // Toggle between expected and scheduled arrivals.
        WID_VT_EXPECTED, // Show the shared order list.
        WID_VT_SHARED_ORDER_LIST, // Disable/hide the arrival departure panel.
        WID_VT_ARRIVAL_DEPARTURE_SELECTION, // Disable/hide the expected selection button.
        WID_VT_EXPECTED_SELECTION, // Change speed limit button.
        WID_VT_CHANGE_SPEED, // Clear speed limit button.
        WID_VT_CLEAR_SPEED,
    }
}