
/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file genworld_widget.h Types related to the genworld widgets. */

namespace Nopenttd.Widgets
{
    /** Widgets of the #GenerateLandscapeWindow class. */
    public enum GenerateLandscapeWidgets
    {
// Button with icon "Temperate".
        WID_GL_TEMPERATE, // Button with icon "Arctic".
        WID_GL_ARCTIC, // Button with icon "Tropical".
        WID_GL_TROPICAL, // Button with icon "Toyland".
        WID_GL_TOYLAND,

// Dropdown 'map X size'.
        WID_GL_MAPSIZE_X_PULLDOWN, // Dropdown 'map Y size'.
        WID_GL_MAPSIZE_Y_PULLDOWN,

// Dropdown 'No. of towns'.
        WID_GL_TOWN_PULLDOWN, // Dropdown 'No. of industries'.
        WID_GL_INDUSTRY_PULLDOWN,

// 'Generate' button.
        WID_GL_GENERATE_BUTTON,

// Decrease max. heightlevel
        WID_GL_MAX_HEIGHTLEVEL_DOWN, // Max. heightlevel
        WID_GL_MAX_HEIGHTLEVEL_TEXT, // Increase max. heightlevel
        WID_GL_MAX_HEIGHTLEVEL_UP,

// Decrease start year.
        WID_GL_START_DATE_DOWN, // Start year.
        WID_GL_START_DATE_TEXT, // Increase start year.
        WID_GL_START_DATE_UP,

// Decrease snow level.
        WID_GL_SNOW_LEVEL_DOWN, // Snow level.
        WID_GL_SNOW_LEVEL_TEXT, // Increase snow level.
        WID_GL_SNOW_LEVEL_UP,

// Dropdown 'Tree algorithm'.
        WID_GL_TREE_PULLDOWN, // Dropdown 'Land generator'.
        WID_GL_LANDSCAPE_PULLDOWN,

// Heightmap name.
        WID_GL_HEIGHTMAP_NAME_TEXT, // Size of heightmap.
        WID_GL_HEIGHTMAP_SIZE_TEXT, // Dropdown 'Heightmap rotation'.
        WID_GL_HEIGHTMAP_ROTATION_PULLDOWN,

// Dropdown 'Terrain type'.
        WID_GL_TERRAIN_PULLDOWN, // Dropdown 'Sea level'.
        WID_GL_WATER_PULLDOWN, // Dropdown 'Rivers'.
        WID_GL_RIVER_PULLDOWN, // Dropdown 'Smoothness'.
        WID_GL_SMOOTHNESS_PULLDOWN, // Dropdown 'Variety distribution'.
        WID_GL_VARIETY_PULLDOWN,

// 'Random'/'Manual' borders.
        WID_GL_BORDERS_RANDOM, // NW 'Water'/'Freeform'.
        WID_GL_WATER_NW, // NE 'Water'/'Freeform'.
        WID_GL_WATER_NE, // SE 'Water'/'Freeform'.
        WID_GL_WATER_SE, // SW 'Water'/'Freeform'.
        WID_GL_WATER_SW,
    }

/** Widgets of the #CreateScenarioWindow class. */
    public enum CreateScenarioWidgets
    {
// Select temperate landscape style.
        WID_CS_TEMPERATE, // Select arctic landscape style.
        WID_CS_ARCTIC, // Select tropical landscape style.
        WID_CS_TROPICAL, // Select toy-land landscape style.
        WID_CS_TOYLAND, // Generate an empty flat world.
        WID_CS_EMPTY_WORLD, // Generate random land button
        WID_CS_RANDOM_WORLD, // Pull-down arrow for x map size.
        WID_CS_MAPSIZE_X_PULLDOWN, // Pull-down arrow for y map size.
        WID_CS_MAPSIZE_Y_PULLDOWN, // Decrease start year (start earlier).
        WID_CS_START_DATE_DOWN, // Clickable start date value.
        WID_CS_START_DATE_TEXT, // Increase start year (start later).
        WID_CS_START_DATE_UP, // Decrease flat land height.
        WID_CS_FLAT_LAND_HEIGHT_DOWN, // Clickable flat land height value.
        WID_CS_FLAT_LAND_HEIGHT_TEXT, // Increase flat land height.
        WID_CS_FLAT_LAND_HEIGHT_UP,
    }

/** Widgets of the #GenerateProgressWindow class. */
    public enum GenerationProgressWidgets
    {
// Progress bar.
        WID_GP_PROGRESS_BAR, // Text with the progress bar.
        WID_GP_PROGRESS_TEXT, // Abort button.
        WID_GP_ABORT,
    }
}
