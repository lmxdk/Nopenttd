/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file group_type.h Types of a group. */

namespace Nopenttd
{
    /// Type for all group identifiers.
    public struct GroupID
    {
        public ushort Id { get; set; }

        public GroupID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(GroupID id)
        {
            return id.Id;
        }

        public static implicit operator GroupID(ushort id)
        {
            return new GroupID(id);
        }
    }

    public class GroupConstants
    {
        /// Sentinel for a to-be-created group.
        public static readonly GroupID NEW_GROUP = 0xFFFC;

        /// All vehicles are in this group.
        public static readonly GroupID ALL_GROUP = 0xFFFD;

        /// Ungrouped vehicles are in this group.
        public static readonly GroupID DEFAULT_GROUP = 0xFFFE;

        /// Sentinel for invalid groups.
        public static readonly GroupID INVALID_GROUP = 0xFFFF;

        /// The maximum length of a group name in characters including '\0'
        public static readonly uint MAX_LENGTH_GROUP_NAME_CHARS = 32; 
    }
}