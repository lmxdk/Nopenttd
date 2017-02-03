/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file zoom_type.h Types related to zooming in and out. */

using System;

namespace Nopenttd
{
    public class Zoom
    {
        public const int ZOOM_LVL_SHIFT = 2;
        public const int ZOOM_LVL_BASE = 1 << ZOOM_LVL_SHIFT;

        public static ZoomLevel _gui_zoom = ZoomLevel.ZOOM_LVL_NORMAL;


        /**
         * Scale by zoom level, usually shift left (when zoom > ZOOM_LVL_NORMAL)
         * When shifting right, value is rounded up
         * @param value value to shift
         * @param zoom  zoom level to shift to
         * @return shifted value
         */
        //inline
        public static int ScaleByZoom(int value, ZoomLevel zoom)
        {
            if (zoom < 0) throw new ArgumentOutOfRangeException(nameof(zoom), "must be greater than zero");
            return value << (int) zoom;
        }

        /**
         * Scale by zoom level, usually shift right (when zoom > ZOOM_LVL_NORMAL)
         * When shifting right, value is rounded up
         * @param value value to shift
         * @param zoom  zoom level to shift to
         * @return shifted value
         */
        //inline
        public static int UnScaleByZoom(int value, ZoomLevel zoom)
        {
            if (zoom < 0) throw new ArgumentOutOfRangeException(nameof(zoom), "must be greater than zero");
            return (value + (1 << (int) zoom) - 1) >> (int) zoom;
        }

        /**
         * Scale by zoom level, usually shift left (when zoom > ZOOM_LVL_NORMAL)
         * @param value value to shift
         * @param zoom  zoom level to shift to
         * @return shifted value
         */
        //inline
        public static int ScaleByZoomLower(int value, ZoomLevel zoom)
        {
            if (zoom < 0) throw new ArgumentOutOfRangeException(nameof(zoom), "must be greater than zero");
            return value << (int) zoom;
        }

        /**
         * Scale by zoom level, usually shift right (when zoom > ZOOM_LVL_NORMAL)
         * @param value value to shift
         * @param zoom  zoom level to shift to
         * @return shifted value
         */
        //inline
        public static int UnScaleByZoomLower(int value, ZoomLevel zoom)
        {
            if (zoom < 0) throw new ArgumentOutOfRangeException(nameof(zoom), "must be greater than zero");
            return value >> (int) zoom;
        }

        /**
         * Short-hand to apply GUI zoom level.
         * @param value Pixel amount at #ZOOM_LVL_BEGIN (full zoom in).
         * @return Pixel amount at #ZOOM_LVL_GUI (current interface size).
         */
        //inline
        public static int UnScaleGUI(int value)
        {
            return UnScaleByZoom(value, _gui_zoom);
        }

        /**
         * Scale traditional pixel dimensions to GUI zoom level.
         * @param value Pixel amount at #ZOOM_LVL_BASE (traditional "normal" interface size).
         * @return Pixel amount at #ZOOM_LVL_GUI (current interface size).
         */
        //inline
        public static int ScaleGUITrad(int value)
        {
            return UnScaleGUI(value * ZOOM_LVL_BASE);
        }

    }

    /** All zoom levels we know. */

    public enum ZoomLevel
    {
        /* Our possible zoom-levels */

        /// Begin for iteration.
        ZOOM_LVL_BEGIN = 0,

        /// The normal zoom level.
        ZOOM_LVL_NORMAL = 0,

        /// Zoomed 2 times out.
        ZOOM_LVL_OUT_2X,

        /// Zoomed 4 times out.
        ZOOM_LVL_OUT_4X,

        /// Zoomed 8 times out.
        ZOOM_LVL_OUT_8X,

        /// Zoomed 16 times out.
        ZOOM_LVL_OUT_16X,

        /// Zoomed 32 times out.
        ZOOM_LVL_OUT_32X,

        /// End for iteration.
        ZOOM_LVL_END,

        /// Number of zoom levels.
        ZOOM_LVL_COUNT = ZOOM_LVL_END - ZOOM_LVL_BEGIN,

        /* Here we define in which zoom viewports are */

        /// Default zoom level for viewports.
        ZOOM_LVL_VIEWPORT = ZOOM_LVL_OUT_4X,

        /// Default zoom level for the news messages.
        ZOOM_LVL_NEWS = ZOOM_LVL_OUT_4X,

        /// Default zoom level for the industry view.
        ZOOM_LVL_INDUSTRY = ZOOM_LVL_OUT_8X,

        /// Default zoom level for the town view.
        ZOOM_LVL_TOWN = ZOOM_LVL_OUT_8X,

        /// Default zoom level for the aircraft view.
        ZOOM_LVL_AIRCRAFT = ZOOM_LVL_OUT_4X,

        /// Default zoom level for the ship view.
        ZOOM_LVL_SHIP = ZOOM_LVL_OUT_4X,

        /// Default zoom level for the train view.
        ZOOM_LVL_TRAIN = ZOOM_LVL_OUT_4X,

        /// Default zoom level for the road vehicle view.
        ZOOM_LVL_ROADVEH = ZOOM_LVL_OUT_4X,

        /// Default zoom level for the world screen shot.
        ZOOM_LVL_WORLD_SCREENSHOT = ZOOM_LVL_OUT_4X,

        /// All zoomlevels below or equal to this, will result in details on the screen, like road-work, ...
        ZOOM_LVL_DETAIL = ZOOM_LVL_OUT_8X,

        /// Minimum zoom level.
        /// Maximum zoom level.
        ZOOM_LVL_MIN = ZOOM_LVL_NORMAL,
        ZOOM_LVL_MAX = ZOOM_LVL_OUT_32X,
    }

//DECLARE_POSTFIX_INCREMENT(ZoomLevel)

/** Type for storing the zoom level in a byte. */
//typedef SimpleTinyEnumT<ZoomLevel, byte> ZoomLevelByte;

//extern ZoomLevelByte _gui_zoom;
//#define ZOOM_LVL_GUI (_gui_zoom)
}
