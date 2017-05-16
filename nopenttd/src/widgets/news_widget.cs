/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file news_widget.h Types related to the news widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #NewsWindow class. */
    public enum NewsWidgets
    {
// Panel of the window.
        WID_N_PANEL, // Title of the company news.
        WID_N_TITLE, // The news headline.
        WID_N_HEADLINE, // Close the window.
        WID_N_CLOSEBOX, // Date of the news item.
        WID_N_DATE, // Title bar of the window. Only used in small news items.
        WID_N_CAPTION, // Inset around the viewport in the window. Only used in small news items.
        WID_N_INSET, // Viewport in the window.
        WID_N_VIEWPORT, // Message in company news items.
        WID_N_COMPANY_MSG, // Space for displaying the message. Only used in small news items.
        WID_N_MESSAGE, // Face of the manager.
        WID_N_MGR_FACE, // Name of the manager.
        WID_N_MGR_NAME, // Vehicle new title.
        WID_N_VEH_TITLE, // Dark background of new vehicle news.
        WID_N_VEH_BKGND, // Name of the new vehicle.
        WID_N_VEH_NAME, // Graphical display of the new vehicle.
        WID_N_VEH_SPR, // Some technical data of the new vehicle.
        WID_N_VEH_INFO,
    }

/** Widgets of the #MessageHistoryWindow class. */
    public enum MessageHistoryWidgets
    {
// Stickybox.
        WID_MH_STICKYBOX, // Background of the window.
        WID_MH_BACKGROUND, // Scrollbar for the list.
        WID_MH_SCROLLBAR,
    }

}
