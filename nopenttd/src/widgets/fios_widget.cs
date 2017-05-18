

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file fios_widget.h Types related to the fios widgets. */

namespace Nopenttd.Widgets{

	/** Widgets of the #SaveLoadWindow class. */
	public enum SaveLoadWidgets {// Caption of the window.
		WID_SL_CAPTION,                 // Sort by name button.
		WID_SL_SORT_BYNAME,             // Sort by date button.
		WID_SL_SORT_BYDATE,             // Background of window.
		WID_SL_BACKGROUND,              // Background of file selection.
		WID_SL_FILE_BACKGROUND,         // Home button.
		WID_SL_HOME_BUTTON,             // Drives list.
		WID_SL_DRIVES_DIRECTORIES_LIST, // Scrollbar of the file list.
		WID_SL_SCROLLBAR,               // Content download button, only available for play scenario/heightmap.
		WID_SL_CONTENT_DOWNLOAD,        // Title textbox, only available for save operations.
		WID_SL_SAVE_OSK_TITLE,          // Delete button, only available for save operations.
		WID_SL_DELETE_SELECTION,        // Save button, only available for save operations.
		WID_SL_SAVE_GAME,               // Selection 'stack' to 'hide' the content download.
		WID_SL_CONTENT_DOWNLOAD_SEL,    // Panel with game details.
		WID_SL_DETAILS,                 // Button to open NewGgrf configuration.
		WID_SL_NEWGRF_INFO,             // Button to load game/scenario.
		WID_SL_LOAD_BUTTON,             // Button to find missing NewGRFs online.
		WID_SL_MISSING_NEWGRFS,
	}

}
