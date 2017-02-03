/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file transport_type.h Base types related to transport. */

namespace Nopenttd
{

/** Type for the company global vehicle unit number. */

    public struct UnitID
    {
        public ushort Id { get; set; }

        public UnitID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(UnitID id)
        {
            return id.Id;
        }

        public static implicit operator UnitID(ushort id)
        {
            return new UnitID(id);
        }
    }

/** Available types of transport */

    public enum TransportType
    {
        /* These constants are for now linked to the representation of bridges
         * and tunnels, so they can be used by GetTileTrackStatus_TunnelBridge.
         * In an ideal world, these constants would be used everywhere when
         * accessing tunnels and bridges. For now, you should just not change
         * the values for road and rail.
         */

        /// Begin of the iterator.
        TRANSPORT_BEGIN = 0,

        /// Transport by train
        TRANSPORT_RAIL = TRANSPORT_BEGIN,

        /// Transport by road vehicle
        TRANSPORT_ROAD,

        /// Transport over water
        TRANSPORT_WATER,

        /// Transport through air
        TRANSPORT_AIR,

        /// End of iterations.
        TRANSPORT_END,

        /// Sentinel for invalid transport types.
        INVALID_TRANSPORT = 0xff,
    }

/** Helper information for extract tool. */
//template <> struct EnumPropsT<TransportType> : MakeEnumPropsT<TransportType, byte, TRANSPORT_BEGIN, TRANSPORT_END, INVALID_TRANSPORT, 2> {};
}