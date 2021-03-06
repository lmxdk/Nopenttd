
    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file object_widget.h Types related to the object widgets. */

namespace Nopenttd.Widgets
{

    /** Widgets of the #BuildObjectWindow class. */
    public enum BuildObjectWidgets
    {
// The list with classes.
        WID_BO_CLASS_LIST, // The scrollbar associated with the list.
        WID_BO_SCROLLBAR, // The matrix with preview sprites.
        WID_BO_OBJECT_MATRIX, // A preview sprite of the object.
        WID_BO_OBJECT_SPRITE, // The name of the selected object.
        WID_BO_OBJECT_NAME, // The size of the selected object.
        WID_BO_OBJECT_SIZE, // Other information about the object (from the NewGRF).
        WID_BO_INFO,

// Selection preview matrix of objects of a given class.
        WID_BO_SELECT_MATRIX, // Preview image in the #WID_BO_SELECT_MATRIX.
        WID_BO_SELECT_IMAGE, // Scrollbar next to the #WID_BO_SELECT_MATRIX.
        WID_BO_SELECT_SCROLL,
    }
}
