
/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file newgrf_debug_widget.h Types related to the newgrf debug widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #NewGRFInspectWindow class. */
    public enum NewGRFInspectWidgets
    {
// The caption bar of course.
        WID_NGRFI_CAPTION, // Inspect the parent.
        WID_NGRFI_PARENT, // Go to previous vehicle in chain.
        WID_NGRFI_VEH_PREV, // Go to next vehicle in chain.
        WID_NGRFI_VEH_NEXT, // Display for vehicle chain.
        WID_NGRFI_VEH_CHAIN, // Panel widget containing the actual data.
        WID_NGRFI_MAINPANEL, // Scrollbar.
        WID_NGRFI_SCROLLBAR,
    }

/** Widgets of the #SpriteAlignerWindow class. */
    public enum SpriteAlignerWidgets
    {
// Caption of the window.
        WID_SA_CAPTION, // Skip to the previous sprite.
        WID_SA_PREVIOUS, // Go to a given sprite.
        WID_SA_GOTO, // Skip to the next sprite.
        WID_SA_NEXT, // Move the sprite up.
        WID_SA_UP, // Move the sprite to the left.
        WID_SA_LEFT, // Move the sprite to the right.
        WID_SA_RIGHT, // Move the sprite down.
        WID_SA_DOWN, // The actual sprite.
        WID_SA_SPRITE, // The sprite offsets (absolute).
        WID_SA_OFFSETS_ABS, // The sprite offsets (relative).
        WID_SA_OFFSETS_REL, // Sprite picker.
        WID_SA_PICKER, // Queried sprite list.
        WID_SA_LIST, // Scrollbar for sprite list.
        WID_SA_SCROLLBAR, // Reset relative sprite offset
        WID_SA_RESET_REL,
    }

}