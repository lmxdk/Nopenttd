/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file intro_widget.h Types related to the intro widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #SelectGameWindow class. */
    public enum SelectGameIntroWidgets
    {
// Generate game button.
        WID_SGI_GENERATE_GAME, // Load game button.
        WID_SGI_LOAD_GAME, // Play scenario button.
        WID_SGI_PLAY_SCENARIO, // Play heightmap button.
        WID_SGI_PLAY_HEIGHTMAP, // Edit scenario button.
        WID_SGI_EDIT_SCENARIO, // Play network button.
        WID_SGI_PLAY_NETWORK, // Select temperate landscape button.
        WID_SGI_TEMPERATE_LANDSCAPE, // Select arctic landscape button.
        WID_SGI_ARCTIC_LANDSCAPE, // Select tropic landscape button.
        WID_SGI_TROPIC_LANDSCAPE, // Select toyland landscape button.
        WID_SGI_TOYLAND_LANDSCAPE, // Baseset selection.
        WID_SGI_BASESET_SELECTION, // Baseset errors.
        WID_SGI_BASESET, // Translation selection.
        WID_SGI_TRANSLATION_SELECTION, // Translation errors.
        WID_SGI_TRANSLATION, // Options button.
        WID_SGI_OPTIONS, // Highscore button.
        WID_SGI_HIGHSCORE, // Settings button.
        WID_SGI_SETTINGS_OPTIONS, // NewGRF button.
        WID_SGI_GRF_SETTINGS, // Content Download button.
        WID_SGI_CONTENT_DOWNLOAD, // AI button.
        WID_SGI_AI_SETTINGS, // Exit button.
        WID_SGI_EXIT,
    }
}