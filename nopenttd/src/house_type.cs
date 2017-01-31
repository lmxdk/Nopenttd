/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file house_type.h declaration of basic house types and enums */


namespace Nopenttd
{
    /// OpenTTD ID of house types.
    public struct HouseID
    {
        public ushort Id { get; set; }

        public HouseID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(HouseID id)
        {
            return id.Id;
        }

        public static implicit operator HouseID(ushort id)
        {
            return new HouseID(id);
        }
    }

    /// Classes of houses.
    public struct HouseClassID
    {
        public ushort Id { get; set; }

        public HouseClassID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(HouseClassID id)
        {
            return id.Id;
        }

        public static implicit operator HouseClassID(ushort id)
        {
            return new HouseClassID(id);
        }
    }

//struct HouseSpec;
}