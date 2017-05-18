/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file screenshot.h Functions to make screenshots. */

namespace Nopenttd
{
/** Type of requested screenshot */
    public enum ScreenshotType
    {
        /// Screenshot of viewport.
        SC_VIEWPORT,

        /// Raw screenshot from blitter buffer.
        SC_CRASHLOG,

        /// Fully zoomed in screenshot of the visible area.
        SC_ZOOMEDIN,

        /// Zoomed to default zoom level screenshot of the visible area.
        SC_DEFAULTZOOM,

        /// World screenshot.
        SC_WORLD,

        /// Heightmap of the world.
        SC_HEIGHTMAP,
    }
}
