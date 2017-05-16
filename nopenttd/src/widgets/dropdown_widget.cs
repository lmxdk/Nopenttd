/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file dropdown_widget.h Types related to the dropdown widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #DropdownWindow class. */
    public enum DropdownMenuWidgets
    {
// Panel showing the dropdown items.
        WID_DM_ITEMS, // Hide scrollbar if too few items.
        WID_DM_SHOW_SCROLL, // Scrollbar.
        WID_DM_SCROLL,
    }

}