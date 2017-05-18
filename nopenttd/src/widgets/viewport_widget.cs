
/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file viewport_widget.h Types related to the viewport widgets. */
namespace Nopenttd.Widgets
{

    /** Widgets of the #ExtraViewportWindow class. */
    public enum ExtraViewportWidgets
    {
// Caption of window.
        WID_EV_CAPTION, // The viewport.
        WID_EV_VIEWPORT, // Zoom in.
        WID_EV_ZOOM_IN, // Zoom out.
        WID_EV_ZOOM_OUT, // Center the view of this viewport on the main view.
        WID_EV_MAIN_TO_VIEW, // Center the main view on the view of this viewport.
        WID_EV_VIEW_TO_MAIN,
    }
}