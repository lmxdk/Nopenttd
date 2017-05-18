
    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file order_widget.h Types related to the order widgets. */
namespace Nopenttd.Widgets
{
    /** Widgets of the #OrdersWindow class. */
    public enum OrderWidgets
    {
// Caption of the window.
        WID_O_CAPTION, // Toggle timetable view.
        WID_O_TIMETABLE_VIEW, // Order list panel.
        WID_O_ORDER_LIST, // Order list scrollbar.
        WID_O_SCROLLBAR, // Skip current order.
        WID_O_SKIP, // Delete selected order.
        WID_O_DELETE, // Stop sharing orders.
        WID_O_STOP_SHARING, // Goto non-stop to destination.
        WID_O_NON_STOP, // Goto destination.
        WID_O_GOTO, // Select full load.
        WID_O_FULL_LOAD, // Select unload.
        WID_O_UNLOAD, // Select refit.
        WID_O_REFIT, // Select service (at depot).
        WID_O_SERVICE, // Placeholder for refit dropdown when not owner.
        WID_O_EMPTY, // Open refit options.
        WID_O_REFIT_DROPDOWN, // Choose condition variable.
        WID_O_COND_VARIABLE, // Choose condition type.
        WID_O_COND_COMPARATOR, // Choose condition value.
        WID_O_COND_VALUE, // #NWID_SELECTION widget for left part of the top row of the 'your train' order window.
        WID_O_SEL_TOP_LEFT, // #NWID_SELECTION widget for middle part of the top row of the 'your train' order window.
        WID_O_SEL_TOP_MIDDLE, // #NWID_SELECTION widget for right part of the top row of the 'your train' order window.
        WID_O_SEL_TOP_RIGHT, // #NWID_SELECTION widget for the top row of the 'your train' order window.
        WID_O_SEL_TOP_ROW_GROUNDVEHICLE, // #NWID_SELECTION widget for the top row of the 'your non-trains' order window.
        WID_O_SEL_TOP_ROW, // #NWID_SELECTION widget for the middle part of the bottom row of the 'your train' order window.
        WID_O_SEL_BOTTOM_MIDDLE, // Open list of shared vehicles.
        WID_O_SHARED_ORDER_LIST,
    }
}