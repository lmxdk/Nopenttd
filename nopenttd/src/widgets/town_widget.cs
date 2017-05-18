/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file town_widget.h Types related to the town widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #TownDirectoryWindow class. */
    public enum TownDirectoryWidgets
    {
// Direction of sort dropdown.
        WID_TD_SORT_ORDER, // Criteria of sort dropdown.
        WID_TD_SORT_CRITERIA, // List of towns.
        WID_TD_LIST, // Scrollbar for the town list.
        WID_TD_SCROLLBAR, // The world's population.
        WID_TD_WORLD_POPULATION,
    }

/** Widgets of the #TownAuthorityWindow class. */
    public enum TownAuthorityWidgets
    {
// Caption of window.
        WID_TA_CAPTION, // Overview with ratings for each company.
        WID_TA_RATING_INFO, // List of commands for the player.
        WID_TA_COMMAND_LIST, // Scrollbar of the list of commands.
        WID_TA_SCROLLBAR, // Additional information about the action.
        WID_TA_ACTION_INFO, // Do-it button.
        WID_TA_EXECUTE,
    }

/** Widgets of the #TownViewWindow class. */
    public enum TownViewWidgets
    {
// Caption of window.
        WID_TV_CAPTION, // View of the center of the town.
        WID_TV_VIEWPORT, // General information about the town.
        WID_TV_INFO, // Center the main view on this town.
        WID_TV_CENTER_VIEW, // Show the town authority window.
        WID_TV_SHOW_AUTHORITY, // Change the name of this town.
        WID_TV_CHANGE_NAME, // Expand this town (scenario editor only).
        WID_TV_EXPAND, // Delete this town (scenario editor only).
        WID_TV_DELETE,
    }

/** Widgets of the #FoundTownWindow class. */
    public enum TownFoundingWidgets
    {
// Create a new town.
        WID_TF_NEW_TOWN, // Randomly place a town.
        WID_TF_RANDOM_TOWN, // Randomly place many towns.
        WID_TF_MANY_RANDOM_TOWNS, // Editor for the town name.
        WID_TF_TOWN_NAME_EDITBOX, // Generate a random town name.
        WID_TF_TOWN_NAME_RANDOM, // Selection for a small town.
        WID_TF_SIZE_SMALL, // Selection for a medium town.
        WID_TF_SIZE_MEDIUM, // Selection for a large town.
        WID_TF_SIZE_LARGE, // Selection for a randomly sized town.
        WID_TF_SIZE_RANDOM, // Selection for the town's city state.
        WID_TF_CITY, // Selection for the original town layout.
        WID_TF_LAYOUT_ORIGINAL, // Selection for the better town layout.
        WID_TF_LAYOUT_BETTER, // Selection for the 2x2 grid town layout.
        WID_TF_LAYOUT_GRID2, // Selection for the 3x3 grid town layout.
        WID_TF_LAYOUT_GRID3, // Selection for a randomly chosen town layout.
        WID_TF_LAYOUT_RANDOM,
    }
}