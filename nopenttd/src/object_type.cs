/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file object_type.h Types related to object tiles. */

namespace Nopenttd
{
    /** Types of objects. */

    public struct ObjectType
    {
        public ushort Id { get; set; }

        public ObjectType(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(ObjectType id)
        {
            return id.Id;
        }

        public static implicit operator ObjectType(ushort id)
        {
            return new ObjectType(id);
        }
    }

    public class ObjectConstants
    {
        /// The large antenna
        public static readonly ObjectType OBJECT_TRANSMITTER = 0;

        /// The nice lighthouse
        public static readonly ObjectType OBJECT_LIGHTHOUSE = 1;

        /// Statue in towns
        public static readonly ObjectType OBJECT_STATUE = 2;

        /// Owned land 'flag'
        public static readonly ObjectType OBJECT_OWNED_LAND = 3;

        /// HeadQuarter of a player
        public static readonly ObjectType OBJECT_HQ = 4;

        /// Number of supported objects per NewGRF; limited to 255 to allow extending Action3 with an extended byte later on.
        public static readonly ObjectType NUM_OBJECTS_PER_GRF = 255;

        /// Offset for new objects
        public static readonly ObjectType NEW_OBJECT_OFFSET = 5;

        /// Number of supported objects overall
        public static readonly ObjectType NUM_OBJECTS = 64000;

        /// An invalid object
        public static readonly ObjectType INVALID_OBJECT_TYPE = 0xFFFF;


        /// An invalid object
        public static readonly ObjectID INVALID_OBJECT = 0xFFFFFFFF;
    }

/** Unique identifier for an object. */

    public struct ObjectID
    {
        public uint Id { get; set; }

        public ObjectID(uint id)
        {
            Id = id;
        }

        public static implicit operator uint(ObjectID id)
        {
            return id.Id;
        }

        public static implicit operator ObjectID(uint id)
        {
            return new ObjectID(id);
        }
    }
}