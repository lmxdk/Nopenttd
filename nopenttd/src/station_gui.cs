/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file station_gui.h Contains enums and function declarations connected with stations GUI */

namespace Nopenttd
{

/** Types of cargo to display for station coverage. */
    public enum StationCoverageType
    {
        /// Draw only passenger class cargoes.
        SCT_PASSENGERS_ONLY,

        /// Draw all non-passenger class cargoes.
        SCT_NON_PASSENGERS_ONLY,

        /// Draw all cargoes.
        SCT_ALL,
    }
}
