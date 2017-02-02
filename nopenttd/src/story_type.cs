/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file story_type.h basic types related to story pages */

namespace Nopenttd
{
    /// ID of a story page element
    public struct StoryPageElementID
    {
        public ushort Id { get; set; }

        public StoryPageElementID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(StoryPageElementID id)
        {
            return id.Id;
        }

        public static implicit operator StoryPageElementID(ushort id)
        {
            return new StoryPageElementID(id);
        }
    }

    /// ID of a story page
    public struct StoryPageID
    {
        public ushort Id { get; set; }

        public StoryPageID(ushort id)
        {
            Id = id;
        }

        public static implicit operator ushort(StoryPageID id)
        {
            return id.Id;
        }

        public static implicit operator StoryPageID(ushort id)
        {
            return new StoryPageID(id);
        }
    }

    public class StoryConstants
    {
        /// Constant representing a non-existing story page element.
        public readonly static StoryPageElementID INVALID_STORY_PAGE_ELEMENT = 0xFFFF;

        /// Constant representing a non-existing story page.
        public readonly static StoryPageID INVALID_STORY_PAGE = 0xFFFF;
    }
}