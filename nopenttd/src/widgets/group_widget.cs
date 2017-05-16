
    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file group_widget.h Types related to the group widgets. */

namespace Nopenttd.Widgets
{
    public enum GroupListWidgets
    {
// Caption of the window.
        WID_GL_CAPTION, // Sort order.
        WID_GL_SORT_BY_ORDER, // Sort by dropdown list.
        WID_GL_SORT_BY_DROPDOWN, // List of the vehicles.
        WID_GL_LIST_VEHICLE, // Scrollbar for the list.
        WID_GL_LIST_VEHICLE_SCROLLBAR, // Available vehicles.
        WID_GL_AVAILABLE_VEHICLES, // Manage vehicles dropdown list.
        WID_GL_MANAGE_VEHICLES_DROPDOWN, // Stop all button.
        WID_GL_STOP_ALL, // Start all button.
        WID_GL_START_ALL,

// All vehicles entry.
        WID_GL_ALL_VEHICLES, // Default vehicles entry.
        WID_GL_DEFAULT_VEHICLES, // List of the groups.
        WID_GL_LIST_GROUP, // Scrollbar for the list.
        WID_GL_LIST_GROUP_SCROLLBAR, // Create group button.
        WID_GL_CREATE_GROUP, // Delete group button.
        WID_GL_DELETE_GROUP, // Rename group button.
        WID_GL_RENAME_GROUP, // Replace protection button.
        WID_GL_REPLACE_PROTECTION,
    }

}