/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file ai_widget.h Types related to the ai widgets. */
namespace Nopenttd.Widgets
{

    /** Widgets of the #AIListWindow class. */
    public enum AIListWidgets
    {// Caption of the window.
        WID_AIL_CAPTION,   // The matrix with all available AIs.
        WID_AIL_LIST,      // Scrollbar next to the AI list.
        WID_AIL_SCROLLBAR, // Panel to draw some AI information on.
        WID_AIL_INFO_BG,   // Accept button.
        WID_AIL_ACCEPT,    // Cancel button.
        WID_AIL_CANCEL,
    };

    /** Widgets of the #AISettingsWindow class. */
    public enum AISettingsWidgets
    {// Caption of the window.
        WID_AIS_CAPTION,    // Panel to draw the settings on.
        WID_AIS_BACKGROUND, // Scrollbar to scroll through all settings.
        WID_AIS_SCROLLBAR,  // Accept button.
        WID_AIS_ACCEPT,     // Reset button.
        WID_AIS_RESET,
    };

    /** Widgets of the #AIConfigWindow class. */
    public enum AIConfigWidgets
    {// Window background.
        WID_AIC_BACKGROUND,       // Decrease the number of AIs.
        WID_AIC_DECREASE,         // Increase the number of AIs.
        WID_AIC_INCREASE,         // Number of AIs.
        WID_AIC_NUMBER,           // List with current selected GameScript.
        WID_AIC_GAMELIST,         // List with currently selected AIs.
        WID_AIC_LIST,             // Scrollbar to scroll through the selected AIs.
        WID_AIC_SCROLLBAR,        // Move up button.
        WID_AIC_MOVE_UP,          // Move down button.
        WID_AIC_MOVE_DOWN,        // Select another AI button.
        WID_AIC_CHANGE,           // Change AI settings button.
        WID_AIC_CONFIGURE,        // Close window button.
        WID_AIC_CLOSE,            // Open AI readme, changelog (+1) or license (+2).
        WID_AIC_TEXTFILE,         // Download content button.
        WID_AIC_CONTENT_DOWNLOAD = WID_AIC_TEXTFILE + TFT_END,
    };

    /** Widgets of the #AIDebugWindow class. */
    public enum AIDebugWidgets
    {// The row of company buttons.
        WID_AID_VIEW,                 // Name of the current selected.
        WID_AID_NAME_TEXT,            // Settings button.
        WID_AID_SETTINGS,             // Game Script button.
        WID_AID_SCRIPT_GAME,          // Reload button.
        WID_AID_RELOAD_TOGGLE,        // Panel where the log is in.
        WID_AID_LOG_PANEL,            // Scrollbar of the log panel.
        WID_AID_SCROLLBAR,            // Buttons in the VIEW.
        WID_AID_COMPANY_BUTTON_START, // Last possible button in the VIEW.
        WID_AID_COMPANY_BUTTON_END = WID_AID_COMPANY_BUTTON_START + MAX_COMPANIES - 1, // The panel to handle the breaking on string.
        WID_AID_BREAK_STRING_WIDGETS, // Enable breaking on string.
        WID_AID_BREAK_STR_ON_OFF_BTN, // Edit box for the string to break on.
        WID_AID_BREAK_STR_EDIT_BOX,   // Checkbox to use match caching or not.
        WID_AID_MATCH_CASE_BTN,       // Continue button.
        WID_AID_CONTINUE_BTN,
    };

}

