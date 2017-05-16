    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file transparency_widget.h Types related to the transparency widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #TransparenciesWindow class. */
    public enum TransparencyToolbarWidgets
    {
        /* Button row. */ // First toggle button.
        WID_TT_BEGIN, // Signs background transparency toggle button.
        WID_TT_SIGNS = WID_TT_BEGIN, // Trees transparency toggle button.
        WID_TT_TREES, // Houses transparency toggle button.
        WID_TT_HOUSES, // industries transparency toggle button.
        WID_TT_INDUSTRIES, // Company buildings and structures transparency toggle button.
        WID_TT_BUILDINGS, // Bridges transparency toggle button.
        WID_TT_BRIDGES, // Object structure transparency toggle button.
        WID_TT_STRUCTURES, // Catenary transparency toggle button.
        WID_TT_CATENARY, // Loading indicators transparency toggle button.
        WID_TT_LOADING, // End of toggle buttons.
        WID_TT_END,

        /* Panel with buttons for invisibility */ // Panel with 'invisibility' buttons.
        WID_TT_BUTTONS,
    }
}
