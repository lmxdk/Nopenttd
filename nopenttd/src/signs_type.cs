/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file signs_type.h Types related to signs */

namespace Nopenttd
{
    /** The type of the IDs of signs. */

    public struct SignID
    {
        public ushort Label { get; set; }

        public SignID(ushort label)
        {
            Label = label;
        }

        public static implicit operator ushort(SignID label)
        {
            return label.Label;
        }

        public static implicit operator SignID(ushort label)
        {
            return new SignID(label);
        }
    }

    public class SignConstants
    {

        /// Sentinel for an invalid sign.
        public static readonly SignID INVALID_SIGN = 0xFFFF;

        /// The maximum length of a sign name in characters including '\0'
        public const uint MAX_LENGTH_SIGN_NAME_CHARS = 32;
    }
}


