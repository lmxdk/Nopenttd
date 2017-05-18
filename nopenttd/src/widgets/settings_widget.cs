/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file settings_widget.h Types related to the settings widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #GameOptionsWindow class. */
    public enum GameOptionsWidgets
    {
// Background of the window.
        WID_GO_BACKGROUND, // Currency dropdown.
        WID_GO_CURRENCY_DROPDOWN, // Measuring unit dropdown.
        WID_GO_DISTANCE_DROPDOWN, // Dropdown to select the road side (to set the right side ;)).
        WID_GO_ROADSIDE_DROPDOWN, // Town name dropdown.
        WID_GO_TOWNNAME_DROPDOWN, // Dropdown to say how often to autosave.
        WID_GO_AUTOSAVE_DROPDOWN, // Language dropdown.
        WID_GO_LANG_DROPDOWN, // Dropdown for the resolution.
        WID_GO_RESOLUTION_DROPDOWN, // Toggle fullscreen.
        WID_GO_FULLSCREEN_BUTTON, // Dropdown for the GUI zoom level.
        WID_GO_GUI_ZOOM_DROPDOWN, // Use to select a base GRF.
        WID_GO_BASE_GRF_DROPDOWN, // Info about missing files etc.
        WID_GO_BASE_GRF_STATUS, // Open base GRF readme, changelog (+1) or license (+2).
        WID_GO_BASE_GRF_TEXTFILE, // Description of selected base GRF.
        WID_GO_BASE_GRF_DESCRIPTION = WID_GO_BASE_GRF_TEXTFILE + TFT_END, // Use to select a base SFX.
        WID_GO_BASE_SFX_DROPDOWN, // Open base SFX readme, changelog (+1) or license (+2).
        WID_GO_BASE_SFX_TEXTFILE, // Description of selected base SFX.
        WID_GO_BASE_SFX_DESCRIPTION = WID_GO_BASE_SFX_TEXTFILE + TFT_END, // Use to select a base music set.
        WID_GO_BASE_MUSIC_DROPDOWN, // Info about corrupted files etc.
        WID_GO_BASE_MUSIC_STATUS, // Open base music readme, changelog (+1) or license (+2).
        WID_GO_BASE_MUSIC_TEXTFILE, // Description of selected base music set.
        WID_GO_BASE_MUSIC_DESCRIPTION = WID_GO_BASE_MUSIC_TEXTFILE + TFT_END,
    }

/** Widgets of the #GameSettingsWindow class. */
    public enum GameSettingsWidgets
    {
// Text filter.
        WID_GS_FILTER, // Panel widget containing the option lists.
        WID_GS_OPTIONSPANEL, // Scrollbar.
        WID_GS_SCROLLBAR, // Information area to display help text of the selected option.
        WID_GS_HELP_TEXT, // Expand all button.
        WID_GS_EXPAND_ALL, // Collapse all button.
        WID_GS_COLLAPSE_ALL, // Label upfront to the category drop-down box to restrict the list of settings to show
        WID_GS_RESTRICT_CATEGORY, // Label upfront to the type drop-down box to restrict the list of settings to show
        WID_GS_RESTRICT_TYPE, // The drop down box to restrict the list of settings
        WID_GS_RESTRICT_DROPDOWN, // The drop down box to choose client/game/company/all settings
        WID_GS_TYPE_DROPDOWN,
    }

/** Widgets of the #CustomCurrencyWindow class. */
    public enum CustomCurrencyWidgets
    {
// Down button.
        WID_CC_RATE_DOWN, // Up button.
        WID_CC_RATE_UP, // Rate of currency.
        WID_CC_RATE, // Separator edit button.
        WID_CC_SEPARATOR_EDIT, // Current separator.
        WID_CC_SEPARATOR, // Prefix edit button.
        WID_CC_PREFIX_EDIT, // Current prefix.
        WID_CC_PREFIX, // Suffix edit button.
        WID_CC_SUFFIX_EDIT, // Current suffix.
        WID_CC_SUFFIX, // Down button.
        WID_CC_YEAR_DOWN, // Up button.
        WID_CC_YEAR_UP, // Year of introduction.
        WID_CC_YEAR, // Preview.
        WID_CC_PREVIEW,
    }
}
