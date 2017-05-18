

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file osk_widget.h Types related to the osk widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #OskWindow class. */
    public enum OnScreenKeyboardWidgets
    {
// Caption of window.
        WID_OSK_CAPTION, // Edit box.
        WID_OSK_TEXT, // Cancel key.
        WID_OSK_CANCEL, // Ok key.
        WID_OSK_OK, // Backspace key.
        WID_OSK_BACKSPACE, // Special key (at keyboards often used for tab key).
        WID_OSK_SPECIAL, // Capslock key.
        WID_OSK_CAPS, // Shift(lock) key.
        WID_OSK_SHIFT, // Space bar.
        WID_OSK_SPACE, // Cursor left key.
        WID_OSK_LEFT, // Cursor right key.
        WID_OSK_RIGHT, // First widget of the 'normal' keys.
        WID_OSK_LETTERS,

        // First widget of the numbers row.
        WID_OSK_NUMBERS_FIRST = WID_OSK_LETTERS, // Last widget of the numbers row.
        WID_OSK_NUMBERS_LAST = WID_OSK_NUMBERS_FIRST + 13,

        // First widget of the qwerty row.
        WID_OSK_QWERTY_FIRST, // Last widget of the qwerty row.
        WID_OSK_QWERTY_LAST = WID_OSK_QWERTY_FIRST + 11,

        // First widget of the asdfg row.
        WID_OSK_ASDFG_FIRST, // Last widget of the asdfg row.
        WID_OSK_ASDFG_LAST = WID_OSK_ASDFG_FIRST + 11,

        // First widget of the zxcvb row.
        WID_OSK_ZXCVB_FIRST, // Last widget of the zxcvb row.
        WID_OSK_ZXCVB_LAST = WID_OSK_ZXCVB_FIRST + 11,
    }
}
