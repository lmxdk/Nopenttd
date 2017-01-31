/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file industry_type.h Types related to the industry. */

namespace Nopenttd
{
    public struct IndustryID
    {
        public ushort Id { get; set; }

        public IndustryID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(IndustryID id)
        {
            return id.Id;
        }

        public static implicit operator IndustryID(ushort id)
        {
            return new IndustryID(id);
        }
    }

    public struct IndustryGfx
    {
        public ushort Id { get; set; }

        public IndustryGfx(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(IndustryGfx id)
        {
            return id.Id;
        }

        public static implicit operator IndustryGfx(ushort id)
        {
            return new IndustryGfx(id);
        }
    }

    public struct IndustryType
    {
        public byte Id { get; set; }

        public IndustryType(byte id)
        {
            Id = id;
        }

        public static implicit operator byte(IndustryType id)
        {
            return id.Id;
        }

        public static implicit operator IndustryType(byte id)
        {
            return new IndustryType(id);
        }
    }

    public class IndustryConstants
    {

        public static readonly IndustryID INVALID_INDUSTRY = 0xFFFF;

        /// maximum number of industry types per NewGRF; limited to 128 because bit 7 has a special meaning in some variables/callbacks (see MapNewGRFIndustryType).
        public static readonly IndustryType NUM_INDUSTRYTYPES_PER_GRF = 128;

        /// original number of industry types
        public static readonly IndustryType NEW_INDUSTRYOFFSET = 37;

        /// total number of industry types, new and old; limited to 240 because we need some special ids like INVALID_INDUSTRYTYPE, IT_AI_UNKNOWN, IT_AI_TOWN, ...
        public static readonly IndustryType NUM_INDUSTRYTYPES = 240;

        /// one above amount is considered invalid
        public static readonly IndustryType INVALID_INDUSTRYTYPE = NUM_INDUSTRYTYPES;

        /// Maximum number of industry tiles per NewGRF; limited to 255 to allow extending Action3 with an extended byte later on.
        public static readonly IndustryGfx NUM_INDUSTRYTILES_PER_GRF = 255;

        /// flag to mark industry tiles as having no animation
        public static readonly IndustryGfx INDUSTRYTILE_NOANIM = 0xFF;

        /// original number of tiles
        public static readonly IndustryGfx NEW_INDUSTRYTILEOFFSET = 175;

        /// total number of industry tiles, new and old
        public static readonly IndustryGfx NUM_INDUSTRYTILES = 512;

        /// one above amount is considered invalid
        public static readonly IndustryGfx INVALID_INDUSTRYTILE = NUM_INDUSTRYTILES;

        /// final stage of industry construction.
        public static readonly int INDUSTRY_COMPLETED = 3;
    }
}