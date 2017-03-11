/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file viewport_type.h Types related to viewports. */

//#include "zoom_type.h"
//#include "strings_type.h"
//#include "table/strings.h"

using System;
using Nopenttd.src;

namespace Nopenttd
{


/**
 * Data structure for viewport, display of a part of the world
 */

    public struct ViewPort
    {
        /// Screen coordinate left egde of the viewport
        int left;

        /// Screen coordinate top edge of the viewport
        int top;

        /// Screen width of the viewport
        int width;

        /// Screen height of the viewport
        int height;

        /// Virtual left coordinate
        int virtual_left;

        /// Virtual top coordinate 
        int virtual_top;

        /// width << zoom 
        int virtual_width;

        /// height << zoom 
        int virtual_height;

        /// The zoom level of the viewport.
        ZoomLevel zoom;

        LinkGraphOverlay overlay;
    }

/** Margins for the viewport sign */

    public enum ViewportSignMargin
    {
        /// Left margin
        VPSM_LEFT = 1,

        /// Right margin
        VPSM_RIGHT = 1,

        /// Top margin
        VPSM_TOP = 1,

        /// Bottom margin
        VPSM_BOTTOM = 1,
    }

/** Location information about a sign as seen on the viewport */

    public struct ViewportSign
    {
        /// The center position of the sign
        int center;

        /// The top of the sign
        int top;

        /// The width when not zoomed out (normal font)
        ushort width_normal;

        /// The width when zoomed out (small font)
        ushort width_small;

        public void UpdatePosition(int center, int top, StringID str, StringID? str_small = null)
        {
        }

        public void MarkDirty(ZoomLevel maxzoom = ZoomLevel.ZOOM_LVL_MAX)
        {
        }
    };

/**
 * Directions of zooming.
 * @see DoZoomInOutWindow
 */

    public enum ZoomStateChange
    {
        /// Zoom in (get more detailed view).
        ZOOM_IN = 0,

        /// Zoom out (get helicopter view).
        ZOOM_OUT = 1,

        /// Hack, used to update the button status.
        ZOOM_NONE = 2,
    };

/**
 * Some values for constructing bounding boxes (BB). The Z positions under bridges are:
 * z=0..5  Everything that can be built under low bridges.
 * z=6     reserved, currently unused.
 * z=7     Z separator between bridge/tunnel and the things under/above it.
 */

    public static class ViewPortConstants
    {
        ///Everything that can be built under low bridges, must not exceed this Z height.
        public const uint BB_HEIGHT_UNDER_BRIDGE = 6;

        /// Separates the bridge/tunnel from the things under/above it.
        public const uint BB_Z_SEPARATOR = 7;
    }

/** Viewport place method (type of highlighted area and placed objects) */

    [Flags]
    public enum ViewportPlaceMethod
    {
        /// drag in X or Y direction
        VPM_X_OR_Y = 0,

        /// drag only in X axis
        VPM_FIX_X = 1,

        /// drag only in Y axis
        VPM_FIX_Y = 2,

        /// area of land in X and Y directions
        VPM_X_AND_Y = 3,

        /// area of land of limited size
        VPM_X_AND_Y_LIMITED = 4,

        /// drag only in horizontal direction
        VPM_FIX_HORIZONTAL = 5,

        /// drag only in vertical direction
        VPM_FIX_VERTICAL = 6,

        /// Drag only in X axis with limited size
        VPM_X_LIMITED = 7,

        /// Drag only in Y axis with limited size
        VPM_Y_LIMITED = 8,

        /// all rail directions
        VPM_RAILDIRS = 0x40,

        /// similar to VMP_RAILDIRS, but with different cursor
        VPM_SIGNALDIRS = 0x80,
    };

/**
 * Drag and drop selection process, or, what to do with an area of land when
 * you've selected it.
 */

    public enum ViewportDragDropSelectionProcess
    {
        /// Clear area
        DDSP_DEMOLISH_AREA,

        /// Raise / level area
        DDSP_RAISE_AND_LEVEL_AREA,

        /// Lower / level area
        DDSP_LOWER_AND_LEVEL_AREA,

        /// Level area
        DDSP_LEVEL_AREA,

        /// Fill area with desert
        DDSP_CREATE_DESERT,

        /// Fill area with rocks
        DDSP_CREATE_ROCKS,

        /// Create a canal
        DDSP_CREATE_WATER,

        /// Create rivers
        DDSP_CREATE_RIVER,

        /// Plant trees
        DDSP_PLANT_TREES,

        /// Bridge placement
        DDSP_BUILD_BRIDGE,

        /* Rail specific actions */

        /// Rail placement
        DDSP_PLACE_RAIL,

        /// Signal placement
        DDSP_BUILD_SIGNALS,

        /// Station placement
        DDSP_BUILD_STATION,

        /// Station removal
        DDSP_REMOVE_STATION,

        /// Rail conversion
        DDSP_CONVERT_RAIL,

        /* Road specific actions */

        /// Road placement (X axis)
        DDSP_PLACE_ROAD_X_DIR,

        /// Road placement (Y axis)
        DDSP_PLACE_ROAD_Y_DIR,

        /// Road placement (auto)
        DDSP_PLACE_AUTOROAD,

        /// Road stop placement (buses)
        DDSP_BUILD_BUSSTOP,

        /// Road stop placement (trucks)
        DDSP_BUILD_TRUCKSTOP,

        /// Road stop removal (buses)
        DDSP_REMOVE_BUSSTOP,

        /// Road stop removal (trucks)
        DDSP_REMOVE_TRUCKSTOP,
    }
}