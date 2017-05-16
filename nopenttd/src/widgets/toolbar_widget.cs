    /*
     * This file is part of OpenTTD.
     * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
     * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
     * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
     */

    /** @file toolbar_widget.h Types related to the toolbar widgets. */
namespace Nopenttd.Widgets
{

    /** Widgets of the #MainToolbarWindow class. */
    public enum ToolbarNormalWidgets
    {
// Pause the game.
        WID_TN_PAUSE, // Fast forward the game.
        WID_TN_FAST_FORWARD, // Settings menu.
        WID_TN_SETTINGS, // Save menu.
        WID_TN_SAVE, // Small map menu.
        WID_TN_SMALL_MAP, // Town menu.
        WID_TN_TOWNS, // Subsidy menu.
        WID_TN_SUBSIDIES, // Station menu.
        WID_TN_STATIONS, // Finance menu.
        WID_TN_FINANCES, // Company menu.
        WID_TN_COMPANIES, // Story menu.
        WID_TN_STORY, // Goal menu.
        WID_TN_GOAL, // Graph menu.
        WID_TN_GRAPHS, // Company league menu.
        WID_TN_LEAGUE, // Industry menu.
        WID_TN_INDUSTRIES, // Helper for the offset of the vehicle menus.
        WID_TN_VEHICLE_START, // Train menu.
        WID_TN_TRAINS = WID_TN_VEHICLE_START, // Road vehicle menu.
        WID_TN_ROADVEHS, // Ship menu.
        WID_TN_SHIPS, // Aircraft menu.
        WID_TN_AIRCRAFTS, // Zoom in the main viewport.
        WID_TN_ZOOM_IN, // Zoom out the main viewport.
        WID_TN_ZOOM_OUT, // Helper for the offset of the building tools
        WID_TN_BUILDING_TOOLS_START, // Rail building menu.
        WID_TN_RAILS = WID_TN_BUILDING_TOOLS_START, // Road building menu.
        WID_TN_ROADS, // Water building toolbar.
        WID_TN_WATER, // Airport building toolbar.
        WID_TN_AIR, // Landscaping toolbar.
        WID_TN_LANDSCAPE, // Music/sound configuration menu.
        WID_TN_MUSIC_SOUND, // Messages menu.
        WID_TN_MESSAGES, // Help menu.
        WID_TN_HELP, // Only available when toolbar has been split to switch between different subsets.
        WID_TN_SWITCH_BAR, // Helper for knowing the amount of widgets.
        WID_TN_END,
    }

/** Widgets of the #ScenarioEditorToolbarWindow class. */
    public enum ToolbarEditorWidgets
    {
// Pause the game.
        WID_TE_PAUSE, // Fast forward the game.
        WID_TE_FAST_FORWARD, // Settings menu.
        WID_TE_SETTINGS, // Save menu.
        WID_TE_SAVE, // Spacer with "scenario editor" text.
        WID_TE_SPACER, // The date of the scenario.
        WID_TE_DATE, // Reduce the date of the scenario.
        WID_TE_DATE_BACKWARD, // Increase the date of the scenario.
        WID_TE_DATE_FORWARD, // Small map menu.
        WID_TE_SMALL_MAP, // Zoom in the main viewport.
        WID_TE_ZOOM_IN, // Zoom out the main viewport.
        WID_TE_ZOOM_OUT, // Land generation.
        WID_TE_LAND_GENERATE, // Town building window.
        WID_TE_TOWN_GENERATE, // Industry building window.
        WID_TE_INDUSTRY, // Road building menu.
        WID_TE_ROADS, // Water building toolbar.
        WID_TE_WATER, // Tree building toolbar.
        WID_TE_TREES, // Sign building.
        WID_TE_SIGNS, // Container for the date widgets.
        WID_TE_DATE_PANEL,

        /* The following three need to have the same actual widget number as the normal toolbar due to shared code. */ // Music/sound configuration menu.
        WID_TE_MUSIC_SOUND = ToolbarNormalWidgets.WID_TN_MUSIC_SOUND, // Help menu.
        WID_TE_HELP = ToolbarNormalWidgets.WID_TN_HELP, // Only available when toolbar has been split to switch between different subsets.
        WID_TE_SWITCH_BAR = ToolbarNormalWidgets.WID_TN_SWITCH_BAR,
    }
}