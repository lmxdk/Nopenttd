    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file newgrf_widget.h Types related to the newgrf widgets. */

namespace Nopenttd.Widgets
{


    /** Widgets of the #NewGRFParametersWindow class. */
    public enum NewGRFParametersWidgets
    {
// #NWID_SELECTION to optionally display #WID_NP_NUMPAR.
        WID_NP_SHOW_NUMPAR, // Button to decrease number of parameters.
        WID_NP_NUMPAR_DEC, // Button to increase number of parameters.
        WID_NP_NUMPAR_INC, // Optional number of parameters.
        WID_NP_NUMPAR, // Text description.
        WID_NP_NUMPAR_TEXT, // Panel to draw the settings on.
        WID_NP_BACKGROUND, // Scrollbar to scroll through all settings.
        WID_NP_SCROLLBAR, // Accept button.
        WID_NP_ACCEPT, // Reset button.
        WID_NP_RESET, // #NWID_SELECTION to optionally display parameter descriptions.
        WID_NP_SHOW_DESCRIPTION, // Multi-line description of a parameter.
        WID_NP_DESCRIPTION,
    }

/** Widgets of the #NewGRFWindow class. */
    public enum NewGRFStateWidgets
    {
// Active NewGRF preset.
        WID_NS_PRESET_LIST, // Save list of active NewGRFs as presets.
        WID_NS_PRESET_SAVE, // Delete active preset.
        WID_NS_PRESET_DELETE, // Add NewGRF to active list.
        WID_NS_ADD, // Remove NewGRF from active list.
        WID_NS_REMOVE, // Move NewGRF up in active list.
        WID_NS_MOVE_UP, // Move NewGRF down in active list.
        WID_NS_MOVE_DOWN, // Upgrade NewGRFs that have a newer version available.
        WID_NS_UPGRADE, // Filter list of available NewGRFs.
        WID_NS_FILTER, // List window of active NewGRFs.
        WID_NS_FILE_LIST, // Scrollbar for active NewGRF list.
        WID_NS_SCROLLBAR, // List window of available NewGRFs.
        WID_NS_AVAIL_LIST, // Scrollbar for available NewGRF list.
        WID_NS_SCROLL2BAR, // Title for Info on selected NewGRF.
        WID_NS_NEWGRF_INFO_TITLE, // Panel for Info on selected NewGRF.
        WID_NS_NEWGRF_INFO, // Open URL of NewGRF.
        WID_NS_OPEN_URL, // Open NewGRF readme, changelog (+1) or license (+2).
        WID_NS_NEWGRF_TEXTFILE, // Open Parameters Window for selected NewGRF for editing parameters.

        WID_NS_SET_PARAMETERS =
            WID_NS_NEWGRF_TEXTFILE + TFT_END, // Open Parameters Window for selected NewGRF for viewing parameters.
        WID_NS_VIEW_PARAMETERS, // Toggle Palette of selected, active NewGRF.
        WID_NS_TOGGLE_PALETTE, // Apply changes to NewGRF config.
        WID_NS_APPLY_CHANGES, // Rescan files (available NewGRFs).
        WID_NS_RESCAN_FILES, // Rescan files (active NewGRFs).
        WID_NS_RESCAN_FILES2, // Open content download (available NewGRFs).
        WID_NS_CONTENT_DOWNLOAD, // Open content download (active NewGRFs).
        WID_NS_CONTENT_DOWNLOAD2, // Select active list buttons (0 = normal, 1 = simple layout).
        WID_NS_SHOW_REMOVE, // Select display of the buttons below the 'details'.
        WID_NS_SHOW_APPLY,
    }

/** Widgets of the #SavePresetWindow class. */
    public enum SavePresetWidgets
    {
// List with available preset names.
        WID_SVP_PRESET_LIST, // Scrollbar for the list available preset names.
        WID_SVP_SCROLLBAR, // Edit box for changing the preset name.
        WID_SVP_EDITBOX, // Button to cancel saving the preset.
        WID_SVP_CANCEL, // Button to save the preset.
        WID_SVP_SAVE,
    }

/** Widgets of the #ScanProgressWindow class. */
    public enum ScanProgressWidgets
    {
// Simple progress bar.
        WID_SP_PROGRESS_BAR, // Text explaining what is happening.
        WID_SP_PROGRESS_TEXT,
    }

}