/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file landscape_type.h Types related to the landscape. */

namespace Nopenttd
{
    /// Landscape type. @see LandscapeType
    public struct LandscapeID
    {
        public byte Id { get; set; }

        public LandscapeID(byte id)
        {
            Id = id;
        }

        public static implicit operator byte(LandscapeID id)
        {
            return id.Id;
        }

        public static implicit operator LandscapeID(byte id)
        {
            return new LandscapeID(id);
        }
    }


/** Landscape types */

    public enum LandscapeType
    {
        LT_TEMPERATE = 0,
        LT_ARCTIC = 1,
        LT_TROPIC = 2,
        LT_TOYLAND = 3,

        NUM_LANDSCAPE = 4,
    }

/**
 * For storing the water borders which shall be retained.
 */

    public enum Borders
    {
        BORDER_NE = 0,
        BORDER_SE = 1,
        BORDER_SW = 2,
        BORDER_NW = 3,
        BORDERS_RANDOM = 16,
    }
}