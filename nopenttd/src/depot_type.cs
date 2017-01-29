/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file depot_type.h Header files for depots (not hangars) */

namespace Nopenttd
{

    public struct Depot
    {

    }

    public static class DepotConstants
    {
        ///The maximum length of a depot name in characters including '\0'
        public const uint MAX_LENGTH_DEPOT_NAME_CHARS = 32;
    }

    ///Type for the unique identifier of depots.
    public struct DepotID
    {
        public ushort Id { get; set; }

        public DepotID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(DepotID id)
        {
            return id.Id;
        }

        public static implicit operator DepotID(ushort id)
        {
            return new DepotID(id);
        }
    }
}