
/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file music_widget.h Types related to the music widgets. */
namespace Nopenttd.Widgets
{

    /** Widgets of the #MusicTrackSelectionWindow class. */
    public enum MusicTrackSelectionWidgets
    {
// Left button.
        WID_MTS_LIST_LEFT, // Playlist.
        WID_MTS_PLAYLIST, // Right button.
        WID_MTS_LIST_RIGHT, // All button.
        WID_MTS_ALL, // Old button.
        WID_MTS_OLD, // New button.
        WID_MTS_NEW, // Ezy button.
        WID_MTS_EZY, // Custom1 button.
        WID_MTS_CUSTOM1, // Custom2 button.
        WID_MTS_CUSTOM2, // Clear button.
        WID_MTS_CLEAR,
    }

/** Widgets of the #MusicWindow class. */
    public enum MusicWidgets
    {
// Previous button.
        WID_M_PREV, // Next button.
        WID_M_NEXT, // Stop button.
        WID_M_STOP, // Play button.
        WID_M_PLAY, // Sliders.
        WID_M_SLIDERS, // Music volume.
        WID_M_MUSIC_VOL, // Effect volume.
        WID_M_EFFECT_VOL, // Background of the window.
        WID_M_BACKGROUND, // Track playing.
        WID_M_TRACK, // Track number.
        WID_M_TRACK_NR, // Track title.
        WID_M_TRACK_TITLE, // Track name.
        WID_M_TRACK_NAME, // Shuffle button.
        WID_M_SHUFFLE, // Program button.
        WID_M_PROGRAMME, // All button.
        WID_M_ALL, // Old button.
        WID_M_OLD, // New button.
        WID_M_NEW, // Ezy button.
        WID_M_EZY, // Custom1 button.
        WID_M_CUSTOM1, // Custom2 button.
        WID_M_CUSTOM2,
    }

}