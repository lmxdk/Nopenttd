    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file sign_widget.h Types related to the sign widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #SignListWindow class. */
    public enum SignListWidgets
    {
        /* Name starts with SI instead of S, because of collision with SaveLoadWidgets */ // Caption of the window.
        WID_SIL_CAPTION, // List of signs.
        WID_SIL_LIST, // Scrollbar of list.
        WID_SIL_SCROLLBAR, // Text box for typing a filter string.
        WID_SIL_FILTER_TEXT, // Button to toggle if case sensitive filtering should be used.
        WID_SIL_FILTER_MATCH_CASE_BTN, // Scroll to first sign.
        WID_SIL_FILTER_ENTER_BTN,
    }

/** Widgets of the #SignWindow class. */
    public enum QueryEditSignWidgets
    {
// Caption of the window.
        WID_QES_CAPTION, // Text of the query.
        WID_QES_TEXT, // OK button.
        WID_QES_OK, // Cancel button.
        WID_QES_CANCEL, // Delete button.
        WID_QES_DELETE, // Previous button.
        WID_QES_PREVIOUS, // Next button.
        WID_QES_NEXT,
    }
}