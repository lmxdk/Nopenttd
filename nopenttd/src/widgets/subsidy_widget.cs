    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file subsidy_widget.h Types related to the subsidy widgets. */
namespace Nopenttd.Widgets
{

    /** Widgets of the #SubsidyListWindow class. */
    public enum SubsidyListWidgets
    {
        /* Name starts with SU instead of S, because of collision with SaveLoadWidgets. */ // Main panel of window.
        WID_SUL_PANEL, // Scrollbar of panel.
        WID_SUL_SCROLLBAR,
    }
}